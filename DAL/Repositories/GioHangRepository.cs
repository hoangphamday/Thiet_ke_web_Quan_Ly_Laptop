using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Repositories
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly string _connectionString;

        public GioHangRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private GioHang GetOrCreateCart(string maKH)
        {
            GioHang cart = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM GioHang WHERE MaKH = @MaKH", conn))
                {
                    cmd.Parameters.AddWithValue("@MaKH", maKH);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cart = new GioHang
                            {
                                maGioHang = reader["MaGioHang"]?.ToString(),
                                maKhachHang = reader["MaKH"]?.ToString(),
                                ngayTao = reader["NgayTao"] != DBNull.Value ? Convert.ToDateTime(reader["NgayTao"]) : DateTime.MinValue
                            };
                        }
                    }
                }
                
                if (cart == null)
                {
                    cart = new GioHang
                    {
                        maGioHang = Guid.NewGuid().ToString(),
                        maKhachHang = maKH,
                        ngayTao = DateTime.Now
                    };
                    using (SqlCommand insertCmd = new SqlCommand("INSERT INTO GioHang(MaGioHang, MaKH, NgayTao) VALUES(@ID, @MaKH, @NgayTao)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@ID", cart.maGioHang);
                        insertCmd.Parameters.AddWithValue("@MaKH", cart.maKhachHang);
                        insertCmd.Parameters.AddWithValue("@NgayTao", cart.ngayTao);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            return cart;
        }

        public IEnumerable<ChiTietGioHang> GetCartDetails(string maKH)
        {
            var cart = GetOrCreateCart(maKH);
            var list = new List<ChiTietGioHang>();
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ChiTietGioHang WHERE MaGioHang = @CartId", conn))
                {
                    cmd.Parameters.AddWithValue("@CartId", cart.maGioHang);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietGioHang
                            {
                                maChiTietGioHang = reader["MaCTGH"]?.ToString(),
                                maGioHang = reader["MaGioHang"]?.ToString(),
                                maLaptop = reader["MaLaptop"]?.ToString(),
                                soLuong = reader["SoLuong"]?.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool AddToCart(string maKH, string maLaptop, int soLuong)
        {
            var cart = GetOrCreateCart(maKH);
            
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string ctghId = null;
                int currentQty = 0;

                // Check if already exists
                using (SqlCommand checkCmd = new SqlCommand("SELECT MaCTGH, SoLuong FROM ChiTietGioHang WHERE MaGioHang = @CartId AND MaLaptop = @MaLaptop", conn))
                {
                    checkCmd.Parameters.AddWithValue("@CartId", cart.maGioHang);
                    checkCmd.Parameters.AddWithValue("@MaLaptop", maLaptop);
                    using (var reader = checkCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ctghId = reader["MaCTGH"].ToString();
                            currentQty = Convert.ToInt32(reader["SoLuong"]);
                        }
                    }
                }

                if (ctghId != null)
                {
                    // Update
                    int newQty = currentQty + soLuong;
                    using (SqlCommand updateCmd = new SqlCommand("UPDATE ChiTietGioHang SET SoLuong = @Qty WHERE MaCTGH = @Id", conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Qty", newQty);
                        updateCmd.Parameters.AddWithValue("@Id", ctghId);
                        return updateCmd.ExecuteNonQuery() > 0;
                    }
                }
                else
                {
                    // Insert utilizing provided procedure
                    using (SqlCommand cmd = new SqlCommand("sp_GioHang_AddItem", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaCTGH", Guid.NewGuid().ToString());
                        cmd.Parameters.AddWithValue("@MaGioHang", cart.maGioHang);
                        cmd.Parameters.AddWithValue("@MaLaptop", maLaptop);
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                        
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
        }

        public bool UpdateQuantity(string maCTGH, int soLuong)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                if (soLuong <= 0)
                {
                    using (SqlCommand delCmd = new SqlCommand("DELETE FROM ChiTietGioHang WHERE MaCTGH = @Id", conn))
                    {
                        delCmd.Parameters.AddWithValue("@Id", maCTGH);
                        return delCmd.ExecuteNonQuery() > 0;
                    }
                }
                else
                {
                    using (SqlCommand updateCmd = new SqlCommand("UPDATE ChiTietGioHang SET SoLuong = @Qty WHERE MaCTGH = @Id", conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Qty", soLuong);
                        updateCmd.Parameters.AddWithValue("@Id", maCTGH);
                        return updateCmd.ExecuteNonQuery() > 0;
                    }
                }
            }
        }

        public bool RemoveFromCart(string maCTGH)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM ChiTietGioHang WHERE MaCTGH = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", maCTGH);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool ClearCart(string maKH)
        {
            var cart = GetOrCreateCart(maKH);
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM ChiTietGioHang WHERE MaGioHang = @CartId", conn))
                {
                    cmd.Parameters.AddWithValue("@CartId", cart.maGioHang);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

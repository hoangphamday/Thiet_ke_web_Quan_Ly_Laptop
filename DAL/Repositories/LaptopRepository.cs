using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Repositories
{
    public class LaptopRepository : ILaptopRepository
    {
        private readonly string _connectionString;

        public LaptopRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private Laptop MapFromReader(SqlDataReader reader)
        {
            return new Laptop
            {
                maLaptop = reader["MaLaptop"]?.ToString(),
                tenLaptop = reader["TenLaptop"]?.ToString(),
                maHang = reader["MaHang"]?.ToString(),
                CPU = reader["CPU"]?.ToString(),
                RAM = reader["RAM"]?.ToString(),
                SSD = reader["SSD"]?.ToString(),
                GPU = reader["GPU"]?.ToString(),
                manHinh = reader["ManHinh"]?.ToString(),
                Gia = reader["Gia"]?.ToString(),
                soLuong = reader["SoLuong"]?.ToString(),
                baoHanh = reader["BaoHanh"]?.ToString(),
                moTa = reader["MoTa"]?.ToString(),
                ngayThem = reader["NgayThem"] != DBNull.Value ? Convert.ToDateTime(reader["NgayThem"]) : DateTime.MinValue
            };
        }

        public bool Add(Laptop laptop, List<HinhAnhLaptop> images)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_Laptop_Create", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaLaptop", laptop.maLaptop ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@TenLaptop", laptop.tenLaptop ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaHang", laptop.maHang ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@CPU", laptop.CPU ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@RAM", laptop.RAM ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@SSD", laptop.SSD ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@GPU", laptop.GPU ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@ManHinh", laptop.manHinh ?? (object)DBNull.Value);
                            
                            decimal gia = 0; decimal.TryParse(laptop.Gia, out gia);
                            cmd.Parameters.AddWithValue("@Gia", gia);
                            
                            int sl = 0; int.TryParse(laptop.soLuong, out sl);
                            cmd.Parameters.AddWithValue("@SoLuong", sl);
                            
                            int bh = 0; int.TryParse(laptop.baoHanh, out bh);
                            cmd.Parameters.AddWithValue("@BaoHanh", bh);
                            
                            cmd.Parameters.AddWithValue("@MoTa", laptop.moTa ?? (object)DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }

                        if (images != null && images.Count > 0)
                        {
                            foreach (var img in images)
                            {
                                using (SqlCommand imgCmd = new SqlCommand("INSERT INTO HinhAnhLaptop(MaHinh, MaLaptop, DuongDanAnh) VALUES(@MaHinh, @MaLaptop, @Path)", conn, transaction))
                                {
                                    imgCmd.Parameters.AddWithValue("@MaHinh", img.maHinhAnh ?? Guid.NewGuid().ToString());
                                    imgCmd.Parameters.AddWithValue("@MaLaptop", img.maLaptop ?? laptop.maLaptop);
                                    imgCmd.Parameters.AddWithValue("@Path", img.duongDanAnh ?? "");
                                    imgCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Delete related HinhAnhLaptop
                        using (SqlCommand delImg = new SqlCommand("DELETE FROM HinhAnhLaptop WHERE MaLaptop = @Id", conn, transaction))
                        {
                            delImg.Parameters.AddWithValue("@Id", id);
                            delImg.ExecuteNonQuery();
                        }
                        
                        // Delete related DanhGia
                        using (SqlCommand delDG = new SqlCommand("DELETE FROM DanhGia WHERE MaLaptop = @Id", conn, transaction))
                        {
                            delDG.Parameters.AddWithValue("@Id", id);
                            delDG.ExecuteNonQuery();
                        }

                        // Call sp_Laptop_Delete
                        using (SqlCommand cmd = new SqlCommand("sp_Laptop_Delete", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaLaptop", id);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public IEnumerable<Laptop> GetAll()
        {
            var list = new List<Laptop>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Laptop_GetAll", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapFromReader(reader));
                        }
                    }
                }
            }
            return list;
        }

        public Laptop GetById(string id)
        {
            Laptop result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Laptop_GetById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaLaptop", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = MapFromReader(reader);
                        }
                    }
                }
            }
            return result;
        }

        public bool Update(Laptop laptop, List<HinhAnhLaptop> images)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // sp_Laptop_Update provided by user script only updates TenLaptop, Gia, SoLuong
                        using (SqlCommand cmd = new SqlCommand("sp_Laptop_Update", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaLaptop", laptop.maLaptop);
                            cmd.Parameters.AddWithValue("@TenLaptop", laptop.tenLaptop ?? (object)DBNull.Value);
                            
                            decimal gia = 0; decimal.TryParse(laptop.Gia, out gia);
                            cmd.Parameters.AddWithValue("@Gia", gia);
                            
                            int sl = 0; int.TryParse(laptop.soLuong, out sl);
                            cmd.Parameters.AddWithValue("@SoLuong", sl);

                            cmd.ExecuteNonQuery();
                        }

                        // Since user specifically said update ALL images, we'll replace them
                        if (images != null)
                        {
                            using (SqlCommand delImg = new SqlCommand("DELETE FROM HinhAnhLaptop WHERE MaLaptop = @Id", conn, transaction))
                            {
                                delImg.Parameters.AddWithValue("@Id", laptop.maLaptop);
                                delImg.ExecuteNonQuery();
                            }

                            foreach (var img in images)
                            {
                                using (SqlCommand imgCmd = new SqlCommand("INSERT INTO HinhAnhLaptop(MaHinh, MaLaptop, DuongDanAnh) VALUES(@MaHinh, @MaLaptop, @Path)", conn, transaction))
                                {
                                    imgCmd.Parameters.AddWithValue("@MaHinh", img.maHinhAnh ?? Guid.NewGuid().ToString());
                                    imgCmd.Parameters.AddWithValue("@MaLaptop", laptop.maLaptop);
                                    imgCmd.Parameters.AddWithValue("@Path", img.duongDanAnh ?? "");
                                    imgCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public IEnumerable<HangLaptop> GetAllBrands()
        {
            var list = new List<HangLaptop>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM HangLaptop", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new HangLaptop
                            {
                                maHang = reader["MaHang"]?.ToString(),
                                tenHang = reader["TenHang"]?.ToString(),
                                quocGia = reader["QuocGia"]?.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public IEnumerable<DanhGia> GetReviews(string maLaptop)
        {
            var list = new List<DanhGia>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM DanhGia WHERE MaLaptop = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", maLaptop);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new DanhGia
                            {
                                maDanhGia = reader["MaDanhGia"]?.ToString(),
                                maLaptop = reader["MaLaptop"]?.ToString(),
                                maKhachHang = reader["MaKH"]?.ToString(),
                                soSao = reader["SoSao"] != DBNull.Value ? Convert.ToDecimal(reader["SoSao"]) : 0,
                                noiDung = reader["NoiDung"]?.ToString(),
                                ngayDanhGia = reader["NgayDanhGia"] != DBNull.Value ? Convert.ToDateTime(reader["NgayDanhGia"]) : DateTime.MinValue
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool AddReview(DanhGia review)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DanhGia_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaDanhGia", review.maDanhGia ?? Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("@MaLaptop", review.maLaptop);
                    cmd.Parameters.AddWithValue("@MaKH", review.maKhachHang);
                    cmd.Parameters.AddWithValue("@SoSao", review.soSao);
                    cmd.Parameters.AddWithValue("@NoiDung", review.noiDung ?? (object)DBNull.Value);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

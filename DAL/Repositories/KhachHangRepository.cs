using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Repositories
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly string _connectionString;

        public KhachHangRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private KhachHang MapFromReader(SqlDataReader reader)
        {
            return new KhachHang
            {
                maKhachHang = reader["MaKH"]?.ToString(),
                tenKhachHang = reader["TenKH"]?.ToString(),
                dienThoai = reader["DienThoai"]?.ToString(),
                email = reader["Email"]?.ToString(),
                diaChi = reader["DiaChi"]?.ToString(),
                ngayDangKy = reader["NgayDangKy"] != DBNull.Value ? Convert.ToDateTime(reader["NgayDangKy"]) : DateTime.MinValue,
                maTaiKhoan = reader["MaTK"]?.ToString()
            };
        }

        public IEnumerable<KhachHang> GetAll()
        {
            var list = new List<KhachHang>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_KhachHang_GetAll", conn))
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

        public KhachHang GetById(string id)
        {
            KhachHang result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM KhachHang WHERE MaKH = @MaKH", conn))
                {
                    cmd.Parameters.AddWithValue("@MaKH", id);
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

        public bool Add(KhachHang khachHang)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_KhachHang_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaKH", khachHang.maKhachHang ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenKH", khachHang.tenKhachHang ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DienThoai", khachHang.dienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", khachHang.email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiaChi", khachHang.diaChi ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTK", khachHang.maTaiKhoan ?? (object)DBNull.Value);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(KhachHang khachHang)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE KhachHang SET TenKH = @TenKH, DienThoai = @DienThoai, Email = @Email, DiaChi = @DiaChi, MaTK = @MaTK WHERE MaKH = @MaKH", conn))
                {
                    cmd.Parameters.AddWithValue("@MaKH", khachHang.maKhachHang ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenKH", khachHang.tenKhachHang ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DienThoai", khachHang.dienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", khachHang.email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiaChi", khachHang.diaChi ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MaTK", khachHang.maTaiKhoan ?? (object)DBNull.Value);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM KhachHang WHERE MaKH = @MaKH", conn))
                {
                    cmd.Parameters.AddWithValue("@MaKH", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

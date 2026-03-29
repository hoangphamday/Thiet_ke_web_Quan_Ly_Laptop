using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Repositories
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly string _connectionString;

        public TaiKhoanRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private TaiKhoan MapFromReader(SqlDataReader reader)
        {
            return new TaiKhoan
            {
                maTaiKhoan = reader["MaTK"].ToString(),
                tenDangNhap = reader["TenDangNhap"].ToString(),
                matKhau = reader["MatKhau"].ToString(),
                vaiTro = reader["Role"].ToString(),
                trangThai = Convert.ToBoolean(reader["TrangThai"]) ? "Hoạt động" : "Khóa"
            };
        }

        public IEnumerable<TaiKhoan> GetAll()
        {
            var list = new List<TaiKhoan>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM TaiKhoan", conn))
                {
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

        public TaiKhoan GetById(string id)
        {
            TaiKhoan result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM TaiKhoan WHERE MaTK = @MaTK", conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", id);
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

        public bool Add(TaiKhoan taiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_Register", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaTK", taiKhoan.maTaiKhoan);
                    cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoan.tenDangNhap);
                    cmd.Parameters.AddWithValue("@MatKhau", taiKhoan.matKhau);
                    cmd.Parameters.AddWithValue("@Role", taiKhoan.vaiTro);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(TaiKhoan taiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // Convert string "Hoạt động" / "Khóa" to BIT
                bool trangThaiBit = (taiKhoan.trangThai == "Hoạt động");
                using (SqlCommand cmd = new SqlCommand("UPDATE TaiKhoan SET TenDangNhap = @TenDangNhap, MatKhau = @MatKhau, Role = @Role, TrangThai = @TrangThai WHERE MaTK = @MaTK", conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", taiKhoan.maTaiKhoan);
                    cmd.Parameters.AddWithValue("@TenDangNhap", taiKhoan.tenDangNhap);
                    cmd.Parameters.AddWithValue("@MatKhau", taiKhoan.matKhau);
                    cmd.Parameters.AddWithValue("@Role", taiKhoan.vaiTro);
                    cmd.Parameters.AddWithValue("@TrangThai", trangThaiBit);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM TaiKhoan WHERE MaTK = @MaTK", conn))
                {
                    cmd.Parameters.AddWithValue("@MaTK", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public TaiKhoan Login(string username, string password, string vaitro)
        {
            TaiKhoan result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_TaiKhoan_Login", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TenDangNhap", username);
                    cmd.Parameters.AddWithValue("@MatKhau", password);
                    
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
            // Additional check for role
            if (result != null && result.vaiTro == vaitro)
            {
                return result;
            }
            return null;
        }
    }
}

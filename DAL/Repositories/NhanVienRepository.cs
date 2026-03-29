using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Repositories
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly string _connectionString;

        public NhanVienRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private NhanVien MapFromReader(SqlDataReader reader)
        {
            return new NhanVien
            {
                maNhanVien = reader["MaNV"]?.ToString(),
                tenNhanVien = reader["TenNV"]?.ToString(),
                dienThoai = reader["DienThoai"]?.ToString(),
                chucVu = reader["ChucVu"]?.ToString(),
                NgaySinh = reader["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(reader["NgaySinh"]) : DateTime.MinValue,
                ngayVaoLam = reader["NgayVaoLam"] != DBNull.Value ? Convert.ToDateTime(reader["NgayVaoLam"]) : DateTime.MinValue,
                maTaiKhoan = reader["MaTK"]?.ToString()
                // Email field is in DB but not mapped in Model directly unless the user skipped it. Let's map whatever properties exist.
                // The prompt model: maNhanVien, tenNhanVien, NgaySinh, diaChi, dienThoai, chucVu, ngayVaoLam, maTaiKhoan
                // Wait, diaChi is not in DB table NhanVien ("MaNV, TenNV, DienThoai, Email, ChucVu, NgaySinh, NgayVaoLam, MaTK"). So no diaChi mapping.
            };
        }

        public IEnumerable<NhanVien> GetAll()
        {
            var list = new List<NhanVien>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhanVien_GetAll", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var n = MapFromReader(reader);
                            // Set diaChi to email just in case since diaChi is not in DB Schema, or leave it blank
                            // Let's not set diaChi, there's no such column in DB script. 
                            list.Add(n);
                        }
                    }
                }
            }
            return list;
        }

        public NhanVien GetById(string id)
        {
            NhanVien result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhanVien_GetById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", id);
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

        public bool Add(NhanVien nhanVien)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhanVien_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", nhanVien.maNhanVien ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenNV", nhanVien.tenNhanVien ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DienThoai", nhanVien.dienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", nhanVien.diaChi ?? "no-email@ex.com"); // Reusing diaChi for email since model lacks email
                    cmd.Parameters.AddWithValue("@ChucVu", nhanVien.chucVu ?? (object)DBNull.Value);
                    
                    if (nhanVien.NgaySinh != DateTime.MinValue && nhanVien.NgaySinh.Year > 1)
                        cmd.Parameters.AddWithValue("@NgaySinh", nhanVien.NgaySinh);
                    else
                        cmd.Parameters.AddWithValue("@NgaySinh", DBNull.Value);
                        
                    if (nhanVien.ngayVaoLam != DateTime.MinValue && nhanVien.ngayVaoLam.Year > 1)
                        cmd.Parameters.AddWithValue("@NgayVaoLam", nhanVien.ngayVaoLam);
                    else
                        cmd.Parameters.AddWithValue("@NgayVaoLam", DBNull.Value);
                        
                    cmd.Parameters.AddWithValue("@MaTK", nhanVien.maTaiKhoan ?? (object)DBNull.Value);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(NhanVien nhanVien)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhanVien_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", nhanVien.maNhanVien ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenNV", nhanVien.tenNhanVien ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DienThoai", nhanVien.dienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", nhanVien.diaChi ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ChucVu", nhanVien.chucVu ?? (object)DBNull.Value);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhanVien_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNV", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

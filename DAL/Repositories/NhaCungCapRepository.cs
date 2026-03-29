using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Repositories
{
    public class NhaCungCapRepository : INhaCungCapRepository
    {
        private readonly string _connectionString;

        public NhaCungCapRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private NhaCungCap MapFromReader(SqlDataReader reader)
        {
            return new NhaCungCap
            {
                maNhaCungCap = reader["MaNCC"]?.ToString(),
                tenNhaCungCap = reader["TenNCC"]?.ToString(),
                dienThoai = reader["DienThoai"]?.ToString(),
                email = reader["Email"]?.ToString(),
                diaChi = reader["DiaChi"]?.ToString()
            };
        }

        public IEnumerable<NhaCungCap> GetAll()
        {
            var list = new List<NhaCungCap>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_GetAll", conn))
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

        public NhaCungCap GetById(string id)
        {
            NhaCungCap result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_GetById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNCC", id);
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

        public bool Add(NhaCungCap nhaCungCap)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNCC", nhaCungCap.maNhaCungCap ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenNCC", nhaCungCap.tenNhaCungCap ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DienThoai", nhaCungCap.dienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", nhaCungCap.email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiaChi", nhaCungCap.diaChi ?? (object)DBNull.Value);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(NhaCungCap nhaCungCap)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_Update", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNCC", nhaCungCap.maNhaCungCap ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TenNCC", nhaCungCap.tenNhaCungCap ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DienThoai", nhaCungCap.dienThoai ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", nhaCungCap.email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DiaChi", nhaCungCap.diaChi ?? (object)DBNull.Value);
                    
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(string id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NhaCungCap_Delete", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaNCC", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public IEnumerable<NhaCungCap> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return GetAll();
            
            var list = new List<NhaCungCap>();
            var lowerKeyword = "%" + keyword.ToLower() + "%";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                // We'll just use a raw query for Search since there's no SP for it
                string sql = "SELECT * FROM NhaCungCap WHERE LOWER(TenNCC) LIKE @Keyword OR DienThoai LIKE @Keyword";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", lowerKeyword);
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
    }
}

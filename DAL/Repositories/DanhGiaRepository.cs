using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DanhGiaRepository : IDanhGiaRepository
    {
        private readonly string _connectionString;

        public DanhGiaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<DanhGia>> GetByLaptop(string maLaptop)
        {
            var list = new List<DanhGia>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM DanhGia WHERE MaLaptop = @MaLaptop", conn))
                {
                    cmd.Parameters.AddWithValue("@MaLaptop", maLaptop);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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

        public async Task Add(DanhGia model)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DanhGia_Create", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MaDanhGia", model.maDanhGia ?? Guid.NewGuid().ToString());
                    cmd.Parameters.AddWithValue("@MaLaptop", model.maLaptop);
                    cmd.Parameters.AddWithValue("@MaKH", model.maKhachHang);
                    cmd.Parameters.AddWithValue("@SoSao", model.soSao);
                    cmd.Parameters.AddWithValue("@NoiDung", model.noiDung ?? (object)DBNull.Value);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Delete(string maDanhGia)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM DanhGia WHERE MaDanhGia = @MaDanhGia", conn))
                {
                    cmd.Parameters.AddWithValue("@MaDanhGia", maDanhGia);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

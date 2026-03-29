using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Repositories
{
    public class DonHangRepository : IDonHangRepository
    {
        private readonly string _connectionString;

        public DonHangRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private DonHang MapFromReader(SqlDataReader reader)
        {
            return new DonHang
            {
                maDonHang = reader["MaDonHang"]?.ToString(),
                maKhachHang = reader["MaKH"]?.ToString(),
                maNhanVien = reader["MaNV"]?.ToString(),
                ngayDat = reader["NgayDat"] != DBNull.Value ? Convert.ToDateTime(reader["NgayDat"]) : DateTime.MinValue,
                tongTien = reader["TongTien"]?.ToString(),
                trangThai = reader["TrangThai"]?.ToString(),
                diaChiGiaoHang = reader["DiaChiGiaoHang"]?.ToString(),
                phuongThucThanhToan = reader["PhuongThucThanhToan"]?.ToString(),
                ghiChu = reader["GhiChu"]?.ToString()
            };
        }

        public IEnumerable<DonHang> GetAll()
        {
            var list = new List<DonHang>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM DonHang", conn))
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

        public DonHang GetById(string id)
        {
            DonHang result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM DonHang WHERE MaDonHang = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
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

        public IEnumerable<ChiTietDonHang> GetDetails(string orderId)
        {
            var list = new List<ChiTietDonHang>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ChiTietDonHang WHERE MaDonHang = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", orderId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietDonHang
                            {
                                maChiTietDonHang = reader["MaCTDH"]?.ToString(),
                                maDonHang = reader["MaDonHang"]?.ToString(),
                                maLaptop = reader["MaLaptop"]?.ToString(),
                                soLuong = reader["SoLuong"]?.ToString(),
                                donGia = reader["DonGia"]?.ToString(),
                                thanhTien = reader["ThanhTien"]?.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool AddOrderWithDetails(DonHang order, List<ChiTietDonHang> details)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Create Order using Procedure
                        using (SqlCommand cmd = new SqlCommand("sp_DonHang_Create", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaDonHang", order.maDonHang);
                            cmd.Parameters.AddWithValue("@MaKH", order.maKhachHang ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaNV", order.maNhanVien ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@DiaChi", order.diaChiGiaoHang ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@PTTT", order.phuongThucThanhToan ?? (object)DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }

                        // Add Details
                        foreach (var det in details)
                        {
                            using (SqlCommand cmdDet = new SqlCommand("sp_DonHang_AddDetail", conn, transaction))
                            {
                                cmdDet.CommandType = CommandType.StoredProcedure;
                                cmdDet.Parameters.AddWithValue("@MaCTDH", det.maChiTietDonHang ?? Guid.NewGuid().ToString());
                                cmdDet.Parameters.AddWithValue("@MaDonHang", order.maDonHang);
                                cmdDet.Parameters.AddWithValue("@MaLaptop", det.maLaptop);
                                
                                int sl = 0; int.TryParse(det.soLuong, out sl);
                                cmdDet.Parameters.AddWithValue("@SoLuong", sl);
                                
                                decimal dg = 0; decimal.TryParse(det.donGia, out dg);
                                cmdDet.Parameters.AddWithValue("@DonGia", dg);
                                
                                cmdDet.ExecuteNonQuery();
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

        public bool UpdateStatus(string id, string status)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE DonHang SET TrangThai = @Status WHERE MaDonHang = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

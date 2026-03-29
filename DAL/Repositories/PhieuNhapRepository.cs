using DAL.Interfaces;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL.Repositories
{
    public class PhieuNhapRepository : IPhieuNhapRepository
    {
        private readonly string _connectionString;

        public PhieuNhapRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private PhieuNhap MapFromReader(SqlDataReader reader)
        {
            return new PhieuNhap
            {
                maPhieuNhap = reader["MaPhieuNhap"]?.ToString(),
                maNhaCungCap = reader["MaNCC"]?.ToString(),
                maNhanVien = reader["MaNV"]?.ToString(),
                ngayNhap = reader["NgayNhap"] != DBNull.Value ? Convert.ToDateTime(reader["NgayNhap"]) : DateTime.MinValue,
                tongTien = reader["TongTien"]?.ToString(),
                ghiChu = reader["GhiChu"]?.ToString()
            };
        }

        public IEnumerable<PhieuNhap> GetAll()
        {
            var list = new List<PhieuNhap>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PhieuNhap", conn))
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

        public PhieuNhap GetById(string id)
        {
            PhieuNhap result = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PhieuNhap WHERE MaPhieuNhap = @Id", conn))
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

        public IEnumerable<ChiTietPhieuNhap> GetDetails(string maPhieuNhap)
        {
            var list = new List<ChiTietPhieuNhap>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", maPhieuNhap);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new ChiTietPhieuNhap
                            {
                                maChiTietPhieuNhap = reader["MaCTPN"]?.ToString(),
                                maPhieuNhap = reader["MaPhieuNhap"]?.ToString(),
                                maLaptop = reader["MaLaptop"]?.ToString(),
                                soLuong = reader["SoLuong"]?.ToString(),
                                giaNhap = reader["GiaNhap"]?.ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool CreateImportTicket(PhieuNhap phieu, List<ChiTietPhieuNhap> details)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_PhieuNhap_Create", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaPhieuNhap", phieu.maPhieuNhap);
                            cmd.Parameters.AddWithValue("@MaNCC", phieu.maNhaCungCap ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaNV", phieu.maNhanVien ?? (object)DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }

                        foreach (var det in details)
                        {
                            using (SqlCommand cmdDet = new SqlCommand("sp_PhieuNhap_AddDetail", conn, transaction))
                            {
                                cmdDet.CommandType = CommandType.StoredProcedure;
                                cmdDet.Parameters.AddWithValue("@MaCTPN", det.maChiTietPhieuNhap ?? Guid.NewGuid().ToString());
                                cmdDet.Parameters.AddWithValue("@MaPhieuNhap", phieu.maPhieuNhap);
                                cmdDet.Parameters.AddWithValue("@MaLaptop", det.maLaptop);
                                
                                int sl = 0; int.TryParse(det.soLuong, out sl);
                                cmdDet.Parameters.AddWithValue("@SoLuong", sl);
                                
                                decimal gn = 0; decimal.TryParse(det.giaNhap, out gn);
                                cmdDet.Parameters.AddWithValue("@GiaNhap", gn);
                                
                                cmdDet.ExecuteNonQuery();
                            }
                        }

                        // update note if there's any
                        if (!string.IsNullOrEmpty(phieu.ghiChu))
                        {
                            using (SqlCommand noteCmd = new SqlCommand("UPDATE PhieuNhap SET GhiChu = @Note WHERE MaPhieuNhap = @Id", conn, transaction))
                            {
                                noteCmd.Parameters.AddWithValue("@Note", phieu.ghiChu);
                                noteCmd.Parameters.AddWithValue("@Id", phieu.maPhieuNhap);
                                noteCmd.ExecuteNonQuery();
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
                        // get details
                        var listCT = new List<dynamic>();
                        using (SqlCommand cmdGet = new SqlCommand("SELECT MaLaptop, SoLuong FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @Id", conn, transaction))
                        {
                            cmdGet.Parameters.AddWithValue("@Id", id);
                            using (var reader = cmdGet.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    listCT.Add(new {
                                        MaLaptop = reader["MaLaptop"]?.ToString(),
                                        SoLuong = Convert.ToInt32(reader["SoLuong"])
                                    });
                                }
                            }
                        }

                        // deduct back the inventory
                        foreach(var ct in listCT)
                        {
                            using (SqlCommand cmdUpLaptop = new SqlCommand("UPDATE Laptop SET SoLuong = ISNULL(SoLuong,0) - @Qty WHERE MaLaptop = @MaLaptop", conn, transaction))
                            {
                                cmdUpLaptop.Parameters.AddWithValue("@Qty", ct.SoLuong);
                                cmdUpLaptop.Parameters.AddWithValue("@MaLaptop", ct.MaLaptop);
                                cmdUpLaptop.ExecuteNonQuery();
                            }
                        }

                        // delete details
                        using (SqlCommand delCT = new SqlCommand("DELETE FROM ChiTietPhieuNhap WHERE MaPhieuNhap = @Id", conn, transaction))
                        {
                            delCT.Parameters.AddWithValue("@Id", id);
                            delCT.ExecuteNonQuery();
                        }

                        // delete master
                        using (SqlCommand delP = new SqlCommand("DELETE FROM PhieuNhap WHERE MaPhieuNhap = @Id", conn, transaction))
                        {
                            delP.Parameters.AddWithValue("@Id", id);
                            delP.ExecuteNonQuery();
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
    }
}

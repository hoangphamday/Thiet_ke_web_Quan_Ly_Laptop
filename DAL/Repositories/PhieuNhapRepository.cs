using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class PhieuNhapRepository : IPhieuNhapRepository
    {
        private readonly AppDbContext _context;

        public PhieuNhapRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PhieuNhap> GetAll()
        {
            return _context.PhieuNhaps.ToList();
        }

        public PhieuNhap GetById(string id)
        {
            return _context.PhieuNhaps.FirstOrDefault(x => x.maPhieuNhap == id);
        }

        public IEnumerable<ChiTietPhieuNhap> GetDetails(string maPhieuNhap)
        {
            return _context.ChiTietPhieuNhaps.Where(x => x.maPhieuNhap == maPhieuNhap).ToList();
        }

        public bool CreateImportTicket(PhieuNhap phieu, List<ChiTietPhieuNhap> details)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.PhieuNhaps.Add(phieu);
                    _context.ChiTietPhieuNhaps.AddRange(details);

                    // Cộng thêm số lượng laptop
                    foreach (var detail in details)
                    {
                        var laptop = _context.Laptops.FirstOrDefault(l => l.maLaptop == detail.maLaptop);
                        if (laptop != null)
                        {
                            int currentStock = 0;
                            int importQty = 0;
                            int.TryParse(laptop.soLuong, out currentStock);
                            int.TryParse(detail.soLuong, out importQty);
                            
                            laptop.soLuong = (currentStock + importQty).ToString();
                            _context.Laptops.Update(laptop);
                        }
                    }

                    _context.SaveChanges();
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

        public bool Delete(string id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var phieu = _context.PhieuNhaps.FirstOrDefault(p => p.maPhieuNhap == id);
                    if (phieu == null) return false;

                    var details = _context.ChiTietPhieuNhaps.Where(d => d.maPhieuNhap == id).ToList();
                    
                    // Trừ lại kho
                    foreach (var detail in details)
                    {
                        var laptop = _context.Laptops.FirstOrDefault(l => l.maLaptop == detail.maLaptop);
                        if (laptop != null)
                        {
                            int currentStock = 0;
                            int importQty = 0;
                            int.TryParse(laptop.soLuong, out currentStock);
                            int.TryParse(detail.soLuong, out importQty);
                            
                            laptop.soLuong = Math.Max(0, currentStock - importQty).ToString();
                            _context.Laptops.Update(laptop);
                        }
                    }

                    _context.ChiTietPhieuNhaps.RemoveRange(details);
                    _context.PhieuNhaps.Remove(phieu);
                    
                    _context.SaveChanges();
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

using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class DonHangRepository : IDonHangRepository
    {
        private readonly AppDbContext _context;

        public DonHangRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DonHang> GetAll()
        {
            return _context.DonHangs.ToList();
        }

        public DonHang GetById(string id)
        {
            return _context.DonHangs.FirstOrDefault(x => x.maDonHang == id);
        }

        public IEnumerable<ChiTietDonHang> GetDetails(string orderId)
        {
            return _context.ChiTietDonHangs.Where(x => x.maDonHang == orderId).ToList();
        }

        public bool AddOrderWithDetails(DonHang order, List<ChiTietDonHang> details)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.DonHangs.Add(order);
                    _context.ChiTietDonHangs.AddRange(details);

                    // Trừ số lượng laptop (Dùng string theo Model định nghĩa)
                    foreach (var detail in details)
                    {
                        var laptop = _context.Laptops.FirstOrDefault(l => l.maLaptop == detail.maLaptop);
                        if (laptop != null)
                        {
                            int currentStock = 0;
                            int orderQty = 0;
                            int.TryParse(laptop.soLuong, out currentStock);
                            int.TryParse(detail.soLuong, out orderQty);
                            
                            laptop.soLuong = (currentStock - orderQty).ToString();
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

        public bool UpdateStatus(string id, string status)
        {
            var order = _context.DonHangs.FirstOrDefault(x => x.maDonHang == id);
            if (order != null)
            {
                order.trangThai = status;
                _context.DonHangs.Update(order);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}

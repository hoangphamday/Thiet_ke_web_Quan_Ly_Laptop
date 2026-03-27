using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class GioHangRepository : IGioHangRepository
    {
        private readonly AppDbContext _context;

        public GioHangRepository(AppDbContext context)
        {
            _context = context;
        }

        private GioHang GetOrCreateCart(string maKH)
        {
            var cart = _context.GioHangs.FirstOrDefault(x => x.maKhachHang == maKH);
            if (cart == null)
            {
                cart = new GioHang
                {
                    maGioHang = Guid.NewGuid().ToString(),
                    maKhachHang = maKH,
                    ngayTao = DateTime.Now
                };
                _context.GioHangs.Add(cart);
                _context.SaveChanges();
            }
            return cart;
        }

        public IEnumerable<ChiTietGioHang> GetCartDetails(string maKH)
        {
            var cart = _context.GioHangs.FirstOrDefault(x => x.maKhachHang == maKH);
            if (cart == null) return new List<ChiTietGioHang>();

            return _context.ChiTietGioHangs.Where(x => x.maGioHang == cart.maGioHang).ToList();
        }

        public bool AddToCart(string maKH, string maLaptop, int soLuong)
        {
            var cart = GetOrCreateCart(maKH);
            
            var detail = _context.ChiTietGioHangs.FirstOrDefault(x => x.maGioHang == cart.maGioHang && x.maLaptop == maLaptop);
            if (detail != null)
            {
                int currentQty = 0;
                int.TryParse(detail.soLuong, out currentQty);
                detail.soLuong = (currentQty + soLuong).ToString();
                _context.ChiTietGioHangs.Update(detail);
            }
            else
            {
                detail = new ChiTietGioHang
                {
                    maChiTietGioHang = Guid.NewGuid().ToString(),
                    maGioHang = cart.maGioHang,
                    maLaptop = maLaptop,
                    soLuong = soLuong.ToString()
                };
                _context.ChiTietGioHangs.Add(detail);
            }

            return _context.SaveChanges() > 0;
        }

        public bool UpdateQuantity(string maCTGH, int soLuong)
        {
            var detail = _context.ChiTietGioHangs.FirstOrDefault(x => x.maChiTietGioHang == maCTGH);
            if (detail != null)
            {
                if (soLuong <= 0)
                {
                    _context.ChiTietGioHangs.Remove(detail);
                }
                else
                {
                    detail.soLuong = soLuong.ToString();
                    _context.ChiTietGioHangs.Update(detail);
                }
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public bool RemoveFromCart(string maCTGH)
        {
            var detail = _context.ChiTietGioHangs.FirstOrDefault(x => x.maChiTietGioHang == maCTGH);
            if (detail != null)
            {
                _context.ChiTietGioHangs.Remove(detail);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public bool ClearCart(string maKH)
        {
            var cart = _context.GioHangs.FirstOrDefault(x => x.maKhachHang == maKH);
            if (cart != null)
            {
                var details = _context.ChiTietGioHangs.Where(x => x.maGioHang == cart.maGioHang).ToList();
                _context.ChiTietGioHangs.RemoveRange(details);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}

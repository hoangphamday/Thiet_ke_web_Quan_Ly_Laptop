using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class KhachHangRepository : IKhachHangRepository
    {
        private readonly AppDbContext _context;

        public KhachHangRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<KhachHang> GetAll()
        {
            return _context.KhachHangs.ToList();
        }

        public KhachHang GetById(string id)
        {
            return _context.KhachHangs.FirstOrDefault(x => x.maKhachHang == id);
        }

        public bool Add(KhachHang khachHang)
        {
            _context.KhachHangs.Add(khachHang);
            return _context.SaveChanges() > 0;
        }

        public bool Update(KhachHang khachHang)
        {
            _context.KhachHangs.Update(khachHang);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(string id)
        {
            var entity = _context.KhachHangs.FirstOrDefault(x => x.maKhachHang == id);
            if (entity != null)
            {
                _context.KhachHangs.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}

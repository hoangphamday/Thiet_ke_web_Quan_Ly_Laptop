using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class TaiKhoanRepository : ITaiKhoanRepository
    {
        private readonly AppDbContext _context;

        public TaiKhoanRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TaiKhoan> GetAll()
        {
            return _context.TaiKhoans.ToList();
        }

        public TaiKhoan GetById(string id)
        {
            return _context.TaiKhoans.FirstOrDefault(x => x.maTaiKhoan == id);
        }

        public bool Add(TaiKhoan taiKhoan)
        {
            _context.TaiKhoans.Add(taiKhoan);
            return _context.SaveChanges() > 0;
        }

        public bool Update(TaiKhoan taiKhoan)
        {
            _context.TaiKhoans.Update(taiKhoan);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(string id)
        {
            var entity = _context.TaiKhoans.FirstOrDefault(x => x.maTaiKhoan == id);
            if (entity != null)
            {
                _context.TaiKhoans.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public TaiKhoan Login(string username, string password, string vaitro)
        {
            return _context.TaiKhoans.FirstOrDefault(x => 
                x.tenDangNhap == username && 
                x.matKhau == password && 
                x.vaiTro == vaitro &&
                x.trangThai == "Hoạt động");
        }
    }
}

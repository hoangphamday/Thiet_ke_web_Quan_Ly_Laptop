using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class NhanVienRepository : INhanVienRepository
    {
        private readonly AppDbContext _context;

        public NhanVienRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<NhanVien> GetAll()
        {
            return _context.Nhanviens.ToList();
        }

        public NhanVien GetById(string id)
        {
            return _context.Nhanviens.FirstOrDefault(x => x.maNhanVien == id);
        }

        public bool Add(NhanVien nhanVien)
        {
            _context.Nhanviens.Add(nhanVien);
            return _context.SaveChanges() > 0;
        }

        public bool Update(NhanVien nhanVien)
        {
            _context.Nhanviens.Update(nhanVien);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(string id)
        {
            var entity = _context.Nhanviens.FirstOrDefault(x => x.maNhanVien == id);
            if (entity != null)
            {
                _context.Nhanviens.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}

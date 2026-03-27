using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class NhaCungCapRepository : INhaCungCapRepository
    {
        private readonly AppDbContext _context;

        public NhaCungCapRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<NhaCungCap> GetAll()
        {
            return _context.NhaCungCaps.ToList();
        }

        public NhaCungCap GetById(string id)
        {
            return _context.NhaCungCaps.FirstOrDefault(x => x.maNhaCungCap == id);
        }

        public bool Add(NhaCungCap nhaCungCap)
        {
            _context.NhaCungCaps.Add(nhaCungCap);
            return _context.SaveChanges() > 0;
        }

        public bool Update(NhaCungCap nhaCungCap)
        {
            _context.NhaCungCaps.Update(nhaCungCap);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(string id)
        {
            var entity = _context.NhaCungCaps.FirstOrDefault(x => x.maNhaCungCap == id);
            if (entity != null)
            {
                _context.NhaCungCaps.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public IEnumerable<NhaCungCap> Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return GetAll();
            var lowerKeyword = keyword.ToLower();
            return _context.NhaCungCaps.Where(x => 
                (x.tenNhaCungCap != null && x.tenNhaCungCap.ToLower().Contains(lowerKeyword)) ||
                (x.dienThoai != null && x.dienThoai.Contains(lowerKeyword))
            ).ToList();
        }
    }
}

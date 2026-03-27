using Models;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface INhanVienRepository
    {
        IEnumerable<NhanVien> GetAll();
        NhanVien GetById(string id);
        bool Add(NhanVien nhanVien);
        bool Update(NhanVien nhanVien);
        bool Delete(string id);
    }
}

using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.Interfaces
{
    public interface INhanVienBLL
    {
        List<NhanVien> GetAll();
        NhanVien GetById(string id);
        bool Create(NhanVien nv);
        bool Update(NhanVien nv);
        bool Delete(string id);
    }
}

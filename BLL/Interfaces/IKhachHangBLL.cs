using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IKhachHangBLL
    {
        List<KhachHang> GetAll();
        KhachHang GetById(string id);
        bool Create(KhachHang kh);
        bool Update(KhachHang kh);
    }
}

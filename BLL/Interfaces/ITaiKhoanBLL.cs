using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DAL;

namespace BLL.Interfaces
{
    internal interface ITaiKhoanBLL
    {
        List<TaiKhoan> GetAll();
        TaiKhoan GetById(string id);
        bool Create(TaiKhoan model);
        bool Update(TaiKhoan model);
        bool Delete(string id);
        TaiKhoan Login(string username, string password, string vaitro);
    }
}

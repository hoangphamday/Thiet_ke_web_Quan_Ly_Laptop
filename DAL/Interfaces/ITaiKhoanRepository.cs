using Models;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface ITaiKhoanRepository
    {
        IEnumerable<TaiKhoan> GetAll();
        TaiKhoan GetById(string id);
        bool Add(TaiKhoan taiKhoan);
        bool Update(TaiKhoan taiKhoan);
        bool Delete(string id);
        TaiKhoan Login(string username, string password, string vaitro);
    }
}

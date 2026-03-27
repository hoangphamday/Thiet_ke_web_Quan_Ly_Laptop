using Models;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IKhachHangRepository
    {
        IEnumerable<KhachHang> GetAll();
        KhachHang GetById(string id);
        bool Add(KhachHang khachHang);
        bool Update(KhachHang khachHang);
        bool Delete(string id);
    }
}

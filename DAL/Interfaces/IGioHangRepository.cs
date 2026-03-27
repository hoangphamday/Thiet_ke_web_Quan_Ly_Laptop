using Models;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IGioHangRepository
    {
        IEnumerable<ChiTietGioHang> GetCartDetails(string maKH);
        bool AddToCart(string maKH, string maLaptop, int soLuong);
        bool UpdateQuantity(string maCTGH, int soLuong);
        bool RemoveFromCart(string maCTGH);
        bool ClearCart(string maKH);
    }
}

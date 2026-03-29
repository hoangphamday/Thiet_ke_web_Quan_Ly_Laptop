using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IgioHangBLL
    {
        // L?y gi? hàng kèm danh sách s?n ph?m bên trong
        List<ChiTietGioHang> GetCartDetails(string maKH);

        // Thêm/S?a/Xóa s?n ph?m trong gi?
        bool AddToCart(string maKH, string maLaptop, int soLuong);
        bool UpdateQuantity(string maCTGH, int soLuong);
        bool RemoveFromCart(string maCTGH);
        bool ClearCart(string maKH);
    }
}

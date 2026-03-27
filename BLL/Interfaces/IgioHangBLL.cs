using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    internal interface IgioHangBLL
    {
        // Lấy giỏ hàng kèm danh sách sản phẩm bên trong
        List<ChiTietGioHang> GetCartDetails(string maKH);

        // Thêm/Sửa/Xóa sản phẩm trong giỏ
        bool AddToCart(string maKH, string maLaptop, int soLuong);
        bool UpdateQuantity(string maCTGH, int soLuong);
        bool RemoveFromCart(string maCTGH);
        bool ClearCart(string maKH);
    }
}

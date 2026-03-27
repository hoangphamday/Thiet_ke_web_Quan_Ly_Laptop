using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    internal interface IDonHangBLL
    {
        List<DonHang> GetAll();
        DonHang GetById(string id);
        // Lấy chi tiết của một đơn hàng cụ thể
        List<ChiTietDonHang> GetDetails(string maDonHang);

        // Nghiệp vụ quan trọng nhất: Lưu đơn + Lưu chi tiết + Trừ số lượng tồn kho Laptop
        bool PlaceOrder(DonHang order, List<ChiTietDonHang> details);

        bool UpdateStatus(string maDonHang, string trangThai);
    }
}

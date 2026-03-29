using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDonHangBLL
    {
        List<DonHang> GetAll();
        DonHang GetById(string id);
        // L?y chi ti?t c?a m?t don hàng c? th?
        List<ChiTietDonHang> GetDetails(string maDonHang);

        // Nghi?p v? quan tr?ng nh?t: Luu don + Luu chi ti?t + Tr? s? lu?ng t?n kho Laptop
        bool PlaceOrder(DonHang order, List<ChiTietDonHang> details);

        bool UpdateStatus(string maDonHang, string trangThai);
    }
}

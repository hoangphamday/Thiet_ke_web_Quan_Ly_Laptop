using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPhieuNhapBLL
    {
        List<PhieuNhap> GetAll();
        PhieuNhap GetById(string id);
        List<ChiTietPhieuNhap> GetDetails(string maPhieuNhap);

        // Nghi?p v? quan tr?ng: 
        // 1. Luu PhieuNhap 
        // 2. Luu danh sách ChiTietPhieuNhap 
        // 3. G?i LaptopDAL d? C?NG THÊM s? lu?ng vào kho
        bool CreateImportTicket(PhieuNhap phieu, List<ChiTietPhieuNhap> details);

        bool Delete(string id); // Thu?ng là h?y phi?u và tr? l?i kho
    }
}

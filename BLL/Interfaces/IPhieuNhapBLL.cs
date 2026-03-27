using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    internal interface IPhieuNhapBLL
    {
        List<PhieuNhap> GetAll();
        PhieuNhap GetById(string id);
        List<ChiTietPhieuNhap> GetDetails(string maPhieuNhap);

        // Nghiệp vụ quan trọng: 
        // 1. Lưu PhieuNhap 
        // 2. Lưu danh sách ChiTietPhieuNhap 
        // 3. Gọi LaptopDAL để CỘNG THÊM số lượng vào kho
        bool CreateImportTicket(PhieuNhap phieu, List<ChiTietPhieuNhap> details);

        bool Delete(string id); // Thường là hủy phiếu và trừ lại kho
    }
}

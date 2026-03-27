using Models;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IPhieuNhapRepository
    {
        IEnumerable<PhieuNhap> GetAll();
        PhieuNhap GetById(string id);
        IEnumerable<ChiTietPhieuNhap> GetDetails(string maPhieuNhap);
        bool CreateImportTicket(PhieuNhap phieu, List<ChiTietPhieuNhap> details);
        bool Delete(string id);
    }
}

using Models;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IDonHangRepository
    {
        IEnumerable<DonHang> GetAll();
        DonHang GetById(string id);
        IEnumerable<ChiTietDonHang> GetDetails(string orderId);
        bool AddOrderWithDetails(DonHang order, List<ChiTietDonHang> details);
        bool UpdateStatus(string id, string status);
    }
}

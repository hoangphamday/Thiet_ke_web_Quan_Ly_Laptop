using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class DonHangBLL : IDonHangBLL
    {
        private readonly IDonHangRepository _repository;

        public DonHangBLL(IDonHangRepository repository)
        {
            _repository = repository;
        }

        public List<DonHang> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public DonHang GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<ChiTietDonHang> GetDetails(string maDonHang)
        {
            return _repository.GetDetails(maDonHang).ToList();
        }

        public bool PlaceOrder(DonHang order, List<ChiTietDonHang> details)
        {
            return _repository.AddOrderWithDetails(order, details);
        }

        public bool UpdateStatus(string maDonHang, string trangThai)
        {
            return _repository.UpdateStatus(maDonHang, trangThai);
        }
    }
}

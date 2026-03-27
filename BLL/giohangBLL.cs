using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class giohangBLL : IgioHangBLL
    {
        private readonly IGioHangRepository _repository;

        public giohangBLL(IGioHangRepository repository)
        {
            _repository = repository;
        }

        public List<ChiTietGioHang> GetCartDetails(string maKH)
        {
            return _repository.GetCartDetails(maKH).ToList();
        }

        public bool AddToCart(string maKH, string maLaptop, int soLuong)
        {
            return _repository.AddToCart(maKH, maLaptop, soLuong);
        }

        public bool UpdateQuantity(string maCTGH, int soLuong)
        {
            return _repository.UpdateQuantity(maCTGH, soLuong);
        }

        public bool RemoveFromCart(string maCTGH)
        {
            return _repository.RemoveFromCart(maCTGH);
        }

        public bool ClearCart(string maKH)
        {
            return _repository.ClearCart(maKH);
        }
    }
}

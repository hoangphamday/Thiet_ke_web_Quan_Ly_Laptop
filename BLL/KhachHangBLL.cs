using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class KhachHangBLL : IKhachHangBLL
    {
        private readonly IKhachHangRepository _repository;

        public KhachHangBLL(IKhachHangRepository repository)
        {
            _repository = repository;
        }

        public List<KhachHang> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public KhachHang GetById(string id)
        {
            return _repository.GetById(id);
        }

        public bool Create(KhachHang kh)
        {
            return _repository.Add(kh);
        }

        public bool Update(KhachHang kh)
        {
            return _repository.Update(kh);
        }
    }
}

using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class TaiKhoanBLL : ITaiKhoanBLL
    {
        private readonly ITaiKhoanRepository _repository;

        public TaiKhoanBLL(ITaiKhoanRepository repository)
        {
            _repository = repository;
        }

        public List<TaiKhoan> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public TaiKhoan GetById(string id)
        {
            return _repository.GetById(id);
        }

        public bool Create(TaiKhoan model)
        {
            return _repository.Add(model);
        }

        public bool Update(TaiKhoan model)
        {
            return _repository.Update(model);
        }

        public bool Delete(string id)
        {
            return _repository.Delete(id);
        }

        public TaiKhoan Login(string username, string password, string vaitro)
        {
            return _repository.Login(username, password, vaitro);
        }
    }
}

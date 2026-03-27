using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class NhanVienBLL : INhanVienBLL
    {
        private readonly INhanVienRepository _repository;

        public NhanVienBLL(INhanVienRepository repository)
        {
            _repository = repository;
        }

        public List<NhanVien> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public NhanVien GetById(string id)
        {
            return _repository.GetById(id);
        }

        public bool Create(NhanVien nv)
        {
            return _repository.Add(nv);
        }

        public bool Update(NhanVien nv)
        {
            return _repository.Update(nv);
        }

        public bool Delete(string id)
        {
            return _repository.Delete(id);
        }
    }
}

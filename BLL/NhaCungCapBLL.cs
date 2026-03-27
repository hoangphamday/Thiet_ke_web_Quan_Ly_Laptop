using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class NhaCungCapBLL : INhaCungCapBLL
    {
        private readonly INhaCungCapRepository _repository;

        public NhaCungCapBLL(INhaCungCapRepository repository)
        {
            _repository = repository;
        }

        public List<NhaCungCap> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public NhaCungCap GetById(string id)
        {
            return _repository.GetById(id);
        }

        public bool Create(NhaCungCap ncc)
        {
            return _repository.Add(ncc);
        }

        public bool Update(NhaCungCap ncc)
        {
            return _repository.Update(ncc);
        }

        public bool Delete(string id)
        {
            return _repository.Delete(id);
        }

        public List<NhaCungCap> Search(string keyword)
        {
            return _repository.Search(keyword).ToList();
        }
    }
}

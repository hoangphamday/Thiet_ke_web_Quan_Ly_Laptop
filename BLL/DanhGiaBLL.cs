using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL
{
    public class DanhGiaBLL : IDanhGiaBLL
    {
        private readonly IDanhGiaRepository _repository;

        public DanhGiaBLL(IDanhGiaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DanhGia>> GetByLaptop(string maLaptop)
        {
            return await _repository.GetByLaptop(maLaptop);
        }

        public async Task Add(DanhGia model)
        {
            await _repository.Add(model);
        }

        public async Task Delete(string maDanhGia)
        {
            await _repository.Delete(maDanhGia);
        }
    }
}

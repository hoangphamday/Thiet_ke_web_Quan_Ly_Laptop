using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class PhieuNhapBLL : IPhieuNhapBLL
    {
        private readonly IPhieuNhapRepository _repository;

        public PhieuNhapBLL(IPhieuNhapRepository repository)
        {
            _repository = repository;
        }

        public List<PhieuNhap> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public PhieuNhap GetById(string id)
        {
            return _repository.GetById(id);
        }

        public List<ChiTietPhieuNhap> GetDetails(string maPhieuNhap)
        {
            return _repository.GetDetails(maPhieuNhap).ToList();
        }

        public bool CreateImportTicket(PhieuNhap phieu, List<ChiTietPhieuNhap> details)
        {
            return _repository.CreateImportTicket(phieu, details);
        }

        public bool Delete(string id)
        {
            return _repository.Delete(id);
        }
    }
}

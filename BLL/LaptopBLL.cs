using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class LaptopBLL : ILaptopBLL
    {
        private readonly ILaptopRepository _repository;

        public LaptopBLL(ILaptopRepository repository)
        {
            _repository = repository;
        }

        public List<Laptop> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Laptop GetById(string id)
        {
            return _repository.GetById(id);
        }

        public bool Create(Laptop model, List<HinhAnhLaptop> images)
        {
            return _repository.Add(model, images);
        }

        public bool Update(Laptop model, List<HinhAnhLaptop> images)
        {
            return _repository.Update(model, images);
        }

        public bool Delete(string id)
        {
            return _repository.Delete(id);
        }

        public List<HangLaptop> GetAllBrands()
        {
            return _repository.GetAllBrands().ToList();
        }

        public List<DanhGia> GetReviews(string maLaptop)
        {
            return _repository.GetReviews(maLaptop).ToList();
        }

        public bool AddReview(DanhGia review)
        {
            return _repository.AddReview(review);
        }
    }
}

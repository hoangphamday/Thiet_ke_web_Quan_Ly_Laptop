using Models;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public interface ILaptopRepository
    {
        IEnumerable<Laptop> GetAll();
        Laptop GetById(string id);
        bool Add(Laptop laptop, List<HinhAnhLaptop> images);
        bool Update(Laptop laptop, List<HinhAnhLaptop> images);
        bool Delete(string id);
        
        IEnumerable<HangLaptop> GetAllBrands();
        
        IEnumerable<DanhGia> GetReviews(string maLaptop);
        bool AddReview(DanhGia review);
    }
}

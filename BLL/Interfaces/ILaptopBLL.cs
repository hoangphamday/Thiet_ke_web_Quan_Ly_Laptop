using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ILaptopBLL
    {
        // Laptop & Hình ?nh
        List<Laptop> GetAll();
        Laptop GetById(string id);
        bool Create(Laptop model, List<HinhAnhLaptop> images);
        bool Update(Laptop model, List<HinhAnhLaptop> images);
        bool Delete(string id);

        // Hãng Laptop
        List<HangLaptop> GetAllBrands();

        // Ðánh giá
        List<DanhGia> GetReviews(string maLaptop);
        bool AddReview(DanhGia review);
    }
}

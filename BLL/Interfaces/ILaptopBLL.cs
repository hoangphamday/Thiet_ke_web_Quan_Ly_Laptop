using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    internal interface ILaptopBLL
    {
        // Laptop & Hình ảnh
        List<Laptop> GetAll();
        Laptop GetById(string id);
        bool Create(Laptop model, List<HinhAnhLaptop> images);
        bool Update(Laptop model, List<HinhAnhLaptop> images);
        bool Delete(string id);

        // Hãng Laptop
        List<HangLaptop> GetAllBrands();

        // Đánh giá
        List<DanhGia> GetReviews(string maLaptop);
        bool AddReview(DanhGia review);
    }
}

using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDanhGiaRepository
    {
        Task<List<DanhGia>> GetByLaptop(string maLaptop);
        Task Add(DanhGia model);
        Task Delete(string maDanhGia);
    }
}

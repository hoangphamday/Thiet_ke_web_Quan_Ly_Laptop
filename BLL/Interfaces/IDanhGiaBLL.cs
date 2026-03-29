using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDanhGiaBLL
    {
        Task<List<DanhGia>> GetByLaptop(string maLaptop);
        Task Add(DanhGia model);
        Task Delete(string maDanhGia);
    }
}

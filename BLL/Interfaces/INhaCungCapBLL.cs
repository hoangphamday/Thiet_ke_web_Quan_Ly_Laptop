using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    internal interface INhaCungCapBLL
    {
        List<NhaCungCap> GetAll();
        NhaCungCap GetById(string id);
        bool Create(NhaCungCap ncc);
        bool Update(NhaCungCap ncc);
        bool Delete(string id);

        // Tìm nhanh NCC theo tên hoặc số điện thoại
        List<NhaCungCap> Search(string keyword);
    }
}

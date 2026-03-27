using Models;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface INhaCungCapRepository
    {
        IEnumerable<NhaCungCap> GetAll();
        NhaCungCap GetById(string id);
        bool Add(NhaCungCap nhaCungCap);
        bool Update(NhaCungCap nhaCungCap);
        bool Delete(string id);
        IEnumerable<NhaCungCap> Search(string keyword);
    }
}

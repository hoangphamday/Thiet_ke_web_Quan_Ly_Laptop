using Models.DTOs;
using System.Collections.Generic;

namespace BLL.Services
{
    // Layer Service giao tiếp bằng DTOs, không trả Entity rò rỉ ra API
    public interface ILaptopService
    {
        IEnumerable<LaptopDTO> GetAllLaptops();
        LaptopDTO GetLaptopById(string id);
    }
}

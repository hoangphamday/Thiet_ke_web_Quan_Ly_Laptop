using BLL.Services;
using DAL.Repositories;
using Models.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class LaptopService : ILaptopService
    {
        private readonly ILaptopRepository _repository;

        // Dependency Injection từ Data Access Layer
        public LaptopService(ILaptopRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<LaptopDTO> GetAllLaptops()
        {
            // Trong thực tế bạn có thể mapping rỗng (hoặc dùng AutoMapper)
            // Lấy từ Entity(Repository) -> Chuyển sang DTOs -> Trả cho Controller
            return _repository.GetAll().Select(l => new LaptopDTO 
            { 
               // Map tay các properties tại đây
            }).ToList();
        }

        public LaptopDTO GetLaptopById(string id)
        {
            var laptop = _repository.GetById(id);
            if (laptop == null) return null;

            return new LaptopDTO
            {
               // Map tay các properties tại đây
            };
        }
    }
}

using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;

namespace API_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class danhGiaController : ControllerBase
    {
        private readonly IDanhGiaBLL _danhGiaBLL;

        public danhGiaController(IDanhGiaBLL danhGiaBLL)
        {
            _danhGiaBLL = danhGiaBLL;
        }

        /// <summary>
        /// Lấy tất cả bài đánh giá của một Laptop cụ thể
        /// </summary>
        [HttpGet("get-by-laptop/{maLaptop}")]
        public async System.Threading.Tasks.Task<IActionResult> GetByLaptop(string maLaptop)
        {
            try
            {
                var list = await _danhGiaBLL.GetByLaptop(maLaptop);
                return Ok(new { message = "Thành công", data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Thêm hoặc gửi một bài đánh giá mới cho sản phẩm
        /// </summary>
        [HttpPost("create")]
        public async System.Threading.Tasks.Task<IActionResult> Create([FromBody] DanhGia model)
        {
            try
            {
                await _danhGiaBLL.Add(model);
                return Ok(new { message = "Cảm ơn bạn đã đánh giá sản phẩm!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi khi gửi đánh giá: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa bỏ một bài đánh giá (dành cho quản trị viên)
        /// </summary>
        [HttpDelete("delete/{id}")]
        public async System.Threading.Tasks.Task<IActionResult> Delete(string id)
        {
            try
            {
                await _danhGiaBLL.Delete(id);
                return Ok(new { message = "Xóa đánh giá thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Không thể xóa bài đánh giá này: " + ex.Message });
            }
        }
    }
}

using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;

namespace API_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class taiKhoanController : ControllerBase
    {
        private readonly ITaiKhoanBLL _taiKhoanBLL;

        public taiKhoanController(ITaiKhoanBLL taiKhoanBLL)
        {
            _taiKhoanBLL = taiKhoanBLL;
        }

        /// <summary>
        /// Lấy danh sách tất cả tài khoản trong hệ thống
        /// </summary>
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var list = _taiKhoanBLL.GetAll();
                return Ok(new { message = "Thành công", data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin tài khoản theo mã (ID)
        /// </summary>
        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var tk = _taiKhoanBLL.GetById(id);
                if (tk == null) return NotFound(new { message = "Không tìm thấy tài khoản!" });
                
                return Ok(new { message = "Thành công", data = tk });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Đăng ký / Thêm tài khoản mới
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create([FromBody] TaiKhoan model)
        {
            try
            {
                bool result = _taiKhoanBLL.Create(model);
                if (result) return Ok(new { message = "Tạo tài khoản thành công!" });
                return BadRequest(new { message = "Tạo tài khoản thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin tài khoản
        /// </summary>
        [HttpPut("update")]
        public IActionResult Update([FromBody] TaiKhoan model)
        {
            try
            {
                bool result = _taiKhoanBLL.Update(model);
                if (result) return Ok(new { message = "Cập nhật thành công!" });
                return BadRequest(new { message = "Cập nhật thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                bool result = _taiKhoanBLL.Delete(id);
                if (result) return Ok(new { message = "Xóa tài khoản thành công!" });
                return BadRequest(new { message = "Xóa tài khoản thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}

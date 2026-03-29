using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;

namespace API_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class nhanVienController : ControllerBase
    {
        private readonly INhanVienBLL _nhanVienBLL;

        public nhanVienController(INhanVienBLL nhanVienBLL)
        {
            _nhanVienBLL = nhanVienBLL;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách nhân viên
        /// </summary>
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var list = _nhanVienBLL.GetAll();
                return Ok(new { message = "Thành công", data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi lấy dữ liệu: " + ex.Message });
            }
        }

        /// <summary>
        /// Tìm kiếm nhân viên bằng mã
        /// </summary>
        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var nv = _nhanVienBLL.GetById(id);
                if (nv == null) return NotFound(new { message = "Nhân viên không tồn tại!" });

                return Ok(new { message = "Thành công", data = nv });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Thêm mới một nhân viên vào hệ thống
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create([FromBody] NhanVien model)
        {
            try
            {
                bool result = _nhanVienBLL.Create(model);
                if (result) return Ok(new { message = "Thêm nhân viên mới thành công!" });
                return BadRequest(new { message = "Thêm nhân viên thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Chỉnh sửa thông tin nhân viên
        /// </summary>
        [HttpPut("update")]
        public IActionResult Update([FromBody] NhanVien model)
        {
            try
            {
                bool result = _nhanVienBLL.Update(model);
                if (result) return Ok(new { message = "Cập nhật nhân viên thành công!" });
                return BadRequest(new { message = "Cập nhật thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa một nhân viên theo mã
        /// </summary>
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                bool result = _nhanVienBLL.Delete(id);
                if (result) return Ok(new { message = "Xóa nhân viên thành công!" });
                return BadRequest(new { message = "Lỗi khi xóa nhân viên!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}

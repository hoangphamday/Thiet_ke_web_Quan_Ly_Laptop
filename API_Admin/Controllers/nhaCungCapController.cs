using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;

namespace API_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class nhaCungCapController : ControllerBase
    {
        private readonly INhaCungCapBLL _nhaCungCapBLL;

        public nhaCungCapController(INhaCungCapBLL nhaCungCapBLL)
        {
            _nhaCungCapBLL = nhaCungCapBLL;
        }

        /// <summary>
        /// Lấy toàn bộ nhà cung cấp
        /// </summary>
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var list = _nhaCungCapBLL.GetAll();
                return Ok(new { message = "Thành công", data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Tìm nhà cung cấp theo mã NCC
        /// </summary>
        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var item = _nhaCungCapBLL.GetById(id);
                if (item == null) return NotFound(new { message = "Không tìm thấy nhà cung cấp!" });

                return Ok(new { message = "Thành công", data = item });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Thêm mới nhà cung cấp
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create([FromBody] NhaCungCap model)
        {
            try
            {
                bool result = _nhaCungCapBLL.Create(model);
                if (result) return Ok(new { message = "Thêm nhà cung cấp thành công!" });
                return BadRequest(new { message = "Thêm thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        [HttpPut("update")]
        public IActionResult Update([FromBody] NhaCungCap model)
        {
            try
            {
                bool result = _nhaCungCapBLL.Update(model);
                if (result) return Ok(new { message = "Cập nhật nhà cung cấp thành công!" });
                return BadRequest(new { message = "Cập nhật thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa nhà cung cấp
        /// </summary>
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                bool result = _nhaCungCapBLL.Delete(id);
                if (result) return Ok(new { message = "Xóa nhà cung cấp an toàn!" });
                return BadRequest(new { message = "Lỗi khi xóa nhà cung cấp!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}

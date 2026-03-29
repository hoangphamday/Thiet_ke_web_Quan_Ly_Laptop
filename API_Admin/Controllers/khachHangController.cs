using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;

namespace API_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class khachHangController : ControllerBase
    {
        private readonly IKhachHangBLL _khachHangBLL;

        public khachHangController(IKhachHangBLL khachHangBLL)
        {
            _khachHangBLL = khachHangBLL;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách khách hàng
        /// </summary>
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var list = _khachHangBLL.GetAll();
                return Ok(new { message = "Thành công", data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Lấy dữ liệu khách hàng theo mã
        /// </summary>
        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var kh = _khachHangBLL.GetById(id);
                if (kh == null) return NotFound(new { message = "Không tìm thấy khách hàng!" });

                return Ok(new { message = "Thành công", data = kh });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Tạo mới một khách hàng
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create([FromBody] KhachHang model)
        {
            try
            {
                bool result = _khachHangBLL.Create(model);
                if (result) return Ok(new { message = "Thêm khách hàng thành công!" });
                return BadRequest(new { message = "Thêm khách hàng thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Sửa thông tin khách hàng
        /// </summary>
        [HttpPut("update")]
        public IActionResult Update([FromBody] KhachHang model)
        {
            try
            {
                bool result = _khachHangBLL.Update(model);
                if (result) return Ok(new { message = "Sửa thông tin khách hàng thành công!" });
                return BadRequest(new { message = "Sửa thông tin thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }


    }
}

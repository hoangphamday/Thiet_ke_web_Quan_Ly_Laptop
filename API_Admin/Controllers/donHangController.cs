using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;

namespace API_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class donHangController : ControllerBase
    {
        private readonly IDonHangBLL _donHangBLL;

        public donHangController(IDonHangBLL donHangBLL)
        {
            _donHangBLL = donHangBLL;
        }

        public class OrderRequestModel
        {
            public DonHang order { get; set; }
            public List<ChiTietDonHang> details { get; set; }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách đơn hàng đã đặt
        /// </summary>
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var list = _donHangBLL.GetAll();
                return Ok(new { message = "Thành công", data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin đơn hàng và chi tiết các sản phẩm trong đó
        /// </summary>
        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var order = _donHangBLL.GetById(id);
                if (order == null) return NotFound(new { message = "Không tìm thấy đơn hàng!" });

                var details = _donHangBLL.GetDetails(id);
                return Ok(new { message = "Thành công", data = new { order, details } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi xử lý hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Tạo một đơn hàng mới (Kèm sản phẩm chi tiết + Trừ tồn kho sản phẩm)
        /// </summary>
        [HttpPost("create")]
        public IActionResult PlaceOrder([FromBody] OrderRequestModel req)
        {
            try
            {
                bool result = _donHangBLL.PlaceOrder(req.order, req.details);
                if (result) return Ok(new { message = "Đơn hàng đã được tạo và trừ kho thành công!" });
                return BadRequest(new { message = "Tạo đơn hàng thất bại, vui lòng kiểm tra lại tồn kho!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật trạng thái của đơn hàng (Đã xử lý, Đang giao, Đã giao, Hủy)
        /// </summary>
        [HttpPut("update-status")]
        public IActionResult UpdateStatus([FromQuery] string maDonHang, [FromQuery] string trangThai)
        {
            try
            {
                bool result = _donHangBLL.UpdateStatus(maDonHang, trangThai);
                if (result) return Ok(new { message = $"Đã chuyển trạng thái đơn hàng thành: {trangThai}" });
                return BadRequest(new { message = "Cập nhật trạng thái thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi đổi trạng thái: " + ex.Message });
            }
        }
    }
}

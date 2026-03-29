using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;

namespace API_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class phieuNhapController : ControllerBase
    {
        private readonly IPhieuNhapBLL _phieuNhapBLL;

        public phieuNhapController(IPhieuNhapBLL phieuNhapBLL)
        {
            _phieuNhapBLL = phieuNhapBLL;
        }

        public class PhieuNhapRequest
        {
            public PhieuNhap phieuNhap { get; set; }
            public List<ChiTietPhieuNhap> chiTiet { get; set; }
        }

        /// <summary>
        /// Lấy danh sách lịch sử phiếu nhập hàng
        /// </summary>
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var list = _phieuNhapBLL.GetAll();
                return Ok(new { message = "Thành công", data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi xuất danh sách: " + ex.Message });
            }
        }

        /// <summary>
        /// Xem thông tin 1 phiếu nhập theo ID
        /// </summary>
        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var pt = _phieuNhapBLL.GetById(id);
                if (pt == null) return NotFound(new { message = "Không tìm thấy phiếu nhập này!" });

                var details = _phieuNhapBLL.GetDetails(id);
                return Ok(new { message = "Thành công", data = new { phieu = pt, chiTiet = details } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Tạo phiếu nhập hàng và cộng tồn kho cho laptop
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create([FromBody] PhieuNhapRequest req)
        {
            try
            {
                bool result = _phieuNhapBLL.CreateImportTicket(req.phieuNhap, req.chiTiet);
                if (result) return Ok(new { message = "Nhập hàng thành công và kho đã được cập nhật!" });
                return BadRequest(new { message = "Lỗi trong quá trình nhập hàng!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa, Hủy Phiếu Nhập và trừ lại số lượng tồn kho (Khôi phục kho cũ)
        /// </summary>
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                bool result = _phieuNhapBLL.Delete(id);
                if (result) return Ok(new { message = "Hủy phiếu nhập thành công, kho đã được khôi phục!" });
                return BadRequest(new { message = "Hủy phiếu thất bại!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}

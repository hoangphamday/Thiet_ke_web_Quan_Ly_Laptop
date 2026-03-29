using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;

namespace API_Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopController : ControllerBase
    {
        private readonly ILaptopBLL _laptopBLL;

        public LaptopController(ILaptopBLL laptopBLL)
        {
            _laptopBLL = laptopBLL;
        }

        public class LaptopRequestModel
        {
            public Laptop laptop { get; set; }
            public List<HinhAnhLaptop> images { get; set; }
        }

        /// <summary>
        /// Lấy tất cả laptop trong cửa hàng
        /// </summary>
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var list = _laptopBLL.GetAll();
                return Ok(new { message = "Thành công", data = list });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin 1 laptop theo mã
        /// </summary>
        [HttpGet("get-by-id/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var laptop = _laptopBLL.GetById(id);
                if (laptop == null) return NotFound(new { message = "Không tìm thấy laptop!" });

                return Ok(new { message = "Thành công", data = laptop });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Thêm mới một dòng laptop và danh sách hình ảnh của nó
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create([FromBody] LaptopRequestModel req)
        {
            try
            {
                bool result = _laptopBLL.Create(req.laptop, req.images);
                if (result) return Ok(new { message = "Thêm mã laptop mới thành công!" });
                return BadRequest(new { message = "Lỗi khi thêm laptop!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin laptop (như Cấu Hình, Số Lượng, Giá)
        /// </summary>
        [HttpPut("update")]
        public IActionResult Update([FromBody] LaptopRequestModel req)
        {
            try
            {
                bool result = _laptopBLL.Update(req.laptop, req.images);
                if (result) return Ok(new { message = "Cập nhật laptop thành công!" });
                return BadRequest(new { message = "Lỗi khi cập nhật laptop!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Xóa vĩnh viễn sản phẩm laptop khỏi kho
        /// </summary>
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                bool result = _laptopBLL.Delete(id);
                if (result) return Ok(new { message = "Xóa sản phẩm thành công!" });
                return BadRequest(new { message = "Không thể xóa sản phẩm!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        /// <summary>
        /// Lấy toàn bộ Hãng Laptop
        /// </summary>
        [HttpGet("get-brands")]
        public IActionResult GetBrands()
        {
            try
            {
                var brands = _laptopBLL.GetAllBrands();
                return Ok(new { message = "Thành công", data = brands });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}

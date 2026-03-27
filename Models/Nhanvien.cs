using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NhanVien
    {
        public string maNhanVien { get; set; }
        public string tenNhanVien { get; set; }
        public DateTime NgaySinh { get; set; }
        public string diaChi { get; set; }
        public string dienThoai { get; set; }
        public string chucVu { get; set; }
        public DateTime ngayVaoLam { get; set; }
        public string maTaiKhoan { get; set; }
    }
}

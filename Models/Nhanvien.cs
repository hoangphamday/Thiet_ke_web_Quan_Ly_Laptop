using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NhanVien
    {
        public string maNhanVien {get; set;}
        public string tenNhanVien {get; set;}
        public string chucVu {get; set;}
        public string soDienThoai {get; set;}
        public string email {get; set;}
        public DateTime ngaySinh {get; set;} 
        public DateTime ngayVaoLam {get; set;}
        public string maTaiKhoan {get; set;}
    }
}

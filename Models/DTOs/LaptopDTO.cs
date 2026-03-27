namespace Models.DTOs
{
    public class LaptopDTO
    {
        // Ví dụ một số cột thường có, bạn có thể bổ sung thêm dựa theo DB thực tế
        public int Id { get; set; }
        public string TenLaptop { get; set; }
        public decimal Gia { get; set; }
    }
}

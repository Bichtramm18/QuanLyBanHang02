namespace QuanLyBanHang02.Models
{
    public class CartModel
    {
        QLNHoaContext da = new QLNHoaContext();
        public int MaNh { get; set; }
        public string? TenNh { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public decimal Total { get { return DonGia * SoLuong; } }
    }
}

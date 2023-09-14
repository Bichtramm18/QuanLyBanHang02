namespace QuanLyBanHang02.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string? StatusMessage { get; set; }
        public List<CartModel>? ListProducts { get; set; }
    }
}

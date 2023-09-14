namespace QuanLyBanHang02.Models
{
    public class SearchProductReq
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public string? Keyword { get; set; }
    }
}

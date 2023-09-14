using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang02.Models;

namespace QuanLyBanHang02.Controllers
{
    public class PerfumeController : Controller
    {
        QLNHoaContext da = new QLNHoaContext();
        [HttpGet("All perfumes")]
        public IActionResult GetAll()
        {
            var ds = da.Perfumes.Select(s => new { s.MaNh, s.TenNh, s.TheTich, s.Huong, s.DonGia }).ToList();
            return Ok(ds);
        }

        [HttpGet("Get perfume by Name")]
        public IActionResult GetPerfumeByName(string TenNH)
        {
            var ds = da.Perfumes.Where(s => s.TenNh == TenNH).Select(s => new { s.MaNh, s.TenNh, s.TheTich, s.Huong, s.DonGia }).ToList();
            return Ok(ds);
        }


        // GET api/<PerfumesController>/5
        [HttpGet("Get Perfume By Supplier")]
        public IActionResult GetPerfumeBySupplier(string TenNCC)
        {
            var ds = da.Suppliers.Where(s => s.TenNcc == TenNCC).Join(da.Perfumes, s => s.MaNcc, p => p.MaNcc, (s, p) => new { p.MaNh, p.TenNh, s.TenNcc, p.TheTich, p.Huong, p.DonGia });
            return Ok(ds);
        }


        [HttpGet("Get Top Best Selling Perfume")]
        public IActionResult GetBestSellingPerfume()
        {
            var ds = da.OrderDetails.OrderByDescending(o => o.SoLuong).ThenBy(o => o.MaNh).Join(da.Perfumes, o => o.MaNh, p => p.MaNh, (o, p) => new { o.MaNh, p.TenNh, o.DonGia }).Take(10);
            return Ok(ds);
        }

        //Tim kiem SP
        [HttpPost("Search perfume by Name")]
        public IActionResult SearchPerfume([FromBody] SearchProductReq searchProductReq)
        {
            var product = SearchPerfumes(searchProductReq);
            return Ok(product);
        }

        private object SearchPerfumes(SearchProductReq searchProductReq)
        {
            var products = da.Perfumes.Where(x => x.TenNh.Contains(searchProductReq.Keyword));
            var offset = (searchProductReq.Page - 1) * searchProductReq.Size;
            var total = products.Count();
            int totalPage = (total % searchProductReq.Size) == 0 ? (int)(total / searchProductReq.Size) : (int)(1 + (total / searchProductReq.Size));
            var data = products.OrderBy(x => x.MaNh).Skip(offset).Take(searchProductReq.Size).ToList();
            var res = new
            {
                Data = data,
                TotalRecord = totalPage,
                TotalPages = totalPage,
                Page = searchProductReq.Page,
                Size = searchProductReq.Size
            };
            return res;
        }

    }
}

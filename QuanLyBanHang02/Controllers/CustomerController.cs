using Microsoft.AspNetCore.Mvc;
using QLNHoa.Models;
using QuanLyBanHang02.Models;

namespace QuanLyBanHang02.Controllers
{
    public class CustomerController : Controller
    {
        QLNHoaContext da = new QLNHoaContext();
        [HttpGet("get all customers")]
        public IActionResult GetAllCustomers()
        {
            var ds = da.Customers.ToList();
            return Ok(ds);
        }
        [HttpGet("get customer by MaKH")]
        public IActionResult GetCustomerByMaKH(int ma)
        {
            var ds = da.Customers.FirstOrDefault(s => s.MaKh == ma);
            return Ok(ds);
        }
        [HttpPost("add new customer")]
        public void AddCustomer([FromBody] Khachhang kh)
        {
            Customer c = new Customer();
            c.HoTenKh = kh.HoTenKh;
            c.DiaChi = kh.DiaChi;
            c.NgaySinh = kh.NgaySinh;
            c.Sdt = kh.Sdt;

            da.Customers.Add(c);
            da.SaveChanges();
        }
        [HttpPut("Edit a customer")]
        public void EditCustomer([FromBody] Khachhang kh)
        {
            Customer c = da.Customers.FirstOrDefault(s => s.MaKh == kh.MaKh);
            c.HoTenKh = kh.HoTenKh;
            c.DiaChi = kh.DiaChi;
            c.NgaySinh = kh.NgaySinh;
            c.Sdt = kh.Sdt;

            da.Customers.Update(c);
            da.SaveChanges();
        }
        [HttpDelete("Delete a customer")]
        public void DeleteCustomer(int id)
        {
            Customer c = da.Customers.FirstOrDefault(s => s.MaKh == id);

            da.Customers.Remove(c);
            da.SaveChanges();
        }
        private object SearchCustomers(SearchCustomer searchCustomer)
        {
            var customers = da.Customers.Where(x => x.HoTenKh.Contains(searchCustomer.Keyword));
            var offset = (searchCustomer.Page - 1) * searchCustomer.Size;
            var total = customers.Count();
            int totalpage = (total % searchCustomer.Size) == 0 ? (int)(total / searchCustomer.Size) : (int)(1 + total / searchCustomer.Size);

            var data = customers.OrderBy(x => x.MaKh).Skip(offset).Take(searchCustomer.Size).ToList();
            var res = new
            {
                Data = data,
                TotalRecord = total,
                TotalPages = totalpage,
                Page = searchCustomer.Page,
                Size = searchCustomer.Size,
            };
            return res;
        }
        // phân trang
        [HttpPost("Pagination")]
        public IActionResult TimCustomer([FromBody] SearchCustomer searchCustomer)
        {
            var ds = SearchCustomers(searchCustomer);
            return Ok(ds);
        }
        // lấy 1 đơn của 1 khách hàng
        [HttpGet("Get Order of 1 customer")]
        public IActionResult GetVIPCustomer(int MaKH)
        {
            var ds = da.Orders.Where(o => o.MaKh == MaKH).Join(da.Customers, o => o.MaKh, c => c.MaKh, (o, c) => new { c.MaKh, o.MaDh, c.HoTenKh, c.Sdt });
            return Ok(ds);
        }
        //Thong ke theo tuoi
        [HttpGet("Thong ke do tuoi khach hang")]
        public IActionResult GetAge(int DoTuoi)
        {
            var ds = da.Customers.Where(s => DateTime.Now.Year - s.NgaySinh.Value.Year == DoTuoi).Join(da.Orders, s => s.MaKh, p => p.MaKh, (s, p) => new { p.MaKh, s.HoTenKh, Tuoi = DateTime.Now.Year - s.NgaySinh.Value.Year, p.MaDh, p.NgayDat, p.ThanhToan });
            return Ok(ds);
        }
    }
}

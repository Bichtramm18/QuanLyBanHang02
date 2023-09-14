using System;
using System.Collections.Generic;

#nullable disable

namespace QuanLyBanHang02.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int MaKh { get; set; }
        public string HoTenKh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace QuanLyBanHang02.Models
{
    public partial class Perfume
    {
        public Perfume()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int MaNh { get; set; }
        public string TenNh { get; set; }
        public int MaNcc { get; set; }
        public int TheTich { get; set; }
        public string Huong { get; set; }
        public int DonGia { get; set; }

        public virtual Supplier MaNccNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

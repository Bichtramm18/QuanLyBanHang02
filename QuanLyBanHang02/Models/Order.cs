using System;
using System.Collections.Generic;

#nullable disable

namespace QuanLyBanHang02.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int MaDh { get; set; }
        public int MaKh { get; set; }
        public DateTime NgayDat { get; set; }
        public string ThanhToan { get; set; }

        public virtual Customer MaKhNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

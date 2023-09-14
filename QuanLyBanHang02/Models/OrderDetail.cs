using System;
using System.Collections.Generic;

#nullable disable

namespace QuanLyBanHang02.Models
{
    public partial class OrderDetail
    {
        public int MaDh { get; set; }
        public int MaNh { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public decimal? GiamGia { get; set; }

        public virtual Order MaDhNavigation { get; set; }
        public virtual Perfume MaNhNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace QuanLyBanHang02.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Perfumes = new HashSet<Perfume>();
        }

        public int MaNcc { get; set; }
        public string TenNcc { get; set; }
        public string QuocGia { get; set; }
        public string DiaChi { get; set; }
        public string Sdt { get; set; }

        public virtual ICollection<Perfume> Perfumes { get; set; }
    }
}

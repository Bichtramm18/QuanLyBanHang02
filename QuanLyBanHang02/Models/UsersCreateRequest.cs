using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHang02.Models
{
    public class UserCreateRequest
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

    }
}

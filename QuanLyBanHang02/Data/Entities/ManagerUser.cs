using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHang02.Data.Entities
{
    public class ManageUser : IdentityUser
    {

        public string? Username { get; set; }

        public string? Password { get; set; }


    }
}
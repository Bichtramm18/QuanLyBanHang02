using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuanLyBanHang02.Controllers;
using QuanLyBanHang02.Data.Entities;
using QuanLyBanHang02.Data;
using QuanLyBanHang02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuanLyBanHang02.Constants;
using QuanLyBanHang02.Data;
using QuanLyBanHang02.Data.Entities;
using QuanLyBanHang02.Models;
using QuanLyBanHang02.Services;
using QuanLyBanHang02.Services;

namespace QuanLyBanHang02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ALoginController : ControllerBase
    {


        private readonly UserManager<ManageUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ManageAppDbContext _context;
        private readonly ILogger<ALoginController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IViewRenderService _viewRenderService;
        private readonly ICacheService _cacheService;
        public ALoginController(UserManager<ManageUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ManageAppDbContext context, IConfiguration configuration, ILogger<ALoginController> logger, IEmailSender emailSender,
            IViewRenderService viewRenderService, ICacheService cacheService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _emailSender = emailSender;
            _viewRenderService = viewRenderService;
            _cacheService = cacheService;
        }

        [HttpPost]

        public async Task<IActionResult> PostUser(UserCreateRequest request) // vì khởi tạo lên ta dùng request
        {
            _logger.LogInformation("Begin PostUser API");

            var user = new ManageUser() // vì tạo một User lên ta dùng User Entites luân vì nó có đủ các tường
            {
                Email = request.Email,
                UserName = request.Username,
                Password = request.Password,

            };
            var result = await _userManager.CreateAsync(user, request.Password); // phương thức CreateAsync đã được Identity.Core, hỗ trợ , bài miên phí ta phải viết nó
            if (result.Succeeded)
            {
                _logger.LogInformation("End PostUser API - Success");
                await _cacheService.RemoveAsync(CacheConstants.GetUsers);
                // send Mail
                var repliedComment = await _context.ManageUsers.FindAsync(user.Id);

                var emailModel = new UserVm()
                {
                    Username = request.Username,
                    Password = request.Password,
                    Email = request.Email,

                };
                //https://github.com/leemunroe/responsive-html-email-template
                var htmlContent = await _viewRenderService.RenderToStringAsync("_PostUserEmail", emailModel);
                //await _emailSender.SendEmailAsync(request.Email, "Bạn đã tạo tài khoản thành công", htmlContent);


                return CreatedAtAction(nameof(GetByUsername), new { name = user.UserName }, request);
            }
            else
            {
                _logger.LogInformation("End PostUser API - Fails");

                return BadRequest(new ApiBadRequestResponse(result));
            }
        }




        [HttpGet]

        public async Task<IActionResult> GetUsers()
        {


            var cachedData = await _cacheService.GetAsync<List<UserVm>>(CacheConstants.GetUsers);

            if (cachedData == null)
            {
                var users = _userManager.Users.AsNoTracking();
                var uservms = await users.Select(u => new UserVm() // vì muốn xem lên ta dùng UserVm
                {
                    Username = u.Username,
                    Password = u.Password,
                    Email = u.Email,

                }).ToListAsync();

                if (uservms.Count > 0)
                {
                    await _cacheService.SetAsync(CacheConstants.GetUsers, uservms, 24);

                    cachedData = uservms;
                }

            }


            return Ok(cachedData);
        }

        [HttpGet("{Find user by name}")]

        public async Task<IActionResult> GetByUsername(string name)
        {
            var user = await _userManager.FindByIdAsync(name);
            if (user == null)
                return NotFound(new ApiNotFoundResponse($"Cannot found user with id: {name}"));

            var userVm = new UserVm()
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password,

            };
            return Ok(userVm);
        }


    }
}

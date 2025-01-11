using libraryManagment.Core.Model;
using libraryManagment.Core.Services;
using libraryManagment.EF.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace libraryManagment.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(ITokenService service,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _service = service;
           _userManager = userManager;
           _signInManager = signInManager;
        }

       // [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUserAsync()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x=>x.Type==ClaimTypes.Email).Value;

            var user = await _userManager.FindByEmailAsync(email);


            return new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = await _service.GetTokenAsync(user)

            };



        }





        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

           

            var user= await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null) {
                return Unauthorized();
            
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);

            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Invalid email or password" });
            }

            var dto = new UserDto
            {
                Email = user.Email,

                FullName = user.FullName,

                Token = await _service.GetTokenAsync(user)

            };

            return dto;

           





        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (registerDto == null) return BadRequest();

            var user = new ApplicationUser
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,

                UserName = registerDto.Email
            };


            var result= await _userManager.CreateAsync(user,registerDto.Password);

            if(!result.Succeeded)  return BadRequest();


            return new UserDto
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = await _service.GetTokenAsync(user)
            };


        }





    }
}

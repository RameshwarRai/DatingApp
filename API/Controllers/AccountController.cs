using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _TokenService;
        public AccountController(DataContext context,ITokenService TokenService)
        {
            _context=context;
            _TokenService=TokenService;

        }
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO)
        {
            if(await UserExists(registerDTO.Username)) return BadRequest("Userename is taken");

            using var hamc= new HMACSHA512();
            var user=new AppUser{
                UserName=registerDTO.Username.ToLower(),
                PasswordHash=hamc.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt=hamc.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        [HttpPost("login")]
public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
{
    var user=await _context.Users.SingleOrDefaultAsync(x => x.UserName==loginDTO.Username);
    if(user==null) return Unauthorized("Invalid username");
    using var hamc =new HMACSHA512(user.PasswordSalt);
    var computeHash=hamc.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
    
    for(int i=0;i<computeHash.Length;i++)
    {
        if(computeHash[i]!=user.PasswordHash[i]) return Unauthorized("Invalid username");

    }
    
    return new UserDTO
            {
                Username=user.UserName,
                Token=_TokenService.CreateToken(user)
            };
}

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName==username.ToLower());
        }
    }
}
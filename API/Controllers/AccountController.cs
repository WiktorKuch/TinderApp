using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public ITokenService _tokenService { get; }
        private readonly IMapper _mapper;
        public AccountController(DataContext context , ITokenService tokenService,IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _context = context;
            
        }
        
        [HttpPost("register")] // post : api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.UserName)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512(); // passwordSalt using-pozbycie się klasy po jej użyciu

                user.UserName=registerDto.UserName.ToLower();
                user.PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                user.PasswordSalt= hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
                

            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
            .Include(p=>p.Photos)
            .SingleOrDefaultAsync(x=>x.UserName == loginDto.UserName);

            if(user == null) return Unauthorized("invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

           var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
           
           for(int i =0; i< computedHash.Length; i++)
           {
            if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
           }
           
           return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

        } 

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x=>x.UserName == username.ToLower());

        }
    }
}
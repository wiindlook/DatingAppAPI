using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entity;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context,ITokenService tokenService,IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) //nu specificam de unde le luam(body,etc)deoarece ApiController se ocupa de asta
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();//acel using ne asigura ca atunci terminam treaba cu aceasta clasa osa fie distrusa(Dispose) corespunzator se fol de Idispossible interface care are o metoda dispose


            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                user.PasswordSalt = hmac.Key;
            
            _context.Users.Add(user); // nu adauga in baza de date doar o urmareste
            await _context.SaveChangesAsync();
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs=user.KnownAs
                
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
           {
            var user = await _context.Users
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x=>x.UserName==loginDto.Username);//se fol await deoarece se face un apel la baza de date
            if (user == null) return Unauthorized("Invalid username");
            using var hmac = new HMACSHA512(user.PasswordSalt);//are un overload care ia un key ca parametru
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0;i<computedHash.Length;++i) //se fol for deoarece este un byte array si trb sa iteram prin fiecare.
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                KnownAs=user.KnownAs
            };
    }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); //verifica daca exista deja numele in baza de date se fol tolower deoarece cand introducem un nume in baza de date acesta va fi convertit la tolower de asemenea
        }
    }
}

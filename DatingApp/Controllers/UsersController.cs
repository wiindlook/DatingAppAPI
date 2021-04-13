using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entity;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
 
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() // returneaza o lista de useri.Ienum returneaza o lista simpla pentru a returna de asta nu am folosit list deoarce list are si alte optiuni 
        {
            //return await _context.Users.ToListAsync(); //daca nu facem nimic cu ele o variabila declara folosim return
            //practic cand un request merge catre baza de date  codul se pune pe pauza este asignat unui task care merge catre baza de date iar dupa prin await ia raspunsul din task.

            var users = await _userRepository.GetMembersAsync();
           
            return Ok(users);
        }
       
      

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);
           

        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // ne da usename-ul userului din tokenul pe care api-ul il foloseste sa autentfiice userul
            var user = await _userRepository.GetUserByUsernameAsync(username);
            _mapper.Map(memberUpdateDto, user);//mapeaza automat proprietatile
            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user");

        }


        private bool ClaimType(System.Security.Claims.Claim obj)
        {
            throw new NotImplementedException();
        }
    }
}

using DatingApp.Data;
using DatingApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
   
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() // returneaza o lista de useri.Ienum returneaza o lista simpla pentru a returna de asta nu am folosit list deoarce list are si alte optiuni 
        {
            return await _context.Users.ToListAsync(); //daca nu facem nimic cu ele o variabila declara folosim return
            //practic cand un request merge catre baza de date  codul se pune pe pauza este asignat unui task care merge catre baza de date iar dupa prin await ia raspunsul din task.

        }
        [Authorize]
        //api/users/exid
        [HttpGet("{id}")]
        public async  Task<ActionResult<AppUser>> GetUser(int id)
        {
           return  await _context.Users.FindAsync(id);
           
        }
    }
}

using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entity;
using DatingApp.Extensions;
using DatingApp.Helper;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]                                                     //se da [fromquery] pt a stii ce parametrii sa ia.
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams) // returneaza o lista de useri.Ienum returneaza o lista simpla pentru a returna de asta nu am folosit list deoarce list are si alte optiuni 
        {
            //return await _context.Users.ToListAsync(); //daca nu facem nimic cu ele o variabila declara folosim return
            //practic cand un request merge catre baza de date  codul se pune pe pauza este asignat unui task care merge catre baza de date iar dupa prin await ia raspunsul din task.

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername()); 
            userParams.CurrentUsername = user.UserName; //luam userul curent

            if(string.IsNullOrEmpty(userParams.Gender))//verificam ce gender are userul
            {
                userParams.Gender = user.Gender == "male" ? "female" : "male";
            }

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,users.TotalCount, users.TotalPages);

            return Ok(users);
        }



        [HttpGet("{username}", Name = "GetUser")] //ii dam un nume rutei pt a putea folosi createdatroute
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);


        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {

            var username = User.GetUsername(); // ne da usename-ul userului din tokenul pe care api-ul il foloseste sa autentfiice userul
                                               // pentru a nu folosi asta de mai sus mereu ne facem un extensions method pentru a ne usura munca(ClaimPrincipleExtensions)
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

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file) //returnam un photoDto, Avem nevoie ca utilizatorul sa returneze niste date 
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message); //verificam daca exista o eroare si daca da returnam eroarea (este data de cloudinary)

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)//verificam daca userul mai are poze
            {
                photo.IsMain = true; //daca nu mai are poze se pune ca main cea pe care o uploeaza
            }
            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));//mapam si daca da eroare returnam un bad requests
                                                                                                                 //folosim createdatroute deoarece vrem sa returnam raspunsul corect adica Created, de asemenea pentru a lua poza de la ruta care vrem noi.
                                                                                                                 //ca route name folosim getusers

            }

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);//nu folosim async deoarece e incarcata deja in memorie din user repo

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain); //punem un main photo off si cel nou pe on
            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;
            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound();
            if (photo.IsMain) return BadRequest("You cannot delete your main photo, pleb");
            if(photo.PublicId!=null)
            {
                var result= await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message); //daca exista o eroare la stergerea din cloudinary nu vrem sa o stergem din baza de date
            }
            user.Photos.Remove(photo);
            if (await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to delete photo");
        }
    }
}

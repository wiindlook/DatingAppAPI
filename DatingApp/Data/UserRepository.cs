using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entity;
using DatingApp.Helper;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context,IMapper mapper) //avem nevoie de constructor pt a ne injecta serviciul
        {
           _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();                                            //acest expresion tree o sa ajunga la database ,ef o sa dea build la acest query ca un expression tree.

            query = query.Where(u => u.UserName != userParams.CurrentUsername);                                                                              //ca ef sa nu dea tracking entitailor aduse pt eficienta    
            query = query.Where(u => u.Gender == userParams.Gender);                                                                                                                                                //when we use projection we dont need to include bcs EF knows


            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1); //-1 deoarece folosim today,ceea ce inseamna ca n-au avut nica ziua de nastere
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)         // "_" se foloseste in loc de default face acelasi lucru
            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper
                .ConfigurationProvider).AsNoTracking(),
                userParams.PageNumber, userParams.PageSize);
                
        }

        public async Task<IEnumerable<AppUser>> GetUserAsync()
        {
           return await _context.Users
                .Include(p=>p.Photos)
                .ToListAsync();

        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
           return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
           return await _context.Users
                .Include(p=>p.Photos) //eager loading
                .SingleOrDefaultAsync(x =>  x.UserName == username);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; //savechanges async returneaza un int de asta am pus>0
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;//this lets entityframework update and add a flag that this has been modified
        }
    }
}

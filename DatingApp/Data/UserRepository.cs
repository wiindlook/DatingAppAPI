using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entity;
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

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //when we use projection we dont need to include bcs EF knows
                .ToListAsync();
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

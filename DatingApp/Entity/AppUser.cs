using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Entity
{
    public class AppUser
    {
        public int Id { get; set; } //se fac publice pt entity framework
        public string UserName { get; set; } //pt folosirea in viitor a identity

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }



    }
}

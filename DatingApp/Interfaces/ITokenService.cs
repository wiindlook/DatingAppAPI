using DatingApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Interfaces
{
    public interface ITokenService //se folosesc interfete pentru teste si e best practice,de asemenea folosim servicii pentru a respecta single responsability principle
    {
        string CreateToken(AppUser user);
    }
}

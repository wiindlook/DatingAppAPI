using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DTOs
{
    public class RegisterDto
    {
        [Required] //validare pentru a nu putea introduce date null,atributul ApiController se ocupa de validarea parametrilor din endpoint automat
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

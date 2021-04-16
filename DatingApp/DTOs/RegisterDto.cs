using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DTOs
{
    public class RegisterDto
    {
        //validare pentru a nu putea introduce date null,atributul ApiController se ocupa de validarea parametrilor din endpoint automat
        [Required] public string Username { get; set; }

        [Required] public string knownAs { get; set; }

        [Required] public string gender { get; set; }

        [Required] public DateTime DateOfBirth { get; set; }

        [Required] public string City { get; set; }

        [Required] public string Country { get; set; }

        [Required]
        [StringLength(8,MinimumLength =4)]
        public string Password { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int senderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public string RecipientPhotoUrl { get; set; }//pt a arata poza userilor care trimit mesaje
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }  //folosim ? pt ca vrem sa fie optional iar valorea sa fie null daca nu a fost citit mesajul
        public DateTime MessageSent { get; set; } 
       
    }
}

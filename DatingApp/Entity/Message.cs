using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public int senderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public AppUser Recipient { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }  //folosim ? pt ca vrem sa fie optional iar valorea sa fie null daca nu a fost citit mesajul
        public DateTime MessageSent { get; set; } = DateTime.Now;
        public bool SenderDeleted { get; set; } //daca senderul sterge mesajul nu vrem sa dispara din castura recipientului 
        public bool RecipientDeleted { get; set; }
    }
}

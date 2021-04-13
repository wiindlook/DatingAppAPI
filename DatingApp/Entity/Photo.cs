using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.Entity
{   [Table("Photos")]//ca entity framework sa ne creeze un tabel cu numele photos in loc de photo
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; } //for later storage

        public AppUser AppUser { get; set; } //s-a folosit convention EF pentru a configura relatia dintre tabele cum vrei noi ci anume
        //Id-ul sa nu poate fi nullable si atunci cand stergem un user sa se stearga si poza and etc, de la restricted la cascade

        public int AppUserId { get; set; } // cu aceste 2 atribute configuram relatia dintre tabele
    }
   

}
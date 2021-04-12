namespace DatingApp.DTOs
{
    public class PhotoDto //avem nevoie de dto pt a evita referinta circulara
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }


}
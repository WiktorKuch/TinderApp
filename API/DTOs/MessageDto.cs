using SQLitePCL;

namespace API.DTOs
{
    public class MessageDto
    {
         public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }//kiedy wiadomość została wysłana
       // public bool SenderDeleted { get; set; } //info czy sender usunął wiadomość ze swojej skrzynki odbiorczej
        //public bool RecipientDeleted { get; set; } // __||__ odbiorca
    }
}
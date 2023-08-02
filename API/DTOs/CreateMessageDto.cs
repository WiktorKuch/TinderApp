namespace API.DTOs
{
    public class CreateMessageDto //do kogo wysyłamy wiadomość
    {
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
    }
}
namespace API.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string Username { get; set; } //aktualnie zalogowana nazwa użytkownika
        public string Container { get; set; } ="Unread";
        

    }
}
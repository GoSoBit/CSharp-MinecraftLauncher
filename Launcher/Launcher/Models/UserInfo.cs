namespace Launcher.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
        }

        public UserInfo(string id, string email, string username)
        {
            Id = id;
            Email = email;
            Username = username;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
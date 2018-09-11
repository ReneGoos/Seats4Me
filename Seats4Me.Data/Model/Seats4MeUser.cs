namespace Seats4Me.Data.Model
{
    public class Seats4MeUser : DataWithId
    {
        private string _email;

        public string Email { get => _email; set => _email = value.ToLower(); }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
    }
}

namespace Seats4Me.Data.Model
{
    public class Seats4MeUser : DataWithId
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
    }
}

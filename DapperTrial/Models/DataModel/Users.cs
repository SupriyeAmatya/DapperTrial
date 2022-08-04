namespace DapperTrial.Models.DataModel
{
    public class Users
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid ActivationCode { get; set; }

    }
}

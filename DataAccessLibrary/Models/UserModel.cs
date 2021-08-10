namespace DataAccessLibrary
{
    public class UserModel
    {
        public string Email { get; set; }
        public string EncryptedPassword { get; set; }
        public Role Role { get; set; }
    }
}

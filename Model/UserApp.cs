namespace GestCore.Models
{

    public class UserApp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; } // Já armazenado como hash
    }
}
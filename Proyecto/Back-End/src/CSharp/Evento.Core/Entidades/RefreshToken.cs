namespace Evento.Core.Entidades
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime Expiration { get; set; }
        
        public RefreshToken() { }
    }
}
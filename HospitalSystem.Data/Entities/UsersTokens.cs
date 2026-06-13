namespace HospitalSystem.Data.Entities
{
    public partial class UsersTokens
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string RefreshTokenHash { get; set; } = null!;
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
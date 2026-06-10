namespace HospitalSystem.Infrastructure.DTOs.People
{
    public class PersonDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string? Email { get; set; }
        public byte Gender { get; set; }
    }
}

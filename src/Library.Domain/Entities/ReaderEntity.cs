namespace Library.Domain.Entities
{
    public class ReaderEntity
    {
        public int Id { get; set; }

        public string? ReaderCode { get; set; }

        public string FullName { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? ReaderType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain.Entities
{
    /// <summary>
    /// Katalog över fördefinierade kompetenser och kopplingar mot användare.
    /// </summary>
    public class Competence
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Category { get; set; } = string.Empty;

        public int SortOrder { get; set; }
    }

    public class UserCompetence
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int CompetenceId { get; set; }
    }
}

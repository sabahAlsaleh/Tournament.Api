using System.ComponentModel.DataAnnotations;

namespace Tournament.Core.Entities
{
    public class TournamentDetails
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        public string Title { get; set; }   
        public DateTime StartDate { get; set; }
        public ICollection<Game>? Games { get; set; }
    }
}

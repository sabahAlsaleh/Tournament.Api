using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tournament.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public int? TournamentId { get; set; }

        //Navigation Property
        // to solve Self-referencing loop ///[JsonIgnore]
        [JsonIgnore]
        public TournamentDetails? Tournament {get; set; }

    }

}
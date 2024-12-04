using System.Text.Json.Serialization;

namespace Tournament.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public int? TournamentId { get; set; }

        //Navigation Property
        // to solve Self-referencing loop ///[JsonIgnore]
        [JsonIgnore]
        public TournamentDetails? Tournament {get; set; }

    }

}
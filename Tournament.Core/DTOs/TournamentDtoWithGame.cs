using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.DTO;

namespace Tournament.Core.DTOs
{
    public class TournamentDtoWithGame
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } = DateTime.Now.AddMonths(3);
        public ICollection<GameDto> Games { get; set; }

    }
}

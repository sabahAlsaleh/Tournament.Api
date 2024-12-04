using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.DTO
{
    public class GameDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using YamlDotNet.Core.Tokens;

namespace GhiblipediaAPI.Models
{
    public class MovieTag
    {
        [ForeignKey("Movie")]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey("Tag")]
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}

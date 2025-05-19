using System.ComponentModel.DataAnnotations.Schema;
using YamlDotNet.Core.Tokens;

namespace GhiblipediaAPI.Models
{
    public class MovieTag
    {
        [ForeignKey("Movie")]
        public int Movie_id { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey("Tag")]
        public int Tag_id { get; set; }
        public Search_tag Tag { get; set; }
    }
}

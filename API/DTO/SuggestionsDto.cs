using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class SuggestionsDto
    {
        public class Suggestion{
            public int value { get; set; }

            public string label { get; set; }
        }
        
        [Required(ErrorMessage = "Suggestions are required")]
        public IEnumerable Suggestions;
    }
}
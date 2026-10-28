
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Carasec_Daria_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Required]
        public string LastName { get; set; } = string.Empty;



        public ICollection<Book>? Books { get; set; }
    }
}

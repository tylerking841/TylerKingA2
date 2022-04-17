using System.ComponentModel.DataAnnotations;
namespace Lab4.Models
{
    public class Client
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "LastName")]
        [Required]
        public string LastName { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "FirstName")]
        [Required]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "BirthDate")]
        public DateTime BirthDate { get; set; }

        public string FullName {
            get
            {
                 return LastName + ", " + FirstName;
            }
        }
        
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}

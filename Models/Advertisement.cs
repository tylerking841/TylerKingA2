using System.ComponentModel.DataAnnotations;
namespace Lab4.Models
{
    public class Advertisement
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        [Display(Name = "Image")]
        public string Url { get; set; }
       
    }
}

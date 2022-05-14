using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShopAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Category name cannot be blank.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description name cannot be blank.")]
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}

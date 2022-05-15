using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Product title cannot be blank.")]
        public string Title { get; set; }
        public string Description { get; set; }
        
        [Required(ErrorMessage = "List Price cannot be blank.")]
        [Range(1, 100000)]

        [Display(Name = "List Price")]
        public double ListPrice { get; set; }
        [Required(ErrorMessage = "Offer cannot be blank.")]
        [Range(1, 100000)]
        [Display(Name = "Offer Price")]
        public double Price { get; set; }

        //[Required(ErrorMessage = "Price for 51+ cannot be blank.")]
        //[Range(1, 100000)]
        //[Display(Name = "Price for 51+")]
        //public double Price50 { get; set; }
        
        [ValidateNever]
        public string ImageUrl { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime LastUpdateDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Category cannot be blank.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        
    }
}

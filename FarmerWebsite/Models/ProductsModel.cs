using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Prog7311_PartTwo.Models
{
    public class ProductsModel
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(100, ErrorMessage = "Product Name cannot be longer than 100 characters.")]
         [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be zero or more.")]
        [Display(Name = "Product Quantity")]
        public int Proudct_Qaunt { get; set; }

        [BindNever]
        [ValidateNever]
        public string UserId { get; set; }

        [BindNever]
        [ValidateNever]
        public string Farmer { get; set; }    //this is the name of the farmer who added the product

        [Required(ErrorMessage = "Product Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product Price must be greater than 0.")]
       [Display(Name = "Product Price")]
        public double ProductPrice { get; set; }

        [StringLength(1000, ErrorMessage = "Product Description cannot be longer than 1000 characters.")]
           [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }

        [ValidateNever]
        [Url(ErrorMessage = "Please enter a valid URL.")]
           [Display(Name = "Product Image URL")]
        public string ImageURL { get; set; }

        [StringLength(100, ErrorMessage = "Category cannot be longer than 100 characters.")]
           [Display(Name = "Product Category")]
        public string ProductCategory { get; set; }

        [Required(ErrorMessage = "Production date is required.")]
        [DataType(DataType.Date)]
           [Display(Name = "Product Date")]
        public DateTime Production { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual IdentityUser User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Products.Models
{
    public class Product
    {
        [Key]
        public int ProductID {get; set;}
        
        [Required]
        [MaxLength(100)]
        public string Name {get; set;}

        [Required]
        public string Description {get; set;}

        [Required]
        [Range(0,999999999)]
        public double Price {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        public List<Association> Categories { get; set; }

    }
}
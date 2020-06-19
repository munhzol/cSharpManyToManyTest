using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Products.Models
{
    public class Association
    {
        [Key]
        public int AssociationID {get; set;}

        public int ProductID {get; set;}

        public int CategoryID {get; set;}

        public Category AddedCategory { get; set; }

        public Product AddedProduct { get; set; }

    }
}
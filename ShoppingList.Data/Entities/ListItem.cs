using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Data.Entities
{
    public class ListItem
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Product { get; set; }
        public int Quantity { get; set; }
        public bool IsBought { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastUpdated { get; set; }

        //[EmailAddress]
        //[Phone]
        //[Display(Name ="Last Update")]
        //[Required, StringLength(50)]
        //[Range(0.0, (double)decimal.MaxValue)]
        //[Column(TypeName = "decimal(9,2)")]
        //public decimal RCprodWater { get; set; }
        //[Range(0, int.MaxValue)]
    }
}

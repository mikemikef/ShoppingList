using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Models
{
    public class ListItemDTO
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Product { get; set; }
        public int Quantity { get; set; }
        public bool IsBought { get; set; }
        public DateTime? Created { get; set; }
    }
}

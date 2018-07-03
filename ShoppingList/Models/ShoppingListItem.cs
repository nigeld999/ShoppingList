using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingList.Models
{
    public class ShoppingListItem
    {
        [Key]
        public int Id {get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        public bool IsNeededToBuy { get; set; } = true;
    }
}
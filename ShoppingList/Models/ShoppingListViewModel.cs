using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingList.Models
{
    public class ShoppingListViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsNeededToBuy { get; set; }
        public bool Remove { get; set; }
    }
}
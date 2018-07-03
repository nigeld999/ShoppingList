namespace ShoppingList.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ShoppingList.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingList.Models.ShoppingListDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShoppingList.Models.ShoppingListDb context)
        {
            //  This method will be called after migrating to the latest version.

            context.ShoppingListItems.AddOrUpdate(i => i.Id,
               new ShoppingListItem { Id = 1, Description = "Cheese" },
               new ShoppingListItem { Id = 2, Description = "Milk" },
               new ShoppingListItem { Id = 3, Description = "Bread" },
               new ShoppingListItem { Id = 4, Description = "Pasta" },
               new ShoppingListItem { Id = 5, Description = "Biscuits" },
               new ShoppingListItem { Id = 6, Description = "Steak" },
               new ShoppingListItem { Id = 7, Description = "Bananas" },
               new ShoppingListItem { Id = 8, Description = "Apples" },
               new ShoppingListItem { Id = 9, Description = "Orange Juice" }
               );
        }
    }
}

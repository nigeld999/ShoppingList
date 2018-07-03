using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity; 


namespace ShoppingList.Models
{
    public class ShoppingListDb : DbContext, IShoppingListDb
    {

        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

        IQueryable<T> IShoppingListDb.Query<T>()
        {
            return Set<T>();
        }

        void IShoppingListDb.Add<T>(T entity)
        {
            Set<T>().Add(entity);
            Entry(entity).State = EntityState.Added;
        }

        void IShoppingListDb.Update<T>(T entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        void IShoppingListDb.Remove<T>(T entity)
        {
            Set<T>().Remove(entity);
            Entry(entity).State = EntityState.Deleted;
        }

        void IShoppingListDb.SaveChanges()
        {
            SaveChanges();
        }

    }
}
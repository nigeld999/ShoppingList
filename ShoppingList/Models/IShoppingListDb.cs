using System;
using System.Data.Entity;
using System.Linq;

namespace ShoppingList.Models
{
    public interface IShoppingListDb : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void SaveChanges();
    }
}
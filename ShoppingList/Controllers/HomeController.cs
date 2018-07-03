using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingList.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ShoppingList.Controllers
{
    public class HomeController : Controller
    {
        IShoppingListDb _shoppingListDb;

        public HomeController()
        {
            _shoppingListDb = new ShoppingListDb();
        }

        //
        // GET: /ShoppingListItem(s)
        public ActionResult Index(bool showOnlyItemsToBuy = false)
        {
            var shoppingList = _shoppingListDb.Query<ShoppingListItem>()
                .Where(i => i.IsNeededToBuy == (showOnlyItemsToBuy == false ? i.IsNeededToBuy : true) )
                .OrderBy(i => i.Description)
                .Select(i => new ShoppingListViewModel
                {
                    Id = i.Id,
                    Description = i.Description,
                    IsNeededToBuy = i.IsNeededToBuy,
                    Remove = false
                }).ToList();

            if (showOnlyItemsToBuy)
            {
                return View("FinalShoppingList", shoppingList);
            }
            else
            {
                return View(shoppingList);
            }
        }


        [HttpPost]
        public ActionResult Index(List<ShoppingListViewModel> shoppingListItems, string command)
        {
            try
            {
                ModelState.Clear();

                if (command == "Add Item")
                {
                    shoppingListItems.Insert(0, new Models.ShoppingListViewModel { Id = -1, Description = "", IsNeededToBuy = true, Remove = false });

                    return View(shoppingListItems);
                }
                else if (command == "Cancel Changes")
                {
                    return RedirectToAction("Index");
                }
                else if (command == "Remove Selected")
                {
                    var newShoppingListItems = shoppingListItems.Where(i => i.Remove == false); 

                    return View(newShoppingListItems);
                }
                else if ((command == "Save list") || (command == "View Next Shop List"))
                  {
                    if (ModelState.IsValid)
                    {
                        //Remove any items marked for removal
                        var newShoppingListItems = shoppingListItems.Where(i => i.Remove == false).ToList();

                        DeleteItems(newShoppingListItems);

                        UpdateItems(newShoppingListItems);

                        AddItems(shoppingListItems);

                        //Save the db entity changes
                        _shoppingListDb.SaveChanges();

                        if (command == "View Next Shop List")
                        {
                            return RedirectToAction("Index", "Home",  new { showOnlyItemsToBuy = true } );
                        }

                    }

                }

                //return original list
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                //error has occured show error message to user
                return RedirectToAction("Error");
            }
        }


        public ActionResult Error()
        {
            ViewBag.Message = "Sorry an error has occured in the application - Please contact your system administrator";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Shopping List Helper Application v1.0.0.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Please contact your system Administrator.";

            return View();
        }


        private void AddItems(List<ShoppingListViewModel> shoppingListItems)
        {
            //Add Items in db where Id is -1
            shoppingListItems.Where(i => i.Id == -1)
                                .Select(i =>
                                        new ShoppingListItem
                                        {
                                            Id = i.Id,
                                            Description = i.Description,
                                            IsNeededToBuy = i.IsNeededToBuy
                                        })
                                        .ToList()
                                        .ForEach((up) => CreateDBItem(up));
        }

        private void UpdateItems(List<ShoppingListViewModel> shoppingListItems)
        {
            //Update Items on db where Ids match and data has changed
            shoppingListItems
                     .Where(db => _shoppingListDb.Query<ShoppingListItem>().Any(i => i.Id == db.Id
                                        && (i.Description != db.Description || i.IsNeededToBuy != db.IsNeededToBuy)))
                                        .Select(i =>
                                               new ShoppingListItem
                                               {
                                                   Id = i.Id,
                                                   Description = i.Description,
                                                   IsNeededToBuy = i.IsNeededToBuy
                                               })
                                               .ToList()
                                                 .ForEach((i) => ModifyDbItem(i));
        }

        private void DeleteItems(List<ShoppingListViewModel> shoppingListItems)
        {
            //Delete Items on db where Id not in list
            _shoppingListDb.Query<ShoppingListItem>().ToList()
                                        .Where(db =>
                                            !shoppingListItems.Select(i => i.Id)
                                            .Contains(db.Id))
                                            .ToList()
                                            .ForEach((i) => DeleteDbItem(i));
        }

        private void CreateDBItem(ShoppingListItem shoppingListItem) => _shoppingListDb.Add(shoppingListItem); //_shoppingListDb.Entry(shoppingListItem).State = EntityState.Added;//_shoppingListDb.SaveChanges();

        private void DeleteDbItem(ShoppingListItem shoppingListViewItem)
        {
            ShoppingListItem shoppingListItem = _shoppingListDb.Query<ShoppingListItem>()
                                                        .Single(db => db.Id == shoppingListViewItem.Id);
            _shoppingListDb.Remove(shoppingListItem);
        }

        private void ModifyDbItem(ShoppingListItem shoppingListViewItem)
        {
            ShoppingListItem shoppingListItem = _shoppingListDb.Query<ShoppingListItem>()
                                                        .Single(db => db.Id == shoppingListViewItem.Id);

            shoppingListItem.Description = shoppingListViewItem.Description;
            shoppingListItem.IsNeededToBuy = shoppingListViewItem.IsNeededToBuy;

            _shoppingListDb.Update(shoppingListItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (_shoppingListDb != null)
            {
                _shoppingListDb.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
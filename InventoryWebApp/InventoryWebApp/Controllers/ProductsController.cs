using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryWebApp.Models;
using Microsoft.AspNet.Identity;

namespace InventoryWebApp.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId().ToString();
            var products = db.Products.Where(p => p.UserId == currentUserId);
            
            var restocks = db.Restocks;
            foreach (var p in products)
            {
                restocks.Where(r => r.ProductId == p.ProductId);
            }

            decimal totalInvestment = 0;
            foreach(var r in restocks)
            {
                totalInvestment += (r.PurchasePrice * r.QuantityPurchased);
            }

            decimal projectedTotal = 0;
            foreach(var p in products)
            {
                projectedTotal += (p.Price * p.Quantity);
            }

            decimal projectedProfit = projectedTotal - totalInvestment;

            ViewBag.TotalInvestment = totalInvestment;
            ViewBag.ProjectedTotal = projectedTotal;
            ViewBag.ProjectedProfit = projectedProfit;

            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Quantity,Price,UserId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.UserId = User.Identity.GetUserId();
                product.Quantity = 0;

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }


            TempData["ProductId"] = product.ProductId;
            TempData["UserId"] = product.UserId;
            TempData["Quantity"] = product.Quantity;

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Quantity,Price,UserId")] Product product)
        {
            product.Quantity = (decimal)TempData["Quantiy"];
            product.UserId = (string)TempData["UserId"];

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            var restock = db.Restocks.Where(r => r.ProductId == id);
            db.Products.Remove(product);
            foreach(var r in restock)
            {
                db.Restocks.Remove(r);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

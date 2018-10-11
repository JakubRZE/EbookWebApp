﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EbookWebApp.Models;
using Microsoft.AspNet.Identity;

namespace EbookWebApp.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Get(int id)
        {
            return View();
        }

        // POST: Order/Get
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Get(int id)
        {
            var userId = User.Identity.GetUserId();

            if ( ModelState.IsValid)
            {
                var order = new Order
                {
                    AplicationUserId = userId,
                    BookId = id
                };

                db.Orders.Add(order);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

            // GET: Order
            public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.AplicationUser).Include(o => o.Book);
            return View(orders.ToList());
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            ViewBag.AplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", "LastName");
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderDate,BookId,AplicationUserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", order.AplicationUserId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", order.BookId);
            return View(order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.AplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", order.AplicationUserId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", order.BookId);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderDate,BookId,AplicationUserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AplicationUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", order.AplicationUserId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", order.BookId);
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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

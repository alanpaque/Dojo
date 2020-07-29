using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using Dojo.Models;

namespace Dojo.Controllers
{
    public class ArtMartialsController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            return View(db.ArtMartials.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtMartial artMartial = db.ArtMartials.Find(id);
            if (artMartial == null)
            {
                return HttpNotFound();
            }
            return View(artMartial);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom")] ArtMartial artMartial)
        {
            if (ModelState.IsValid)
            {
                db.ArtMartials.Add(artMartial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artMartial);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtMartial artMartial = db.ArtMartials.Find(id);
            if (artMartial == null)
            {
                return HttpNotFound();
            }
            return View(artMartial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom")] ArtMartial artMartial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artMartial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artMartial);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtMartial artMartial = db.ArtMartials.Find(id);
            if (artMartial == null)
            {
                return HttpNotFound();
            }
            return View(artMartial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArtMartial artMartial = db.ArtMartials.Find(id);
            db.ArtMartials.Remove(artMartial);
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

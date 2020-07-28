using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BO;
using Dojo.Models;

namespace Dojo.Controllers
{
    public class ArmesController : Controller
    {
        private Context db = new Context();
 
        public ActionResult Index()
        {
            return View(db.Armes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arme arme = db.Armes.Find(id);
            if (arme == null) {
                return HttpNotFound();
            }
            return View(arme);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Arme arme)
        {
            if (ModelState.IsValid) {
                db.Armes.Add(arme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(arme);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arme arme = db.Armes.Find(id);
            if (arme == null) {
                return HttpNotFound();
            }
            return View(arme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Arme arme)
        {
            if (ModelState.IsValid) {
                var armeDb = db.Armes.Find(arme.Id);
                armeDb.Nom = arme.Nom;
                armeDb.Degats = arme.Degats;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arme);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arme arme = db.Armes.Find(id);
            if (arme == null) {
                return HttpNotFound();
            }

            try {
                var samourais = db.Samourais.Where(s => s.Arme.Id == id).ToList();
                if (samourais.Any()) {
                    ViewBag.Samourais = samourais.Select(s => s.Nom).ToList();
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }

            return View(arme);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try {
                Arme arme = db.Armes.Find(id);
                var samourais = db.Samourais.Where(s => s.Arme.Id == id).ToList();
                foreach (var samourai in samourais) {
                    samourai.Arme = null;
                }
                db.Armes.Remove(arme);
                db.SaveChanges();
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }

            return RedirectToAction("Index");
        }
        // Coucou petite perruche
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

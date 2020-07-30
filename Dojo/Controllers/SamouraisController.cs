using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BO;
using Dojo.Models;

namespace Dojo.Controllers
{
    public class SamouraisController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null) {
                return HttpNotFound();
            }
            return View(samourai);
        }

        public ActionResult Create()
        {
            var vm = new SamouraiVM();
            List<int> armeIds = db.Samourais.Where(x => x.Arme != null).Select(x => x.Arme.Id).ToList();
            vm.Armes = db.Armes.Where(x => !armeIds.Contains(x.Id)).ToList();
            vm.ArtMartials.AddRange(db.ArtMartials.ToList());
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM vm)
        {
            if (ModelState.IsValid) {
                if (vm.IdSelectedArme.HasValue) {
                    var samourais = db.Samourais.Where(x => x.Arme.Id == vm.IdSelectedArme).ToList();

                    foreach (var item in samourais)
                    {
                        item.Arme = null;
                        db.Entry(item).State = EntityState.Modified;
                    }

                    vm.Samourai.Arme = db.Armes.Find(vm.IdSelectedArme);
                }
                vm.Samourai.ArtMartials = db.ArtMartials.Where(x => vm.ArtMartialsIds.Contains(x.Id)).ToList();
                db.Samourais.Add(vm.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> armeIds = db.Samourais.Where(x => x.Arme != null).Select(x => x.Arme.Id).ToList();
            vm.Armes = db.Armes.Where(x => !armeIds.Contains(x.Id)).ToList();
            vm.ArtMartials.AddRange(db.ArtMartials.ToList());
            return View(vm);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Samourai samourai = db.Samourais.Find(id);

            if (samourai == null) {
                return HttpNotFound();
            }

            var vm = new SamouraiVM();
            vm.Samourai = samourai;
            List<int> armeIds = db.Samourais.Where(x => x.Arme != null && x.Id != id).Select(x => x.Arme.Id).ToList();
            vm.Armes = db.Armes.Where(x => !armeIds.Contains(x.Id)).ToList();

            if (samourai.Arme != null) {
                vm.IdSelectedArme = samourai.Arme.Id;
            }

            vm.ArtMartials.AddRange(db.ArtMartials.ToList());
            vm.ArtMartialsIds = vm.Samourai.ArtMartials.Select(x => x.Id).ToList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiVM vm)
        {
            if (ModelState.IsValid) {
                var samouraiBase = db.Samourais.Find(vm.Samourai.Id);
                samouraiBase.Force = vm.Samourai.Force;
                samouraiBase.Nom = vm.Samourai.Nom;

                if (vm.IdSelectedArme != null) {
                    var samourais = db.Samourais.Where(x => x.Arme.Id == vm.IdSelectedArme).ToList();

                    Arme arme = null;
                    foreach (var item in samourais) {
                        arme = item.Arme;
                        item.Arme = null;
                        db.Entry(item).State = EntityState.Modified;
                    }

                    if (arme == null) {
                        samouraiBase.Arme = db.Armes.FirstOrDefault(x => x.Id == vm.IdSelectedArme);
                    } else {
                        samouraiBase.Arme = arme;
                    }
                } else {
                    samouraiBase.Arme = null;
                }

                foreach (var item in samouraiBase.ArtMartials) {
                    db.Entry(item).State = EntityState.Modified;
                }
                samouraiBase.ArtMartials = db.ArtMartials.Where(x => vm.ArtMartialsIds.Contains(x.Id)).ToList();

                db.Entry(samouraiBase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<int> armeIds = db.Samourais.Where(x => x.Arme != null && x.Id != vm.Samourai.Id).Select(x => x.Arme.Id).ToList();
            vm.Armes = db.Armes.Where(x => !armeIds.Contains(x.Id)).ToList();
            if (vm.Samourai.Arme != null) {
                vm.IdSelectedArme = vm.Samourai.Arme.Id;
            }
            vm.ArtMartials.AddRange(db.ArtMartials.ToList());
            vm.ArtMartialsIds = vm.Samourai.ArtMartials.Select(x => x.Id).ToList();
            return View(vm);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null) {
                return HttpNotFound();
            }
            return View(samourai);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            foreach (var item in samourai.ArtMartials) {
                db.Entry(item).State = EntityState.Modified;
            }
            samourai.ArtMartials.Clear();
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

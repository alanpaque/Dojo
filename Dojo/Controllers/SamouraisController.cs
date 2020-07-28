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
            vm.Armes = db.Armes.ToList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM vm)
        {
            if (ModelState.IsValid) {
                if (vm.IdSelectedArme.HasValue) {
                    vm.Samourai.Arme = db.Armes.FirstOrDefault(a => a.Id == vm.IdSelectedArme.Value);
                }

                db.Samourais.Add(vm.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            vm.Armes = db.Armes.ToList();
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
            vm.Armes = db.Armes.ToList();
            vm.Samourai = samourai;

            if (samourai.Arme != null) {
                vm.IdSelectedArme = samourai.Arme.Id;
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiVM vm)
        {
            if (ModelState.IsValid) {
                var samouraidb = db.Samourais.Find(vm.Samourai.Id);
                samouraidb.Force = vm.Samourai.Force;
                samouraidb.Nom = vm.Samourai.Nom;
                samouraidb.Arme = null;
                if (vm.IdSelectedArme.HasValue) {
                    samouraidb.Arme = db.Armes.FirstOrDefault(a => a.Id == vm.IdSelectedArme.Value);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            vm.Armes = db.Armes.ToList();
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

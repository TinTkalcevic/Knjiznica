using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Knjiznica.Misc;
using Knjiznica.Models;

namespace Knjiznica.Controllers
{
    [Authorize(Roles = OvlastiKorisnik.Administrator + "," + OvlastiKorisnik.Moderator )]
    public class TipoviKnjigaController : Controller
    {
        private BazaDbContext db = new BazaDbContext();

        // GET: TipoviKnjiga
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.PopisTipovaKnjiga.ToList());
        }

        // GET: TipoviKnjiga/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipKnjige tipKnjige = db.PopisTipovaKnjiga.Find(id);
            if (tipKnjige == null)
            {
                return HttpNotFound();
            }
            return View(tipKnjige);
        }

        // GET: TipoviKnjiga/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoviKnjiga/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sifra,naziv")] TipKnjige tipKnjige)
        {
            if (ModelState.IsValid)
            {
                db.PopisTipovaKnjiga.Add(tipKnjige);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipKnjige);
        }

        // GET: TipoviKnjiga/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipKnjige tipKnjige = db.PopisTipovaKnjiga.Find(id);
            if (tipKnjige == null)
            {
                return HttpNotFound();
            }
            return View(tipKnjige);
        }

        // POST: TipoviKnjiga/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sifra,naziv")] TipKnjige tipKnjige)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipKnjige).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipKnjige);
        }

        // GET: TipoviKnjiga/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipKnjige tipKnjige = db.PopisTipovaKnjiga.Find(id);
            if (tipKnjige == null)
            {
                return HttpNotFound();
            }
            return View(tipKnjige);
        }

        // POST: TipoviKnjiga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TipKnjige tipKnjige = db.PopisTipovaKnjiga.Find(id);
            db.PopisTipovaKnjiga.Remove(tipKnjige);
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

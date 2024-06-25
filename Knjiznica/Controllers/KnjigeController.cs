using Knjiznica.Models;
using Knjiznica.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjiznica.Controllers
{
    [Authorize]
    public class KnjigeController : Controller
    {
        BazaDbContext bazaPodataka = new BazaDbContext();


        // GET: Knjige
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Početna stranica";
            ViewBag.Knjiznica = "Međimursko veleučiliste";
            return View();
        }
        [AllowAnonymous]
        public ActionResult Popis(string naslov, string TipKnjige)
        {
            //KnjigaDB knjigadb = new KnjigaDB();
            // return View(knjigadb);
            
            var knjige = bazaPodataka.PopisKnjiga.ToList();

            var tipoviKnjigaList = bazaPodataka.PopisTipovaKnjiga.OrderBy(x => x.naziv).ToList();
            ViewBag.Tipovi = tipoviKnjigaList;

            // filtriranje(pretrazivanje)
            if (!String.IsNullOrWhiteSpace(naslov))
            {
                knjige = knjige.Where(x => x.NaslovIAutor.ToUpper().Contains(naslov.ToUpper())).ToList();

            }
            if (!String.IsNullOrWhiteSpace(TipKnjige))
            {
                knjige = knjige.Where(x => x.SifraTip == TipKnjige).ToList();
            }
            

            return View(knjige);

        }


        public ActionResult Detalji(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Popis");
            }

            //KnjigaDB knjigedb = new KnjigaDB();
            //Knjiga knjiga = knjigedb.VratiListu().FirstOrDefault(x =>x.KnjigaId == id);

            Knjiga knjiga = bazaPodataka.PopisKnjiga.FirstOrDefault(x => x.KnjigaId == id);

            if (knjiga == null)
            {
                return RedirectToAction("Popis");
            }

                return View(knjiga);
        }
        // azuriraj (GET)
        public ActionResult Azuriraj(int? KnjigaId)
        {
            //if (!KnjigaId.HasValue) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            //KnjigaDB knjigaDB = new KnjigaDB();
            //Knjiga knjiga = knjigaDB.VratiListu().FirstOrDefault(x => x.KnjigaId == KnjigaId);

            Knjiga knjiga = null;

            if (!KnjigaId.HasValue)
            {
                knjiga = new Knjiga();
                ViewBag.Title = "Kreiranje Knjige";
                ViewBag.Novi = true;
            }
            else
            {
                knjiga = bazaPodataka.PopisKnjiga.FirstOrDefault(x => x.KnjigaId == KnjigaId);

                        if (knjiga == null) return HttpNotFound();

                ViewBag.Title = "Ažuriranje podataka o knjizi";
                ViewBag.Novi = false;
            }
            
            var tipovi = bazaPodataka.PopisTipovaKnjiga.OrderBy(x => x.naziv).ToList();
            tipovi.Insert(0, new TipKnjige { sifra = "", naziv = "Nedefinirano" });
            ViewBag.Tipovi = tipovi;
            



            //Knjiga knjiga = bazaPodataka.PopisKnjiga.FirstOrDefault(x => x.KnjigaId == KnjigaId);
            return View(knjiga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]  
        // azuriraj (POST)
        public ActionResult Azuriraj(Knjiga k)
        {
            ///
            if(k.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(k.ImageFile.FileName);
                string extension = Path.GetExtension(k.ImageFile.FileName);

                if(extension==".jpg" || extension==".jpeg" || extension == ".png")
                {
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    k.SlikaPutanja = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    k.ImageFile.SaveAs(fileName);
                }
                else
                {
                    ModelState.AddModelError("SlikaPutanja", "Nepodržana ekstenzija");
                }
            }


            if (ModelState.IsValid)
            {
                if (k.KnjigaId != 0)
                {
                    var PostojecaKnjiga = bazaPodataka.PopisKnjiga.Find(k.KnjigaId);
                    if (PostojecaKnjiga != null)
                    {
                        bazaPodataka.Entry(PostojecaKnjiga).CurrentValues.SetValues(k);
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    bazaPodataka.PopisKnjiga.Add(k);
                }

                bazaPodataka.SaveChanges();
                return RedirectToAction("Popis");
            }

            ViewBag.Title = k.KnjigaId == 0 ? "Kreiranje knjige" : "Ažuriranje podataka o knjizi";
            ViewBag.Novi = k.KnjigaId == 0;

            var tipovi = bazaPodataka.PopisTipovaKnjiga.OrderBy(x => x.naziv).ToList();
            tipovi.Insert(0, new TipKnjige { sifra = "", naziv = "Nedefinirano" });
            ViewBag.Tipovi = tipovi;

            return View(k);
        }

        //Brisi (GET metoda)
        public ActionResult Brisi(int? KnjigaId)
        {
            if (KnjigaId == null)
            {
                return RedirectToAction("Popis");
            }

            Knjiga k = bazaPodataka.PopisKnjiga.FirstOrDefault(x => x.KnjigaId == KnjigaId);

            if (k == null)
            {
                return HttpNotFound();
            }

            ViewBag.Title = "Potvrda brisanja knjige";

            return View(k);
        }
        

        //Brisi (POST metoda)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Brisi(int KnjigaId)
        {
            Knjiga k = bazaPodataka.PopisKnjiga.FirstOrDefault(x => x.KnjigaId == KnjigaId);
            
            if (k == null)
            {
                return HttpNotFound();
            }
            
            bazaPodataka.PopisKnjiga.Remove(k);
            bazaPodataka.SaveChanges();

            return View("BrisiStatus");
           // return RedirectToAction("Popis"); // Preusmjeravanje na popis nakon brisanja

        }



        [AllowAnonymous]
        public ActionResult IspisKnjige( string naslov, string autor)
        {

            var knjige = bazaPodataka.PopisKnjiga.ToList();

            if (!String.IsNullOrWhiteSpace(naslov))
            {
                knjige = knjige.Where(x => x.NaslovKnjige.ToUpper().Contains(naslov.ToUpper())).ToList();
            }
            if (!String.IsNullOrWhiteSpace(autor))
            {
                knjige = knjige.Where(x => x.ImeAutora.ToUpper().Contains(autor.ToUpper())).ToList();
            }

            KnjigeReport knjigeReport = new KnjigeReport();
            knjigeReport.ListaKnjiga(knjige);

            return File(knjigeReport.Podaci, System.Net.Mime.MediaTypeNames.Application.Pdf, "PopisKnjiga.pdf");
        }

    }
}
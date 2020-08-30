using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoSite.Models;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ContosoSite.Controllers
{
    public class ExhibitionsController : Controller
    {
        private VistavkiEntities db = new VistavkiEntities();

        // GET: Exhibitions
        public ViewResult Index(string sortOrder, string searchString, string exhibName)
        {
            //VistavkiEntities db = new VistavkiEntities();
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.NameSort = new SelectList(db.Exhibitions, "Id_exhibition", "Name", exhibName);

            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var exhibitions = from s in db.Exhibitions
                              select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                exhibitions = exhibitions.Where(s => s.Name.Contains(searchString)
                                       || s.ThemeOf.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    exhibitions = exhibitions.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    exhibitions = exhibitions.OrderBy(s => s.Date_Open);
                    break;
                case "date_desc":
                    exhibitions = exhibitions.OrderByDescending(s => s.Date_Open);
                    break;
                default:
                    exhibitions = exhibitions.OrderBy(s => s.Name);
                    break;
            }

            if (!String.IsNullOrEmpty(exhibName))
            {
                exhibitions = exhibitions.Where(x => x.Name.Contains(exhibName));
            }
            return View(exhibitions.ToList());
        }
        public FileStreamResult GenerateXML()
        {
            var exhibitions = from s in db.Exhibitions
                              select s;
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;
            XDocument doc = new XDocument();
            XElement xelem = new XElement("Exhibitions");
            doc.Add(xelem);
           
                using (XmlWriter xw = XmlWriter.Create(ms, xws))
                {
                    foreach (var exhib in db.Exhibitions)
                    {
                    xelem.Add(
                     new XElement("Exhibition",
                      new XElement("Name", exhib.Name),
                      new XElement("ThemeOf", exhib.ThemeOf),
                      new XElement("Address", exhib.Address),
                      new XElement("Price", exhib.Price),
                      new XElement("Date_Open", exhib.Date_Open),
                      new XElement("Data_Close", exhib.Date_Close),
                      new XElement("Status_id", exhib.Status_id)
                     )
                    );

                    }
                    doc.WriteTo(xw);
            }
            ms.Position = 0;
            return File(ms, "text/xml", "Exhibitions.xml");
        }
        // GET: Exhibitions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exhibition exhibition = db.Exhibitions.Find(id);
            if (exhibition == null)
            {
                return HttpNotFound();
            }
            return View(exhibition);
        }

        // GET: Exhibitions/Create
        public ActionResult Create()
        {
            ViewBag.Status_id = new SelectList(db.Status, "Id_status", "Name");
            return View();
        }

        // POST: Exhibitions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_exhibition,Name,ThemeOf,Address,Price,Date_Open,Date_Close,Status_id")] Exhibition exhibition)
        {
            if (ModelState.IsValid)
            {
                db.Exhibitions.Add(exhibition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status_id = new SelectList(db.Status, "Id_status", "Name", exhibition.Status_id);
            return View(exhibition);
        }

        // GET: Exhibitions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exhibition exhibition = db.Exhibitions.Find(id);
            if (exhibition == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status_id = new SelectList(db.Status, "Id_status", "Name", exhibition.Status_id);
            return View(exhibition);
        }

        // POST: Exhibitions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_exhibition,Name,ThemeOf,Address,Price,Date_Open,Date_Close,Status_id")] Exhibition exhibition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exhibition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status_id = new SelectList(db.Status, "Id_status", "Name", exhibition.Status_id);
            return View(exhibition);
        }

        // GET: Exhibitions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exhibition exhibition = db.Exhibitions.Find(id);
            if (exhibition == null)
            {
                return HttpNotFound();
            }
            return View(exhibition);
        }

        // POST: Exhibitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exhibition exhibition = db.Exhibitions.Find(id);
            db.Exhibitions.Remove(exhibition);
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

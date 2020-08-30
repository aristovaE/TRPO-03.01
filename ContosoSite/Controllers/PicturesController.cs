using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoSite.Models;

namespace ContosoSite.Controllers
{
    public class PicturesController : Controller
    {
        private VistavkiEntities db = new VistavkiEntities();

        // GET: Pictures
        public ActionResult Index(int? exhib, int? exhibNameExhibition, int? exhibNameArtist)
        {
            // Use LINQ to get list of genres.
           
            ViewBag.Exhibition_id = new SelectList(db.Exhibitions, "Id_exhibition", "Name", exhib); // preselect item in selectlist by CompanyID param
            var picture = from m in db.Pictures
                         select m;

           
            if (exhib!=null)
            {
                picture = picture.Where(x => x.Exhibition_id == exhib);
            }





           
            if (exhibNameExhibition != null)
            {
                ViewBag.NameSort = new SelectList(db.Exhibitions, "Id_exhibition", "Name", exhibNameExhibition);
                picture = picture.Where(x => x.Exhibition_id == exhibNameExhibition);
            }


            if (exhibNameArtist != null)
            {
                ViewBag.NameSort = new SelectList(db.Exhibitions, "Id_artist", "FirstName", exhibNameArtist);
                picture = picture.Where(x => x.Artist_id == exhibNameArtist);
            }
            return View(picture);

           
        }

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Pictures/Create
        public ActionResult Create()
        {
            ViewBag.Artist_id = new SelectList(db.Artists, "Id_artist", "SecondName");
            ViewBag.Exhibition_id = new SelectList(db.Exhibitions, "Id_exhibition", "Name");
            return View();
        }

        // POST: Pictures/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_pic,Name,Exhibition_id,Artist_id,Description,Image")] Picture picture, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        picture.Image = reader.ReadBytes(upload.ContentLength);
                    }
                }
                db.Pictures.Add(picture);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_model = new SelectList(db.Pictures, "Id_pic", "Name", picture.Id_pic);
            return View(picture);
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.Artist_id = new SelectList(db.Artists, "Id_artist", "SecondName", picture.Artist_id);
            ViewBag.Exhibition_id = new SelectList(db.Exhibitions, "Id_exhibition", "Name", picture.Exhibition_id);
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_pic,Name,Exhibition_id,Artist_id,Description,Image")] Picture picture, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(picture).State = EntityState.Modified;
                    if (upload != null && upload.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            picture.Image = reader.ReadBytes(upload.ContentLength);
                        }
                        db.SaveChanges();
                    }

                    else
                    {
                        db.Entry(picture).Property(m => m.Image).IsModified = false;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }

                return View(picture);
            }
            catch (Exception e) { return null; }

        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
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

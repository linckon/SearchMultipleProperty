using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GadgetsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Gadgets
        public async Task<ActionResult> Index(string keyword = "")
        {

            var queryable = db.Gadgets.Include(x => x.Category).AsQueryable();
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                queryable = queryable.Search(keyword, x => x.GadgetID.ToString(),
                 x => x.CategoryID.ToString(),
                 x => x.Category.Name,
                 x => x.Name,
                 x => x.Description,
                 x => x.Price.ToString());
            }

            var list = await queryable.ToListAsync();
            return View(list);
        }

        // GET: Gadgets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gadget gadget = await db.Gadgets.FindAsync(id);
            if (gadget == null)
            {
                return HttpNotFound();
            }
            return View(gadget);
        }

        // GET: Gadgets/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Gadgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GadgetID,Name,Description,Price,Image,CategoryID")] Gadget gadget)
        {
            if (ModelState.IsValid)
            {
                db.Gadgets.Add(gadget);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", gadget.CategoryID);
            return View(gadget);
        }

        // GET: Gadgets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gadget gadget = await db.Gadgets.FindAsync(id);
            if (gadget == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", gadget.CategoryID);
            return View(gadget);
        }

        // POST: Gadgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GadgetID,Name,Description,Price,Image,CategoryID")] Gadget gadget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gadget).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", gadget.CategoryID);
            return View(gadget);
        }

        // GET: Gadgets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gadget gadget = await db.Gadgets.FindAsync(id);
            if (gadget == null)
            {
                return HttpNotFound();
            }
            return View(gadget);
        }

        // POST: Gadgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Gadget gadget = await db.Gadgets.FindAsync(id);
            db.Gadgets.Remove(gadget);
            await db.SaveChangesAsync();
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

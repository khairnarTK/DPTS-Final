using System.Web.Mvc;

namespace DPTS.Web.Controllers
{
    [BaseController.Roles("SuperAdmin", "Admin")]
    public class UserManagerController : Controller
    {
        /*private Entities db = new Entities();

        public async Task<ActionResult> Index()
        {
            var aspNetUsers = db.AspNetUsers.ToList();
            var Roles = new SelectList(db.AspNetRoles.ToList().Where(e => e.Name != "SuperAdmin"), "Name", "Name");
            return View(aspNetUsers);
            return System.Web.UI.WebControls.View(await db.AspNetUsers.ToListAsync());

            return View(await db.AspNetUsers.ToListAsync());
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(
                Include =
                    "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName"
                )] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUser);
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
        }*/
    }
}
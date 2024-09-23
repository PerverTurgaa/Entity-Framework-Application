using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Controllers
{
    public class OgretmenController : Controller
    {
        private readonly DataContext _context;
        public OgretmenController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ogretmenler = await _context.Ogretmenler.ToListAsync();
            return View(ogretmenler);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogretmen model)
        {
            _context.Ogretmenler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogrtmn = await _context
                .Ogretmenler
                .FirstOrDefaultAsync(o => o.OgretmenId == id);


            if (ogrtmn == null) { return NotFound(); }

            return View(ogrtmn);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // güvenlik önlemi
        public async Task<IActionResult> Edit(int id, Ogretmen model)
        {
            if (id != model.OgretmenId) { return NotFound(); }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    //değişiklikler burda kaydediliyor
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Ogretmenler.Any(o => o.OgretmenId == model.OgretmenId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) { return NotFound(); }

            var ogretmen = await _context.Ogretmenler.FindAsync(id);

            if (ogretmen == null) { return NotFound(); }

            return View(ogretmen);

        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var ogretmen = await _context.Ogretmenler.FindAsync(id);

            if (ogretmen == null)
                return NotFound();

            _context.Ogretmenler.Remove(ogretmen);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
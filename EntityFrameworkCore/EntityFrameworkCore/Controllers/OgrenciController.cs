using EntityFrameworkCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly DataContext _context;
        public OgrenciController(DataContext context) 
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ogrenciler = await _context.Ogrenciler.ToListAsync();
            return View(ogrenciler);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model)
        {
            // veri tabanına sorgu da await kullancaz
            // bana context gerekiyor onu inject edicem
            _context.Ogrenciler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            //ekleme yaptıktan sonra home index e yönlendiriyoruz
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogr = await _context
                .Ogrenciler
                .Include(o => o.KursKayitlari)
                .ThenInclude(o =>  o.Kurs)
                .FirstOrDefaultAsync(o=> o.OgrenciId == id);

            // find async de sadece id ye göre arama yaapbilriim
            //var ogr = await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId == id);
            if (ogr == null) {  return NotFound(); } 

            return View(ogr);
            //editcshtmldeki model ogrenciye gelcek
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // güvenlik önlemi
        public async Task<IActionResult> Edit(int id, Ogrenci model)
        {
            if (id != model.OgrenciId) { return NotFound(); }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    //değişiklikler burda kaydediliyor
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if (!_context.Ogrenciler.Any(o => o.OgrenciId == model.OgrenciId))
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

            var ogrenci = await _context.Ogrenciler.FindAsync(id);

            if(ogrenci == null) { return NotFound(); }

            return View(ogrenci);

        }

        [HttpPost]

        //aynı id root içinde de var hangi id yi alcağımı fromform  ile sölyüyorum
        public async Task<IActionResult> Delete([FromForm]int id)
        //<input type = "hidden" name="id" value="@Model.OgrenciId" /> aslında delete.cshtmlden gelen parametreyi almış olcak id olarak
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);

            if (ogrenci == null)
                return NotFound();

            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewKaratIk.Data;
using NewKaratIk.Dtos;
using NewKaratIk.Extentsions;
using NewKaratIk.Models;
using NewKaratIk.Models.CustomModels;
using NewKaratIk.Models.ViewModels;

namespace NewKaratIk.Controllers.Personal_Yonetimi
{
    [Authorize(Roles = "Ozluk")]
    public class OzlukController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public OzlukUserVM OzlukVM { get; set; }
        public OzlukController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            OzlukVM = new OzlukUserVM()
            {

                UserListVM = _db.Users.Include(a => a.Pozisyon).Include(x => x.Ozluk).ToList(),
                OzlukListVM = _db.Ozluks.ToList(),
                PozisyonListVM = _db.Pozisyons.ToList(),

                Ozluk = new Ozluk(),
                Pozisyon = new Pozisyon(),
                User = new User()

            };
        }
        public IActionResult Index()
        {

            return View(OzlukVM);
        }
        public IActionResult Create()
        {
            return View(OzlukVM);
        }
        [HttpPost]
        public IActionResult Create(Ozluk model)
        {
            string wwwPath = _webHostEnvironment.WebRootPath;
            string contentPath = _webHostEnvironment.ContentRootPath;

            string path = Path.Combine(_webHostEnvironment.WebRootPath, @"Dosyalar\Ozluklar");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var files = HttpContext.Request.Form.Files;

            Ozluk os = new Ozluk();
            os.UserId=model.UserId;
            os.Tcno=model.Tcno;
            os.KanGrubu=model.KanGrubu;


            foreach (IFormFile postedFile in files)
            {
                string fileName = DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(postedFile.FileName);

                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    string temp = @"\Dosyalar\Ozluklar\" + fileName;

                    if (postedFile.Name == "NufusKayitOrnegi") { os.NufusKayitOrnegi = temp; }
                    else if (postedFile.Name == "AdliSicil") { os.AdliSicil = temp; }
                    else if (postedFile.Name == "ogrenimBlegesi") { os.ogrenimBlegesi = temp; }
                    else if (postedFile.Name == "YerlisimYeri") { os.YerlisimYeri = temp; }
                    else if (postedFile.Name == "saglikRaporu") { os.saglikRaporu = temp; }
                    else if (postedFile.Name == "NufusCuzdanFotok") { os.NufusCuzdanFotok = temp; }
                    else if (postedFile.Name == "kursBelgeleri") { os.kursBelgeleri = temp; }
                    else if (postedFile.Name == "Fotograf") { os.Fotograf = temp; }
                    else if (postedFile.Name == "AskerlikBelgesi") { os.AskerlikBelgesi = temp; }
                    else if (postedFile.Name == "iskurKayit") { os.iskurKayit = temp; }
                    else if (postedFile.Name == "isBasvuruFormu") { os.isBasvuruFormu = temp; }
                    else if (postedFile.Name == "Muvakatname") { os.Muvakatname = temp; }

                    _db.Ozluks.Add(os);

                    //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }
          

            var user = _db.Users.SingleOrDefault(x => x.Id == model.UserId);
            user.IsOzluk = true;
            _db.Users.Update(user);

            _db.SaveChanges();

            TempData.Put("message", new AlertMessage()
            {
                Message = "Özlük bilgileri Başarıyla Kaydedilmiştir.",
                AlertType = "success"
            });

            return RedirectToAction(nameof(Create));
        }
        public IActionResult Detaylar(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ozluk = _db.Ozluks.SingleOrDefault(x => x.UserId == id);
            if (ozluk == null)
            {
                return NotFound();
            }
            OzlukVM.Ozluk = ozluk;

            return View(OzlukVM);
        }
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ozluk = _db.Ozluks.SingleOrDefault(x => x.UserId == id);
            if (ozluk == null)
            {
                return NotFound();
            }
            OzlukVM.Ozluk = ozluk;

            return View(OzlukVM);
        }
        [HttpPost]
        public IActionResult Edit(Ozluk model)
        {
            string wwwPath = _webHostEnvironment.WebRootPath;
            string contentPath = _webHostEnvironment.ContentRootPath;

            string path = Path.Combine(_webHostEnvironment.WebRootPath, @"Dosyalar\Ozluklar");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var files = HttpContext.Request.Form.Files;

            Ozluk os = _db.Ozluks.SingleOrDefault(x=>x.UserId==model.UserId);
            os.Tcno = model.Tcno;
            os.KanGrubu = model.KanGrubu;
            os.MaasHesapIbanNo = model.MaasHesapIbanNo;
            _db.Ozluks.Update(os);

            foreach (IFormFile postedFile in files)
            {
                string fileName = DateTime.Now.ToFileTime().ToString() + "_" + Path.GetFileName(postedFile.FileName);

                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    string temp = @"\Dosyalar\Ozluklar\" + fileName;

                    if (postedFile.Name == "NufusKayitOrnegi") { os.NufusKayitOrnegi = temp; }
                    else if (postedFile.Name == "AdliSicil") { os.AdliSicil = temp; }
                    else if (postedFile.Name == "ogrenimBlegesi") { os.ogrenimBlegesi = temp; }
                    else if (postedFile.Name == "YerlisimYeri") { os.YerlisimYeri = temp; }
                    else if (postedFile.Name == "saglikRaporu") { os.saglikRaporu = temp; }
                    else if (postedFile.Name == "NufusCuzdanFotok") { os.NufusCuzdanFotok = temp; }
                    else if (postedFile.Name == "kursBelgeleri") { os.kursBelgeleri = temp; }
                    else if (postedFile.Name == "Fotograf") { os.Fotograf = temp; }
                    else if (postedFile.Name == "AskerlikBelgesi") { os.AskerlikBelgesi = temp; }
                    else if (postedFile.Name == "iskurKayit") { os.iskurKayit = temp; }
                    else if (postedFile.Name == "isBasvuruFormu") { os.isBasvuruFormu = temp; }
                    else if (postedFile.Name == "Muvakatname") { os.Muvakatname = temp; }
                   

                    _db.Ozluks.Update(os);

                    //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }
          
            _db.SaveChanges();

            TempData.Put("message", new AlertMessage()
            {
                Message = "Özlük bilgileri Başarıyla Güncellenmiştir.",
                AlertType = "success"
            });

            return RedirectToAction(nameof(Edit));
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewKaratIk.Data;
using NewKaratIk.Dtos;
using NewKaratIk.Extentsions;
using NewKaratIk.Models;
using NewKaratIk.Models.CustomModels;
using NewKaratIk.Models.ViewModels;

namespace NewKaratIk.Controllers.Kariyer_Yonetimi
{

    public class AdayGenelController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IEmailSender _emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public AdayInterviewModelView adayInterviewVM { get; set; }
        public AdayGenelController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {

            _db = db;
            this.webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;


            adayInterviewVM = new AdayInterviewModelView()
            {
                Aday = new Aday(),
                Interview = new interview(),
                User = new User(),
                Nitelik = new Nitelik(),
                Pozisyon = new Pozisyon(),
                MulakatDegerlendirme = new MulakatDegerlendirme(),
                AdayOnaylama = new AdayOnaylama(),
                AdayOnaylamaListVM = _db.AdayOnaylamas.ToList(),

                MulakatDegerlendirmeListVM = _db.MulakatDegerlendirmes.ToList(),
                NitelikListVM = _db.Niteliks.ToList(),
                UserList = _db.Users.ToList(),
                AdayListVM = _db.Adays.ToList(),
                InterviewListVM = _db.Interviews.ToList(),
                PozisyonListVM = _db.Pozisyons.ToList(),
                InterviewUserListVM = _db.InterviewUsers.Include(x => x.User).Include(x => x.Interview).ToList()
            };
        }
        [Authorize(Roles = "PersonalMulakatlar")]
        public IActionResult Mulakatlarim()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var userid = _db.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().Id;
                ViewBag.UserId = userid;
                return View(adayInterviewVM);
            }
        }
        [Authorize(Roles = "PersonalMulakatlar,Mulakatlar")]

        public IActionResult AdayDegerlendirme(int id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var userid = _db.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().Id;
            ViewBag.UserId = userid;

            var Cat = _db.Interviews.Find(id);
            adayInterviewVM.Interview = Cat;
            return View(adayInterviewVM);
        }
        [Authorize(Roles = "PersonalMulakatlar,Mulakatlar")]
        public IActionResult CreateDegerlendirme([FromBody] List<MulakatDegerlendirme> model)
        {


            int MulakatId = 0;
            var userid = _db.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().Id;
            foreach (MulakatDegerlendirme Mulakde in model)
            {
                var iddone = _db.MulakatDegerlendirmes.Where(x => x.mulakId == Mulakde.mulakId && x.userId == userid).ToList();
                var davetEdildimi = _db.InterviewUsers.Where(x => x.interviewId == Mulakde.mulakId && x.UserId == userid).ToList();
                if (iddone.Count() != 0)
                {
                    return Json("var");
                }
                if (davetEdildimi.Count() == 0)
                {
                    return Json("davet");
                }

                MulakatId = (int)Mulakde.mulakId;
                _db.MulakatDegerlendirmes.Add(Mulakde);

            }
            _db.SaveChanges();
            var interview = _db.Interviews.Find(MulakatId);
            var aday = _db.Adays.SingleOrDefault(x => x.Id == interview.AdayId);
            aday.Status = "2";
            interview.isDone = true;
            _db.Adays.Update(aday);


            int insertedRecords = _db.SaveChanges();
            return Json(insertedRecords);
        }
        public IActionResult Teklifionaylamak(int id, string token)
        {
            if (id == null || token == null)
            {
                return NotFound();
            }
            string tkn = "ass9aasopdk5fosadkos65556sadsasd" + (id + 985) + "6sd4654sop45454dkos4adko4sako4554kasodkadsadsasd";

            if (token != tkn)
            {
                return NotFound();
            }

            return View(id);
        }
        [HttpPost]
        public IActionResult Teklifionaylamak(int adayId, string status, string? imzaliTeklif)
        {
            if (adayId == null)
            {
                return NotFound();
            }
            var teklif = _db.TeklifFormus.SingleOrDefault(x => x.AdayId == adayId);
            if (teklif.Status != "2")
            {
                TempData.Put("message", new AlertMessage()
                {

                    Message = "Bu Teklifi Önceden Değerlendirdiniz !",
                    AlertType = "warning"
                });
                return View(adayId);
            }
            var files = HttpContext.Request.Form.Files;
            if (status == "1" && files.Count() == 0)
            {
                TempData.Put("message", new AlertMessage()
                {
                    Message = " İmzaladığınız Teklif Dosyasını Yükleyiniz Lütfen !",
                    AlertType = "danger"
                });
                return View(adayId);
            }
            var aday = _db.Adays.SingleOrDefault(x => x.Id == adayId);
            if (status == "0")
            {
                teklif.Status = status;
                aday.Status = "10";
                TempData.Put("message", new AlertMessage()
                {
                    Message = "Teklifi Red ettiniz, Yanıtınız Başarıyla Gönderildi.",
                    AlertType = "success"
                });
                _db.TeklifFormus.Update(teklif);
                _db.Adays.Update(aday);
                _db.SaveChanges();
                return View(adayId);
            }


            string teklifDosyasi = imzaliTeklif;
            if (files.Count > 0)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);
                FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Dosyalar\\TeklifFormları", ImageName), FileMode.Create);
                files[0].CopyTo(fileStream);
                teklifDosyasi = @"\Dosyalar\TeklifFormları\" + ImageName;
            }
            teklif.Status = status;
            teklif.imzaliTeklifFile = teklifDosyasi;
            aday.Status = "9";
            _db.Adays.Update(aday);
            _db.TeklifFormus.Update(teklif);
            _db.SaveChanges();
            TempData.Put("message", new AlertMessage()
            {

                Message = "Yanıtınız Başarıyla Gönderildi, Aramıza Hoş Geldininz :)",
                AlertType = "success"
            });
            return View(adayId);
        }
        [Authorize(Roles = "PersonalMulakatlar,Adaylar")]
        public IActionResult AdayOnaylama()
        {
            var userid = _db.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().Id;
            ViewBag.UserId = userid;
            return View(adayInterviewVM);
        }
        [Authorize(Roles = "PersonalMulakatlar,Adaylar")]

        [HttpPost]
        public IActionResult AdayOnaylama(int AdayId, string aciklama, string onay, int userId)
        {
            if (AdayId == null || userId == null)
            {
                return NotFound();
            }
            AdayOnaylama adayonayalam = _db.AdayOnaylamas.SingleOrDefault(x => x.UserId == userId && x.AdayId == AdayId);

            adayonayalam.Onay = onay;
            adayonayalam.aciklama = aciklama;
            _db.AdayOnaylamas.Update(adayonayalam);
            _db.SaveChanges();
            var aday = _db.Adays.SingleOrDefault(x => x.Id == AdayId);

            if (aday.Status == "6")
            {
                TempData.Put("message", new AlertMessage()
                {

                    Message = "Bu Aday Bir Kişi Tarafından Onaylanmadı. ",
                    AlertType = "danger"
                });
                return RedirectToAction(nameof(AdayOnaylama));
            }
            var adayOnaylari = _db.AdayOnaylamas.Where(x => x.AdayId == AdayId);
            int onaylayanlar = _db.AdayOnaylamas.Where(x => x.AdayId == AdayId && x.Onay != "2").Count();
            foreach (var item in adayOnaylari)
            {
                if (adayOnaylari.Count() == onaylayanlar && item.Onay == "1")
                {
                    // onaylandı
                    aday.Status = "11";
                    _db.Adays.Update(aday);

                }
            }

            if (onay == "0")
            {
                // onaylanmadı
                aday.Status = "6";
                _db.Adays.Update(aday);
                _db.SaveChanges();

            }
            _db.SaveChanges();

            TempData.Put("message", new AlertMessage()
            {
                Message = "Onaylama işleminiz başarlı bir şekilde gönderildi .",
                AlertType = "success"
            });

            return RedirectToAction(nameof(AdayOnaylama));
        }

    }
}

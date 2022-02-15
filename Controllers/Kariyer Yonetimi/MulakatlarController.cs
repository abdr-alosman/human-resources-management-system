using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewKaratIk.Data;
using NewKaratIk.Models;
using NewKaratIk.Models.CustomModels;
using NewKaratIk.Models.ViewModels;

namespace NewKaratIk.Controllers.Kariyer_Yonetimi
{
    [Authorize(Roles = "Mulakatlar")]
    public class MulakatlarController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IEmailSender _emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public AdayInterviewModelView adayInterviewVM { get; set; }
        public MulakatlarController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
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
                //MulakatDegerlendirme = new MulakatDegerlendirme(),

                MulakatDegerlendirmeListVM = _db.MulakatDegerlendirmes.ToList(),
                NitelikListVM = _db.Niteliks.ToList(),
                UserList = _db.Users.ToList(),
                AdayListVM = _db.Adays.ToList(),
                InterviewListVM = _db.Interviews.ToList(),
                PozisyonListVM = _db.Pozisyons.ToList(),
                InterviewUserListVM = _db.InterviewUsers.Include(x => x.User).Include(x => x.Interview).ToList()
            };
        }
        public IActionResult Index()
        {
            var userid = _db.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().Id;
            ViewBag.UserId = userid;
            return View(adayInterviewVM);
        }
        public IActionResult Create()
        {
            return View(adayInterviewVM);
        }
        public async Task<IActionResult> CreateMulakat([FromBody] List<interview> model)
        {
            if (model == null)
            {
                return Json("0");
            }

            string AdayName = "";
            string AdayPozisyon = "";
            string AdayEmail = "";

            foreach (interview intr in model)
            {
                if (intr.InterviewUsers.Count == 0)
                {
                    return Json("empty");
                }
                _db.Interviews.Add(intr);
                var aday = _db.Adays.SingleOrDefault(x => x.Id == intr.AdayId);
                aday.Status = "1";
                _db.Adays.Update(aday);
                AdayName = aday.NameSurname;
                AdayPozisyon = _db.Adays.Include(x => x.Pozisyon).SingleOrDefault(x => x.Id == intr.AdayId).Pozisyon.Name;
                AdayEmail = _db.Adays.SingleOrDefault(x => x.Id == intr.AdayId).Email;

                if (intr.ilaveNot == null)
                {
                    intr.ilaveNot = "";
                }

                string msg = "<p> Sayın <b>" + AdayName + ", </b><br>Şirketimize göstermiş olduğunuz ilgiye teşekkür ederiz. Karat Güç Sistemleri San. Tic. AŞ olarak yaptığımız değerlendirme sonucu <b> " + AdayPozisyon + "</b> \t " +
                "pozisyonu için bir mülakat gerçekleştirmek isteriz. Mülakat detayları aşağıda paylaşılmıştır. </p>" +
                "<p><b>Mülakat Tarihi :</b>" + intr.interviewDate + "</p><p><b>Yer : </b>" + intr.Yer + "</p><p><b>Başlık :</b>Karat Güç Sistetmleri Mülakat Daveti</p>" +
                "<p>" + intr.ilaveNot + "</p><p>Karat Güç Sistemleri Insan Kaynakları </p>" + "<p>Süreç ile ilgili herhangi bir sorunuz olması durumunda +90 262 677 62 00 nolu telefondan irtibat kurabilirsiniz. </p>" + "<div style='display: inline;'><div style = 'float:left; padding:0px; margin:0px'>" +
                "<img src = 'https://i.ibb.co/vBnHBsF/ddsds.png' /></div >" +
                "<div><p>Karat Güç Sistemleri San.Tic.A.Ş. <br>TAYSAD Org.San.Böl. 3.Cd.No:9 Çayırova / KOCAELİ <br> Tel: +90 262 677 62 00 <br> Fax: +90 262 677 62 50 <br> www.karatguc.com </p>" +
                "</ div ></ div > ";

                await _emailSender.SendEmailAsync(AdayEmail, " Karat Güç Sistetmleri Mülakat Daveti", msg);

                foreach (var item in intr.InterviewUsers)
                {
                    var user = _db.Users.SingleOrDefault(x => x.Id == item.UserId);

                    string msg2 = "<p> Sayın <b>" + user.NameSurname + ", </b><br>Yapılan değerlendirme sonucu <b> " + AdayPozisyon + "</b>" +
                   " &nbsp; pozisyonu için <b>" + AdayName + "</b> isimli aday ile bir mülakat gerçekleştirmek isteriz. Bu Mülakata siz de davetlisiniz. Mülakat detayları aşağıda paylaşılmıştır. </p>" +
                   "<p><b>Mülakat Tarihi :</b>" + intr.interviewDate + "</p><p><b>Yer : </b>" + intr.Yer + "</p><p><b>Başlık :</b>Karat Güç Sistetmleri Mülakat Daveti</p>" +
                   "<p>" + intr.ilaveNot + "</p><p>Karat Güç Sistemleri Insan Kaynakları </p>" + "<div style='display: inline;'><div style = 'float:left; padding:0px; margin:0px'>" +
                   "<img src = 'https://i.ibb.co/vBnHBsF/ddsds.png' /></div >" +
                   "<div><p>Karat Güç Sistemleri San.Tic.A.Ş. <br>TAYSAD Org.San.Böl. 3.Cd.No:9 Çayırova / KOCAELİ <br> Tel: +90 262 677 62 00 <br> Fax: +90 262 677 62 50 <br> www.karatguc.com </p>" +
                   "</ div ></ div > ";

                    await _emailSender.SendEmailAsync(user.Email, "Mülakat Daveti", msg2);

                }
            }
            int insertedRecords = _db.SaveChanges();
            return Json(insertedRecords);
        }
        public IActionResult Delete(int id)
        {
            var mulakatDegerlendirme = _db.MulakatDegerlendirmes.Where(x => x.mulakId == id);
            foreach (var item in mulakatDegerlendirme)
            {
                _db.MulakatDegerlendirmes.Remove(item);
            }
            var interviewusers = _db.InterviewUsers.Where(x => x.interviewId == id);
            foreach (var item in interviewusers)
            {
                _db.InterviewUsers.Remove(item);
            }
            var Cat = _db.Interviews.Find(id);
            TempData["sil"] = "silindi";
            _db.Interviews.Remove(Cat);

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var userid = _db.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().Id;
            ViewBag.UserId = userid;

            if (id == null)
            {
                return NotFound();
            }
            var Cat = _db.Interviews.Find(id);
            adayInterviewVM.Interview = Cat;
            return View(adayInterviewVM);
        }
        public JsonResult EditMulakat([FromBody] List<interview> model)
        {


            List<InterviewUser> yeniusers = new List<InterviewUser>();
            int MulakatId = 0;
            foreach (interview item in model)
            {
                if (item.InterviewUsers.Count == 0)
                {
                    return Json("empty");
                }
                MulakatId = item.Id;
                var mulakat = _db.Interviews.SingleOrDefault(x => x.Id == item.Id);
                // old

                foreach (var yeni in item.InterviewUsers)
                {
                    yeniusers.Add(yeni);
                }
            }
            var intervieusers = _db.InterviewUsers.Where(x => x.interviewId == MulakatId);
            foreach (var item in intervieusers)
            {
                _db.InterviewUsers.Remove(item);

            }
            foreach (var item2 in yeniusers)
            {
                item2.interviewId = MulakatId;
                _db.InterviewUsers.Add(item2);
            }
            int result = _db.SaveChanges();

            return Json(result);
        }
        public IActionResult MulakatDetaylari(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Cat = _db.Interviews.Find(id);
            adayInterviewVM.Interview = Cat;
            return View(adayInterviewVM);

        }
    }
}

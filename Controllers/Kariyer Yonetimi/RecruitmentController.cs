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
    [Authorize(Roles = "IseAlim")]
    public class RecruitmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IEmailSender _emailSender;
        [BindProperty]
        public AdayInterviewModelView AdayInterviewVM { get; set; }
        public RecruitmentController(ApplicationDbContext databaseContext, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db = databaseContext;
            this.webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;

            AdayInterviewVM = new AdayInterviewModelView()
            {
                Aday = new Aday(),
                Interview = new interview(),
                Pozisyon = new Pozisyon(),
                User = new User(),
                AdayOnaylama = new AdayOnaylama(),
                MulakatDegerlendirme = new MulakatDegerlendirme(),
                Department = new Department(),

                DepartmentList = _db.Departments.ToList(),
                AdayListVM = _db.Adays.Include(x => x.Pozisyon).ToList(),
                AdayOnaylamaListVM = _db.AdayOnaylamas.ToList(),
                InterviewListVM = _db.Interviews.Include(x => x.Aday).ToList(),
                PozisyonListVM = _db.Pozisyons.ToList(),
                InterviewUserListVM = _db.InterviewUsers.Include(x => x.User).Include(x => x.Interview).ToList(),
                MulakatDegerlendirmeListVM = _db.MulakatDegerlendirmes.Include(x => x.Aday).Include(a => a.User).ToList(),
                UserList = _db.Users.ToList(),
                TeklifFormListVM = _db.TeklifFormus.ToList()
            };
        }
        public IActionResult OpenPozitions()
        {
            return View(AdayInterviewVM);
        }
        public IActionResult IseAlim()
        {
            var userid = _db.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault().Id;
            ViewBag.UserId = userid;

            return View(AdayInterviewVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IseAlim(Aday model)
        {
            if (model != null)
            {
                string ImagePath = @"\Dosyalar\Adaylar\defualt-image.png";
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string webRootPath = webHostEnvironment.WebRootPath;
                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);
                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Dosyalar\\Adaylar", ImageName), FileMode.Create);
                    files[0].CopyTo(fileStream);
                    ImagePath = @"\Dosyalar\Adaylar\" + ImageName;
                }


                AdayInterviewVM.Aday = model;
                AdayInterviewVM.Aday.CV = ImagePath;
                _db.Adays.Add(AdayInterviewVM.Aday);
                await _db.SaveChangesAsync();
                TempData["success"] = "hi";
                return RedirectToAction(nameof(IseAlim));

            }
            return View(AdayInterviewVM);
        }
        public IActionResult ChangePosition()
        {
            return View(AdayInterviewVM);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cat = _db.Users.Find(id);
            if (cat == null)
            {
                return NotFound();
            }

            AdayInterviewVM.User = cat;
            return View(AdayInterviewVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPost(User model)
        {
            if (model != null)
            {
                var user = _db.Users.SingleOrDefault(x => x.Id == model.Id);
                int poz = (int)_db.Pozisyons.SingleOrDefault(z => z.Id == model.PozisyonId).DepartmentId;
                user.PozisyonId = model.PozisyonId;
                user.DepartmentId = poz;
                _db.Users.Update(user);

                await _db.SaveChangesAsync();
                TempData["success"] = "hi";
                return RedirectToAction(nameof(ChangePosition));
            }
            return View(AdayInterviewVM);
        }
    }
}

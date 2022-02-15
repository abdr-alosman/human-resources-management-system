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
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace NewKaratIk.Controllers.Kariyer_Yonetimi
{
    [Authorize(Roles = "Adaylar")]
    public class AdaylarController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IEmailSender _emailSender;
        [BindProperty]
        public AdayInterviewModelView AdayInterviewVM { get; set; }
        public AdaylarController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {

            _db = db;
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
        
        public IActionResult Index()
        {
            return View(AdayInterviewVM);
        }
      
        public IActionResult Create()
        {
            return View(AdayInterviewVM);
        }
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreatePost(Aday model)
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
                AdayInterviewVM.Aday.CV = ImagePath;
                _db.Adays.Add(AdayInterviewVM.Aday);
                await _db.SaveChangesAsync();
                TempData["success"] = "hi";
                return RedirectToAction(nameof(Create));

            }
            return View(AdayInterviewVM);
        }
        public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cat = _db.Adays.Find(id);
            if (cat == null)
            {
                return NotFound();
            }
            AdayInterviewVM.Aday = cat;
            return View(AdayInterviewVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPost(Aday model)
        {
            if (model != null)
            {
                Aday aday = _db.Adays.SingleOrDefault(x => x.Id == model.Id);
                var files = HttpContext.Request.Form.Files;
                string CVPath = model.CV;

                if (files.Count > 0)
                {
                    string webRootPath = webHostEnvironment.WebRootPath;
                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);
                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Dosyalar\\Adaylar", ImageName), FileMode.Create);
                    files[0].CopyTo(fileStream);

                    CVPath = @"\Dosyalar\Adaylar\" + ImageName;

                }

                aday.CV = CVPath;
                aday.NameSurname = model.NameSurname;
                aday.Email = model.Email;
                aday.Tel = model.Tel;
                aday.KvkkOnayi = model.KvkkOnayi;
                aday.IpAdres = model.IpAdres;
                aday.PozisyonId = model.PozisyonId;
                aday.Status = model.Status;

                _db.Adays.Update(aday);
                _db.SaveChanges();
                TempData.Put("message", new AlertMessage()
                {
                    
                    Message = "Aday Başarıyla Güncellendi .",
                    AlertType = "success"
                });
                return RedirectToAction(nameof(Index));
            }
            return View(AdayInterviewVM);
        }
        public IActionResult OlumluAdaylar()
        {

            return View(AdayInterviewVM);
        }
       
        public IActionResult Delete(int id)
        {
            var mulakat = _db.Interviews.Where(x => x.AdayId == id);
            var deg = _db.MulakatDegerlendirmes.Where(x => x.AdayId == id);

            foreach (var item in mulakat)
            {
                _db.Interviews.Remove(item);
            }
            foreach (var item in deg)
            {
                _db.MulakatDegerlendirmes.Remove(item);
            }


            var Cat = _db.Adays.Find(id);

            string fileName = @"wwwroot\" + Cat.CV;
            // Check if file exists with its full path    
            if ((System.IO.File.Exists(fileName)))
            {
                System.IO.File.Delete(fileName);
            }

            _db.Adays.Remove(Cat);

            _db.SaveChanges();
            TempData["sil"] = "silindi";

            return RedirectToAction("Index");
        }
        public IActionResult olumluAday(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var aday = _db.Adays.Find(id);
            aday.Status = "3";
            _db.SaveChanges();
            return RedirectToAction(nameof(OlumluAdaylar));

        }
        public async Task<IActionResult> olumsuzAday(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var aday = _db.Adays.Include(x => x.Pozisyon).SingleOrDefault(x => x.Id == id);
            aday.Status = "0";
            _db.SaveChanges();

            string msg = "<p> Sayın <b>" + aday.NameSurname + "</b><br/><br/> Şirketimize göstermiş olduğunuz ilgiye teşekkür ederiz. Yaptığımız değerlendirmelerde<b> " + aday.Pozisyon.Name + "</b> \t " +
               "pozisyonunda şu an için birlikte çalışma fırsatı yaratamayacağımız sonucuna vardık. Farklı fırsatlarda değerlendirmek üzere " +
               "özgeçmişinizi havuzumuzda tutmaya devam edeceğiz. Özgeçmişinizin aday havuzumuzdan çıkarılmasını isterseniz ik@karatguc.com adresine özgeçmişinizin aday havuzundan kaldırılmasını istediğinizi " +
               "belirten bir mail atmanız yeterli olacaktır.  </p><p>Karat Güç Sistemleri Insan Kaynakları </p>" +
               "<div style='display: inline;'><div style = 'float:left; padding:0px; margin:0px'>" +
               "<img src = 'https://i.ibb.co/vBnHBsF/ddsds.png' /></div >" +
               "<div><p>Karat Güç Sistemleri San.Tic.A.Ş. <br>TAYSAD Org.San.Böl. 3.Cd.No:9 Çayırova / KOCAELİ <br> Tel: +90 262 677 62 00 <br> Fax: +90 262 677 62 50 <br> www.karatguc.com </p>" +
               "</ div ></ div > ";


            await _emailSender.SendEmailAsync(aday.Email, "Şirketimize ilginiz için çok teşekkürler", msg);
            TempData.Put("message", new AlertMessage()
            {
               
                Message = "Adaya Başarı ile Email Gönderildi .",
                AlertType = "success"
            });
            return RedirectToAction(nameof(Index));

        }
        public IActionResult TeklifFormu(int id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var Cat = _db.Adays.Find(id);
            AdayInterviewVM.Aday = Cat;
            return View(AdayInterviewVM);
        }
        [HttpPost]
        public async Task<IActionResult> TeklifFormuPost(TeklifFormu teklifFormu)
        {
            GC.Collect();
            if (teklifFormu != null)
            {
                var teklif = _db.TeklifFormus.SingleOrDefault(x => x.AdayId == teklifFormu.AdayId);

                if (teklif != null)
                {
                    _db.TeklifFormus.Remove(teklif);
                    string fileName = @"wwwroot\" + teklif.TeklifFile;
                    // Check if file exists with its full path    
                    if ((System.IO.File.Exists(fileName)))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    _db.SaveChanges();

                }
                string ImagePath = @"\Dosyalar\TeklifFormları\defualt-image.png";
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string webRootPath = webHostEnvironment.WebRootPath;
                    string ImageName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);
                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Dosyalar\\TeklifFormları", ImageName), FileMode.Create);
                    files[0].CopyTo(fileStream);
                    ImagePath = @"\Dosyalar\TeklifFormları\" + ImageName;

                }

                int id2 = (int)teklifFormu.AdayId;

                AdayInterviewVM.TeklifFormu = teklifFormu;
                AdayInterviewVM.TeklifFormu.TeklifFile = ImagePath;
                AdayInterviewVM.TeklifFormu.Status = "2";
                _db.TeklifFormus.Add(AdayInterviewVM.TeklifFormu);
                await _db.SaveChangesAsync();


                TempData["success"] = "hi";

                return RedirectToAction("TeklifFormuMailGonder", new { id = id2 });

            }
            return View(AdayInterviewVM);
        }
        public IActionResult TeklifFormuMailGonder(int id)
        {
            GC.Collect();
            var teklif = _db.TeklifFormus.SingleOrDefault(x => x.AdayId == id);
            var aday = _db.Adays.Include(x => x.Pozisyon).SingleOrDefault(x => x.Id == id);
            aday.Status = "7";
            _db.Adays.Update(aday);
            _db.SaveChanges();
            string filePath = teklif.TeklifFile.Substring(25);
            string dd = @"wwwroot\Dosyalar\TeklifFormları\" + filePath;

            Attachment data = new Attachment(dd, MediaTypeNames.Application.Pdf);

            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);


            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.Host = "mail.example.com";
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("example@mail.local", "a123456!");

            string code = "ass9aasopdk5fosadkos65556sadsasd" + (id + 985) + "6sd4654sop45454dkos4adko4sako4554kasodkadsadsasd";

            string url1 = Url.Action("Teklifionaylamak", "AdayGenel", new
            {
                id = aday.Id,
                token = code

            });
        
            var path1 = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + url1;

            string msg = "<p>Sayın <b>" + aday.NameSurname + "</b>, <br>Yapmış olduğumuz aday değerlendirmeleri sonucumuza göre, sizinle  <b> " + aday.Pozisyon.Name + "</b> \t " +
               " Pozisyonumuz ile başlamak istemekteyiz.Bu kapsamda, ekte yer almakta olan yazılı teklifimizi bulabilirsiniz. Teklifimiz " + teklif.TeklifDate + " tarihine kadar geçerli olacak olup, " +
               "Kabul etmeniz durumunda imzalayıp aşağdaki linke tıklayarak iletmeniz hususunda, gereğini bilgilerinize rica eder,Sizi aramızda görmekten mutlu olacağımızı belirtmek isteriz. " +
               "<p>Teklifi Kabul ediyor musunuz ? : <a href='"+ path1 + "'>Teklifi Değerlendirin Lütfen</a></p><p>" + teklif.IlaveNot + "</p>" +
               "<p>süreç ile ilgili herhangi bir sorunuz olması durumunda bizimle bu numaradan ( +90 262 677 62 00 ) irtibat kurmanızı rica ederiz .</ p>" +
               "</p><p>Karat Güç Sistemleri Insan Kaynakları </p>" +
               "<div style='display: inline;'><div style = 'float:left; padding:0px; margin:0px'>" +
               "<img src = 'https://i.ibb.co/vBnHBsF/ddsds.png' /></div >" +
               "<div><p>Karat Güç Sistemleri San.Tic.A.Ş. <br>TAYSAD Org.San.Böl. 3.Cd.No:9 Çayırova / KOCAELİ <br> Tel: +90 262 677 62 00 <br> Fax: +90 262 677 62 50 <br> www.karatguc.com </p>" +
               "</ div ></ div > ";


            MailMessage mm = new MailMessage();

            mm = new MailMessage("example@email.com", aday.Email, "İş Teklifimiz hk", msg);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.Attachments.Add(data);
            client.Send(mm);


            return RedirectToAction(nameof(OlumluAdaylar));
        }
  
        public IActionResult EvrakListesiGonder(int id)
        {
            var aday = _db.Adays.SingleOrDefault(x => x.Id == id);
            aday.Status = "8";
            _db.Adays.Update(aday);
            _db.SaveChanges();
            string filePath = "iş başı evrak listesi.pdf";
            string dd = @"wwwroot\Dosyalar\iş başı evrak listesi.pdf";

            Attachment data = new Attachment(dd, MediaTypeNames.Application.Pdf);

            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);

            SmtpClient client = new SmtpClient();
            client.Port = 25;
            client.Host = "mail.example.com";
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("example@mail.local", "digi9821!");

            string msg = "<p> Sayın <b>" + aday.NameSurname + "</b><br></br>" +
              " Öncelikle aramıza hoş geldiniz. İşe Giriş Evrak Listesi ekte yer almaktadır.Bilgilerinize sunarız." +
              ",Sizi aramızda görmekten mutlu olacağımızı belirtmek isterim. Evraklarınızı başlamadan en az 2 gün önce mesai bitimine kadar tamamlayıp Muhasebe departmanına iletmenizi rica ediyoruz. " +
              "<p>süreç ile ilgili herhangi bir sorunuz olması durumunda bizimle bu numaradan ( +90 262 677 62 00 ) irtibat kurmanızı rica ederiz .</ p>" +
              "</p><p>Karat Güç Sistemleri Insan Kaynakları </p>" +
              "<div style='display: inline;'><div style = 'float:left; padding:0px; margin:0px'>" +
              "<img src = 'https://i.ibb.co/vBnHBsF/ddsds.png' /></div >" +
              "<div><p>Karat Güç Sistemleri San.Tic.A.Ş. <br>TAYSAD Org.San.Böl. 3.Cd.No:9 Çayırova / KOCAELİ <br> Tel: +90 262 677 62 00 <br> Fax: +90 262 677 62 50 <br> www.karatguc.com </p>" +
              "</ div ></ div > ";


            MailMessage mm = new MailMessage();


            mm = new MailMessage("example@mail.com", aday.Email, "İşe Giriş Evrak Listesi Hk.", msg);

            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            mm.Attachments.Add(data);
            client.Send(mm);
            TempData["success"] = "hi";

            return RedirectToAction("OlumluAdaylar");
        }
        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 50)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [Authorize(Roles = "Adaylar")]
        public async Task<IActionResult> KvkkOnay(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Cat = _db.Adays.SingleOrDefault(x => x.Id == id);

            string code = "ass9aasopdk5fosadkos65556sadsasd" + Cat.Id + 985 + "6565565325478965454sop45454dkos4adko4sako4554kasodkadsadsasd";
            var url = Url.Action("KvkkAdayonaylamak", "Adaylar", new
            {
                id = Cat.Id,
                token = code,
                status = true


            }); ;
            var url2 = Url.Action("KvkkAdayonaylamak", "Adaylar", new
            {
                id = Cat.Id,
                token = code,
                status = false


            });
            var path1 = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + url;
            var path2 = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + url2;

            string msg = $"<p> Sayın <b>" + Cat.NameSurname + "</b><br>Özgeçmişinizin Karat Güç Sistemleri AŞ tarafından değerlendirmeye alınabilmesi için Kişisel Verilerin Korunması Aydınlatma metnini onaylamanız beklenmektedir. Kişisel verilerin korunması politikamız için <a href='http://digital.karatguc.com/Reconciliation/KisiselVeri' target='_blank'>tıklayınız</a>" +
            "<br></br>KVKK Politakamızı Onaylıyor musunsuz : <a href='" + path1 + "' style='color:green;'> Kvkk'yı Onaylıyorum</a> &nbsp;	<a href='" + path2 + "' style='color:red;'>  Onaylamıyorum</a>" +
                "<p>Karat Güç Sistemleri Insan Kaynakları </p>" +
               "<div style='display: inline;'><div style = 'float:left; padding:0px; margin:0px'>" +
               "<img src = 'https://i.ibb.co/vBnHBsF/ddsds.png' /></div >" +
               "<div><p>Karat Güç Sistemleri San.Tic.A.Ş. <br>TAYSAD Org.San.Böl. 3.Cd.No:9 Çayırova / KOCAELİ <br> Tel: +90 262 677 62 00 <br> Fax: +90 262 677 62 50 <br> www.karatguc.com </p>" +
               "</ div ></ div > ";


            await _emailSender.SendEmailAsync(Cat.Email, "KVKK Onayı HK.", msg);
            TempData.Put("message", new AlertMessage()
            {
                
                Message = "Adaya Başarı ile Email Gönderildi .",
                AlertType = "success"
            });
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public string KvkkAdayonaylamak(int id, string token, bool status)
        {

            // Retrieve client IP address through HttpContext.Connection
            //var ClientIPAddr = HttpContext.Connection.RemoteIpAddress.MapToIPv6().ToString();
            var ClientIPAddr = Request.HttpContext.Connection.RemoteIpAddress.ToString();


            if (id == null || token == null)
            {
                return "Hata";
            }
            string tkn = "ass9aasopdk5fosadkos65556sadsasd" + id + 985 + "6565565325478965454sop45454dkos4adko4sako4554kasodkadsadsasd";

            if (token != tkn)
            {
                return "Hata";
            }



            Aday aday = _db.Adays.FirstOrDefault(x => x.Id == id);
            if (aday == null)
            {
                return "Hata";
            }
            if (aday.KvkkOnayi == true)
            {
                return null;
            }
            if (status == false)
            {
                aday.KvkkOnayi = status;
                aday.IpAdres = ClientIPAddr;

                _db.Adays.Update(aday);
                _db.SaveChanges();
                return "Kvkk'yı onaylamadınız ";
            }
            else
            {
                aday.KvkkOnayi = status;
                aday.IpAdres = ClientIPAddr;

                _db.Adays.Update(aday);
                _db.SaveChanges();

                return "Kvkk Onayınız Başarı ile alındı. ";
            }

        }
        [Authorize(Roles = "Adaylar")]
        public IActionResult OnayIste(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Cat = _db.Adays.Find(id);
            AdayInterviewVM.Aday = Cat;
            return View(AdayInterviewVM);
        }
        public async Task<IActionResult> OnayaGonder([FromBody] List<AdayOnaylamaDto> model)
        {
            List<string> emails = new List<string>();

            foreach (var item in model)
            {
                if (item.katilanlar.Count() == 0)
                {
                    return Json("hata");
                }
                else
                {
                    foreach (var item2 in item.katilanlar)
                    {
                        var kontrol = _db.AdayOnaylamas.Where(x => x.UserId == item2 && x.AdayId == item.AdayId).ToList();
                        if (kontrol.Count() != 0)
                        {
                            return Json("var");
                        }
                        else
                        {
                            var adayonayalam = new AdayOnaylama();
                            adayonayalam.AdayId = item.AdayId;
                            adayonayalam.UserId = item2;
                            adayonayalam.Onay = "2";
                            _db.AdayOnaylamas.Add(adayonayalam);
                            var userEmail = _db.Users.Where(x => x.Id == item2).SingleOrDefault();
                            string msg = $" Sayın {userEmail.NameSurname}, <br></br> Karat Insan Kaynakları Portalı üzerinde bir adayı işe almak için onayınız istenmektedir. IK Portalına giriş yaparak Onaylama İşlemenizi Yapabilirsiniz." +

                                $"<br></br><p> Karat Güç Sistemleri Insan Kaynakları </p> " +
                               "<div style='display: inline;'><div style = 'float:left; padding:0px; margin:0px'>" +
                               "<img src = 'https://i.ibb.co/vBnHBsF/ddsds.png' /></div >" +
                               "<div><p>Karat Güç Sistemleri San.Tic.A.Ş. <br>TAYSAD Org.San.Böl. 3.Cd.No:9 Çayırova / KOCAELİ <br> Tel: +90 262 677 62 00 <br> Fax: +90 262 677 62 50 <br> www.karatguc.com </p>" +
                               "</ div ></ div > ";
                            await _emailSender.SendEmailAsync(userEmail.Email, "Aday Onaylama Talebi", msg);
                        }
                        var aday = _db.Adays.SingleOrDefault(x => x.Id == item.AdayId);
                        aday.Status = "4";
                        _db.Adays.Update(aday);

                    }
                }
            }

            var result = _db.SaveChanges();
            return Json(result);

        }
    }
}

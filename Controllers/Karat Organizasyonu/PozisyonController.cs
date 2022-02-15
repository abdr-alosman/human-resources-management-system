using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewKaratIk.Data;
using NewKaratIk.Dtos;
using NewKaratIk.Models;
using NewKaratIk.Models.CustomModels;
using NewKaratIk.Models.ViewModels;

namespace NewKaratIk.Controllers.Karat_Organizasyonu
{
    [Authorize(Roles = "Pozisyon")]
    public class PozisyonController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public PozisyonViewModel PozisyonVM { get; set; }
        public PozisyonController(ApplicationDbContext databaseContext)
        {
            _db = databaseContext;
            PozisyonVM = new PozisyonViewModel()
            {
                Pozisyon = new Pozisyon(),
                Nitelik = new Nitelik(),
                User = new User(),
                Department = new Department(),

                DepartmentList = _db.Departments.ToList(),
                UserList = _db.Users.ToList(),
                NitelikListVM = _db.Niteliks.ToList(),
                PozisyonListVM = _db.Pozisyons.ToList()
            };
        }
        public IActionResult Index()
        {
            return View(PozisyonVM);
        }

        public IActionResult Create()
        {

            return View(PozisyonVM);

        }
        [HttpPost]
        public async Task<IActionResult> CreatePozisyon([FromBody] List<PozisyonModel> model)
        {

            List<int> UsersId = new List<int>();
            Pozisyon pozdb = new Pozisyon();
            string pozName = "";
            int pozSayisi = 0;
            int? managerId = 0;
            int UserListcount = 0;

            foreach (var pozisyon in model)
            {
                pozName = pozisyon.Name;
                pozSayisi = pozisyon.pozSayisi;
                managerId = pozisyon.ManagerId;
                UserListcount = pozisyon.UserList.Count();

                pozdb.Name = pozisyon.Name;
                pozdb.pozSayisi = pozisyon.pozSayisi;
                pozdb.NitelikList = pozisyon.NitelikList;
                pozdb.GorevTanimi = pozisyon.GorevTanimi;
                pozdb.ManagerId = pozisyon.ManagerId;
                pozdb.Seviye = pozisyon.Seviye;
                pozdb.DepartmentId = pozisyon.DepartmentId;
                _db.Pozisyons.Add(pozdb);

                foreach (var item1 in pozisyon.UserList)
                {
                    UsersId.Add(item1);
                }
            }
            await _db.SaveChangesAsync();

            int pozId = _db.Pozisyons.FirstOrDefault(x => x.Name == pozName && x.pozSayisi == pozSayisi && x.Status == true && x.ManagerId == managerId).Id;
            AddUserPoz(pozId, UsersId);

            int insertedRecords = await _db.SaveChangesAsync();
            return Json(insertedRecords);
        }
        private void AddUserPoz(int PozId, List<int> UsersId)
        {
            var pozum = _db.Pozisyons.SingleOrDefault(x => x.Id == PozId);
            foreach (var item2 in UsersId)
            {
                var user = _db.Users.Where(x => x.Id == item2).SingleOrDefault();
                user.PozisyonId = PozId;
                user.DepartmentId = pozum.DepartmentId;
            }
        }
        public IActionResult Delete(int id)
        {
            var Cat = _db.Pozisyons.Find(id);
            var employe = _db.Users.FirstOrDefault(x => x.PozisyonId == id);

            if (Cat == null)
            {
                return NotFound();
            }
            if (employe != null)
            {
                TempData["silinmedi"] = "silinmedi";

                return RedirectToAction("Index");
            }

            Cat.Status = false;
            _db.SaveChanges();
            TempData["sil"] = "silindi";

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poz = _db.Pozisyons.Find(id);

            if (poz == null)
            {
                return NotFound();
            }
            TempData["id"] = id;
            PozisyonVM.Pozisyon = poz;
            PozisyonVM.PozisyonListVM = _db.Pozisyons.Include(x => x.Department).ToList();
            return View(PozisyonVM);
        }
        public async Task<IActionResult> EditPozisyon([FromBody] List<PozisyonModel> model)
        {

            List<int> OldUsers = new List<int>();
            List<int> UsersId = new List<int>();

            int pozId = 0;


            foreach (var pozisyon in model)
            {
                var pozisyonum = _db.Pozisyons.Find(pozisyon.Id);

                pozId = pozisyon.Id;

                pozisyonum.Name = pozisyon.Name;
                pozisyonum.pozSayisi = pozisyon.pozSayisi;
                pozisyonum.NitelikList = pozisyon.NitelikList;
                pozisyonum.GorevTanimi = pozisyon.GorevTanimi;
                pozisyonum.ManagerId = pozisyon.ManagerId;
                pozisyonum.Seviye = pozisyon.Seviye;
                pozisyonum.DepartmentId = pozisyon.DepartmentId;
                _db.Pozisyons.Update(pozisyonum);

                if (pozisyonum.UserList != null)
                {
                    foreach (var item1 in pozisyonum.UserList)
                    {
                        OldUsers.Add(item1.Id);
                    }

                    foreach (var item2 in pozisyon.UserList)
                    {
                        UsersId.Add(item2);
                    }
                }
                else
                {
                    foreach (var item2 in pozisyon.UserList)
                    {
                        UsersId.Add(item2);
                    }
                }

            }
            await _db.SaveChangesAsync();

            UpdatePoztion(pozId, UsersId, OldUsers);

            int insertedRecords = _db.SaveChanges();

            return Json(insertedRecords);

        }
        private void UpdatePoztion(int PozId, List<int> UsersId, List<int> OldUsers)
        {
            var pozum = _db.Pozisyons.SingleOrDefault(x => x.Id == PozId);
            foreach (var item in OldUsers)
            {
                var user = _db.Users.Where(x => x.Id == item).SingleOrDefault();
                user.PozisyonId = null;
                user.DepartmentId = null;

            }

            foreach (var item2 in UsersId)
            {
                var user = _db.Users.Where(x => x.Id == item2).SingleOrDefault();
                user.PozisyonId = PozId;
                user.DepartmentId = pozum.DepartmentId;
            }
        }
    }
}

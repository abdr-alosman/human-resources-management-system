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
    [Authorize(Roles = "Departmanlar")]
    public class DepartmanlarController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public UserDepartmentModelView UserDepartmentVM { get; set; }
        public DepartmanlarController(ApplicationDbContext databaseContext)
        {
            _db = databaseContext;
            UserDepartmentVM = new UserDepartmentModelView()
            {
                User = new User(),
                Pozisyon = new Pozisyon(),
                Department = new Department(),
                UserList = _db.Users.AsNoTracking().ToList(),
                DepartmentList = _db.Departments.ToList(),
                PozisyonListVM = _db.Pozisyons.ToList()
            };
        }
        public IActionResult DepartmentList()
        {
            return View(UserDepartmentVM);
        }
        public IActionResult CreateDepartment(int? id)
        {

            DepartmenModel DepartmenModel = new DepartmenModel();

            ViewBag.Id = id;
            if (id == null)
            {
                return View(DepartmenModel);
            }
            var dep = _db.Departments.Find(id);
            DepartmenModel.Id = dep.Id;
            DepartmenModel.Name = dep.Name;
            DepartmenModel.Status = dep.Status;

            return View(DepartmenModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateDepartment(DepartmenModel? model)
        {

            if (model.Id != null)
            {
                Department departman = _db.Departments.Find(model.Id);
                departman.Name = model.Name;
                departman.Status = model.Status;
                _db.Departments.Update(departman);
            }

            else
            {
                Department departman = new Department
                {
                    Name = model.Name,
                    Status = true
                };
                _db.Departments.Add(departman);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("DepartmentList", new { id = model.Id });
        }
    }
}

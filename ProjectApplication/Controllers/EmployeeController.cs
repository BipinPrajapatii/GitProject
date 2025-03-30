using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectApplication.Data;
using ProjectApplication.Entities;
using ProjectApplication.Models.ViewModel;

namespace ProjectApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MyApplicationDbContext _context;

        public EmployeeController(MyApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            ViewBag.Departments = new List<SelectListItem>
        {
            new SelectListItem { Value = "HR", Text = "HR" },
            new SelectListItem { Value = "IT", Text = "IT" },
            new SelectListItem { Value = "Finance", Text = "Finance" }
        };

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new List<SelectListItem>
                {
                new SelectListItem { Value = "HR", Text = "HR" },
                new SelectListItem { Value = "IT", Text = "IT" },
                new SelectListItem { Value = "Finance", Text = "Finance" }
                };

                return View(model);
            }
            var employee = new Employee
            {
                FullName = model.FullName,
                Email = model.Email,
                Department = model.Department,
                Gender = model.Gender,
                IsActive = model.IsActive
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("Success");
        }
        public IActionResult Success()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetEmployees()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = await _context.Employees.CountAsync();

            var query = _context.Employees.AsQueryable();

            // Filtering (Search)
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e => e.FullName.Contains(searchValue) || e.Email.Contains(searchValue));
            }

            // Sorting
            switch (sortColumnIndex)
            {
                case "0":
                    query = sortColumnDirection == "asc" ? query.OrderBy(e => e.FullName) : query.OrderByDescending(e => e.FullName);
                    break;
                case "1":
                    query = sortColumnDirection == "asc" ? query.OrderBy(e => e.Email) : query.OrderByDescending(e => e.Email);
                    break;
                case "2":
                    query = sortColumnDirection == "asc" ? query.OrderBy(e => e.Department) : query.OrderByDescending(e => e.Department);
                    break;
            }

            var filteredRecords = await query.CountAsync();
            var employees = await query.Skip(skip).Take(pageSize).ToListAsync();

            return Json(new
            {
                draw = draw,
                recordsFiltered = filteredRecords,
                recordsTotal = totalRecords,
                data = employees
            });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practical.Data;
using Practical.Enums;
using Practical.Extentions;
using Practical.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practical.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext _context;
        public EmployeeController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index(int order, int sortType = (int)SortType.Asc, bool sortChange = false, int pageSize = 5, int page = 1)
        {
            ViewBag.SortType = sortType;
            ViewBag.order = order;

            if (sortChange)
            {
                if (sortType == (int)SortType.Asc)
                    ViewBag.SortType = SortType.Desc;
                if (sortType == (int)SortType.Desc)
                    ViewBag.SortType = SortType.Asc;
            }

            var query = _context.Employee.Include(x => x.Department).Select(x => new EmployeeListModel
            {
                EmployeeId = x.EmployeeId,
                IsActive = x.IsActive,
                BirthDate = x.BirthDate,
                Department = x.Department.DepartmentName,
                EmployeeName = x.EmployeeName,
                Sex = x.Sex
            });
            
            switch (order)
            {
                case (int)EmployeePageSort.Department:
                    switch (sortType)
                    {
                        case (int)SortType.Asc:
                            query = query.OrderBy(x => x.Department);
                            break;
                        case (int)SortType.Desc:
                            query = query.OrderByDescending(x => x.Department);
                            break;
                    }
                    break;

                case (int)EmployeePageSort.Age:
                    // Age is not in database and I am using building query step by step as I Age propery is evaluate at client side so we can not use that property in order by.
                    // The approch which i have used that will first create the query and accoring to query it will fetch records.
                    // If we want to use sorting on Age we have to take data in a memory( by applying toList() at last where query is first initialize.
                    query = query.OrderByDescending(x => x.Age);
                    break;

                case (int)EmployeePageSort.Active:
                    switch (sortType)
                    {
                        case (int)SortType.Asc:
                            query = query.OrderBy(x => x.IsActive);
                            break;
                        case (int)SortType.Desc:
                            query = query.OrderByDescending(x => x.IsActive);
                            break;
                    }
                    break;

                case (int)EmployeePageSort.Sex:
                    switch (sortType)
                    {
                        case (int)SortType.Asc:
                            query = query.OrderBy(x => x.IsActive);
                            break;
                        case (int)SortType.Desc:
                            query = query.OrderByDescending(x => x.IsActive);
                            break;
                    }

                    break;

                case (int)EmployeePageSort.EmployeeName:
                default:
                    switch (sortType)
                    {
                        case (int)SortType.Asc:
                            query = query.OrderBy(stu => stu.EmployeeName);
                            break;
                        case (int)SortType.Desc:
                            query = query.OrderByDescending(x => x.EmployeeName);
                            break;
                    }

                    break;
            }
            return View(new PagedList<EmployeeListModel>
            {
                TotalRecords = _context.Employee.Count(),
                CurrentPage = page,
                PageSize = pageSize,
                Content = query.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList()
            });
        }

        public IActionResult Create()
        {
            ViewData["Method"] = "Create";
            ViewData["DepartmentList"] = GetDepartmentList();
            return View(new Employee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    if (employee.EmployeeId <= 0)
                    {
                        message = "Employee Inserted Sucessfully!!";
                        _context.Add(employee);
                    }
                    else
                    {
                        var data = _context.Employee.FirstOrDefault(x => x.EmployeeId == employee.EmployeeId);
                        if (data != null)
                        {
                            data.EmployeeId = employee.EmployeeId;
                            data.EmployeeName = employee.EmployeeName;
                            data.Address = employee.Address;
                            data.DepartmentId = employee.DepartmentId;
                            data.BirthDate = employee.BirthDate;
                            data.IsActive = employee.IsActive;
                            data.Sex = employee.Sex;
                        }
                        message = "Employee Updated Sucessfully!!";
                    }
                    await _context.SaveChangesAsync();

                    return new JsonResult(new
                    {
                        Success = true,
                        Message = message
                    });
                }

                return new JsonResult(new
                {
                    Success = false,
                    Message = string.Join("<br>", ModelState.GetErrorMessages())
                });
            }
            catch (Exception ex)
            {
                // we can implemete logic for the logging
                return new JsonResult(new
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Method"] = "Edit";
            ViewData["DepartmentList"] = GetDepartmentList();

            var employees = await _context.Employee.SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }
            return PartialView("Create", employees);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.EmployeeId == id).ConfigureAwait(false);
            if (employee == null)
            {
                return NotFound();
            }

            return PartialView(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employees = await _context.Employee.SingleOrDefaultAsync(m => m.EmployeeId == id);
            _context.Employee.Remove(employees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }

        private List<SelectListItem> GetDepartmentList()
        {
            return _context.Department.Select(x => new SelectListItem
            {
                Value = x.DepartmentId.ToString(),
                Text = x.DepartmentName.ToString()
            }).ToList();
        }
    }
}


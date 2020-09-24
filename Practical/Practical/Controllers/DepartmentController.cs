using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practical.Data;
using Practical.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Practical.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationContext _context;
        public DepartmentController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // reason to get data in a variable is that the package wich i am using EntityFrameworkCore is throwing following exception while perfoming group by so I have took that in a list and use linq query
            // InvalidOperationException: Processing of the LINQ expression '(GroupByShaperExpression: KeySelector: (e.DepartmentId), ElementSelector:(EntityShaperExpression: EntityType: Employee ValueBufferExpression: (ProjectionBindingExpression: Inner) IsNullable: False ) )' by 'RelationalProjectionBindingExpressionVisitor' failed. This may indicate either a bug or a limitation in EF Core. See https://go.microsoft.com/fwlink/?linkid=2101433 for more detailed information.
            var employee = _context.Employee.ToList();
            var department = _context.Department.ToList();
           
            var data = (from d in department
                      join e in employee on
                     d.DepartmentId equals e.DepartmentId into eG
                     select new DepartmentWisEmployeeCountModel
                     {
                         DepartmentName = d.DepartmentName,
                         ActiveEmployeeCount = eG.Count(x=>x.IsActive),
                         InActiveEmployeeCount = eG.Count(x => !x.IsActive)
                     }).ToList();

            //using lamda expression 
            var datausinglamda =( department
                .Join(employee.GroupBy(x => x.DepartmentId), dept => dept.DepartmentId, empGroup => empGroup.Key,
                        (dept, empGroup) => new DepartmentWisEmployeeCountModel { 
                        DepartmentName = dept.DepartmentName,
                        ActiveEmployeeCount = empGroup.Count(z=>z.IsActive),
                        InActiveEmployeeCount = empGroup.Count(z => !z.IsActive)
                    })).ToList();

            return View(data);
        }
    }
}

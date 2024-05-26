using Microsoft.AspNetCore.Mvc;
using MVCCRUD.Data;
using MVCCRUD.Models;
using MVCCRUD.Models.Domain;

namespace MVCCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly CrudDBContext crudDBContext;

        public EmployeeController(CrudDBContext crudDBContext)
        {
            this.crudDBContext = crudDBContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest) 
        {
           var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };
            
           await crudDBContext.Employees.AddAsync(employee);
            await crudDBContext.SaveChangesAsync();
            return RedirectToAction("Add");
            }
        
    }
}

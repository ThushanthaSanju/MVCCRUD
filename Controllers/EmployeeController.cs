using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Index()
        {
            var employees = await crudDBContext.Employees.ToListAsync();
          return View(employees);
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
            return RedirectToAction("Index");
            }

        [HttpGet]
        public async  Task<IActionResult> View(Guid id)
        {
            var employee = await crudDBContext.Employees.FirstOrDefaultAsync(e => e.Id == id);

            if (employee != null)
            {
                var ViewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };
                return await Task.Run(()=> View("View",ViewModel));
            }
           
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel updateEmployeeRequest)
        {
            var employee = await crudDBContext.Employees.FirstOrDefaultAsync(e => e.Id == updateEmployeeRequest.Id);

            if (employee != null)
            {
                employee.Name = updateEmployeeRequest.Name;
                employee.Email = updateEmployeeRequest.Email;
                employee.Salary = updateEmployeeRequest.Salary;
                employee.Department = updateEmployeeRequest.Department;
                employee.DateOfBirth = updateEmployeeRequest.DateOfBirth;

                crudDBContext.Employees.Update(employee);
                await crudDBContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel updateEmployeeRequest)
        {
            var employee = await crudDBContext.Employees.FindAsync(updateEmployeeRequest.Id);

            if (employee != null)
            {
                crudDBContext.Employees.Remove(employee);
                await crudDBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
    }
}

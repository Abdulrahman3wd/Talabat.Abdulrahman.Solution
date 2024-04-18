using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.EmployeeSpecs;
using Talabat.Core.Specifications.ProductSpecs;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeeController(IGenericRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        [HttpGet] // GET : /api/employee
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var spec = new EmployeeWithDepartmentSecifications();
            var employees = await _employeeRepo.GetAllWithSpecAsync(spec);

            return Ok(employees);

        }
        [HttpGet("{id}")] //Get : /api/employee/id 
        public async Task<ActionResult<Employee>>GetEmployee(int id)
        {
            var spec = new EmployeeWithDepartmentSecifications();
            var employee = await _employeeRepo.GetWithSpecAsync(spec);
            if (employee is null)
                return NotFound(new ApiResponse(404));

            return Ok(employee);
        }

    }
}

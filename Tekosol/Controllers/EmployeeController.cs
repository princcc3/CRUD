using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEmployee;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Tekosol;

namespace MyEmployee.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmployeeController(IConfiguration config)
        {
            _config = config;
        }

        //To get all employee details
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<Employee> employees = await SelectAllEmployees(connection);
            return Ok(employees);
        }

        //To get the details of an employee with specific ID
        [HttpGet("{employeeid}")]
            public async Task<ActionResult<List<Employee>>> GetEmployee(int employeeid)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var employee = await connection.QueryAsync<Employee>("SELECT * FROM Employees WHERE EmployeeId = @id",new { id = employeeid });
            return Ok(employee);
        }

        //To insert new employee
        [HttpPost]
        public async Task<ActionResult<List<Employee>>> AddEmployee(Employee emp)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var employee = await connection.QueryAsync<Employee>("INSERT INTO Employees(FirstName, LastName, Email, PhoneNumber, Department) VALUES(@FirstName, @LastName, @Email, @PhoneNumber, @Department)",emp);
            return Ok(await SelectAllEmployees(connection));
        }

        //To update an existing employee details
        [HttpPut("{id}")]
        public async Task<ActionResult<List<Employee>>> UpdateEmployee(int id, Employee emp)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var employee = await connection.QueryAsync<Employee>("UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, Email = @Email,PhoneNumber = @PhoneNumber, Department = @Department WHERE EmployeeId = @id", new { id, emp.FirstName, emp.LastName, emp.Email, emp.PhoneNumber, emp.Department });
            return Ok(await SelectAllEmployees(connection));
        }

        //To delete an employee with using ID
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var employee = await connection.QueryAsync<Employee>("DELETE FROM Employees WHERE EmployeeId = @id", new { id });
            return Ok(await SelectAllEmployees(connection));
        }
        
        //To display full table data after API execution
        private static async Task<IEnumerable<Employee>> SelectAllEmployees(SqlConnection connection)
        {
            return await connection.QueryAsync<Employee>("select * from Employees");
        }

    }
}

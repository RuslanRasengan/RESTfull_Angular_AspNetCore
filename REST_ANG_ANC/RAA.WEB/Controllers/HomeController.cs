﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RAA.DataAccess;
using RAA.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAA.WEB.Controllers
{
    namespace AspNetCoreAngularApp.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class HomeController : ControllerBase
        {
            private readonly ApplicationContext _context;

            public HomeController(ApplicationContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
            {
                return await _context.Employee.ToListAsync();
            }
            [HttpGet("{id}")]
            public async Task<ActionResult<Employee>> GetEmployee(int id)
            {
                var employee = await _context.Employee.FindAsync(id);

                if (employee == null)
                {
                    return NotFound();
                }

                return employee;
            }
            [HttpPut("{id}")]
            public async Task<IActionResult> PutEmployee(int id, Employee employee)
            {
                if (id != employee.Id)
                {
                    return BadRequest();
                }

                _context.Entry(employee).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            [HttpPost]
            public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
            {
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
            }
            [HttpDelete("{id}")]
            public async Task<ActionResult<Employee>> DeleteEmployee(int id)
            {
                var employee = await _context.Employee.FindAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync();

                return employee;
            }

            private bool EmployeeExists(int id)
            {
                return _context.Employee.Any(e => e.Id == id);
            }
        }
    }
}

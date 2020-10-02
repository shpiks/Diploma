using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly DB_for_CourseContext _context;

        public EmployeesController(DB_for_CourseContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Materials>>> GetEmployees(int id)
        {

            _context.Materials.Where(x => x.DateOfTerm < DateTime.Today).ToList().ForEach(x => x.ExecutedOrNotExecuted = true);
            _context.SaveChanges();
            
            var materials = await _context.MaterialEmployees.Include(x => x.MaterialMaterial).Where(x => x.EmployeeEmployeeId == id).Select(x => x.MaterialMaterial).Where(x => x.ExecutedOrNotExecuted != true).ToListAsync();
            

            if (materials == null)
            {
                return NotFound();
            }

            return materials;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployees(int id, Employees employees)
        {
            if (id != employees.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employees).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(id))
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

        [Route("RewriteEmployees")]
        //[HttpPut("{id}")]
        public async Task<IActionResult> RewriteEmployees([FromBody]string stringJson)
        {
            string[] jsonStringArray = stringJson.Split(new char[] { '|' });
            string materialStringJson = jsonStringArray[0];
            string employeeStringJson = jsonStringArray[1];
            Materials materials = JsonConvert.DeserializeObject<Materials>(materialStringJson);
            Employees employees = JsonConvert.DeserializeObject<Employees>(employeeStringJson);

            var materialEmployees = _context.MaterialEmployees.FirstOrDefault(x => x.MaterialMaterial == materials);
            _context.MaterialEmployees.Remove(materialEmployees);
            await _context.SaveChangesAsync();
            var materialTerm = _context.Materials.Find(materials.MaterialId);
            var employeeTerm = _context.Employees.Find(employees.EmployeeId);
            _context.MaterialEmployees.Add(new MaterialEmployees { EmployeeEmployee = employeeTerm, MaterialMaterial = materialTerm });
            await _context.SaveChangesAsync();


            return NoContent();
        }

        [Route("GetEmployeesWhereTermToday/{id}")]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployeesWhereTermToday(int id)
        {
            var employees = await _context.MaterialEmployees.Include(x => x.EmployeeEmployee).Where(x => x.MaterialMaterialId == id).Select(x => x.EmployeeEmployee).ToListAsync();



            if (employees == null)
            {
                return NotFound();
            }

            return employees;
        }

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employees>> PostEmployees(Employees employees)
        {
            _context.Employees.Add(employees);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeesExists(employees.EmployeeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployees", new { id = employees.EmployeeId }, employees);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employees>> DeleteEmployees(int id)
        {
            var employees = await _context.Employees.FindAsync(id);
            var materialEmployees = _context.MaterialEmployees.Where(x => x.EmployeeEmployee == employees).ToList();
            _context.MaterialEmployees.RemoveRange(materialEmployees);
            if (employees == null)
            {
                return NotFound();
            }
            

            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeesExists(long id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}

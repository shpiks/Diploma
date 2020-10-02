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
    public class MaterialsController : ControllerBase
    {
        private readonly DB_for_CourseContext _context;

        public MaterialsController(DB_for_CourseContext context)
        {
            _context = context;
        }

        // GET: api/Materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materials>>> GetMaterials()
        {
            //var materialsWhereTermToday = await _context.Materials.Where(x => x.DateOfTerm == DateTime.Today && x.ExecutedOrNotExecuted != true).ToListAsync();
            //string stringJson = JsonConvert.SerializeObject(material) + "|" + JsonConvert.SerializeObject(employee);


            return await _context.Materials.Where(x => x.DateOfTerm == DateTime.Today && x.ExecutedOrNotExecuted != true).ToListAsync();
            //return await _context.Materials.ToListAsync();
        }

        [Route("GetMaterialsWhereTermTomorrow")]
        public async Task<ActionResult<IEnumerable<Materials>>> GetMaterialsWhereTermTomorrow()
        {

            //db.Materials.ToList().Where(x => x.DateOfTerm == DateTime.Today.AddDays(1) && x.ExecutedOrNotExecuted != true).ToList().ForEach(x => Materials.Add(x));

            return await _context.Materials.Where(x => x.DateOfTerm == DateTime.Today.AddDays(1) && x.ExecutedOrNotExecuted != true).ToListAsync();
            //return await _context.Materials.ToListAsync();
        }

        //GET: api/Materials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Materials>> GetMaterials(int id)
        {
            var materials = await _context.Materials.FindAsync(id);

            if (materials == null)
            {
                return NotFound();
            }

            return materials;
        }

        // PUT: api/Materials/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterials(int id, Materials materials)
        {
            if (id != materials.MaterialId)
            {
                return BadRequest();
            }

            _context.Entry(materials).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialsExists(id))
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

        // POST: api/Materials
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Materials>> PostMaterials([FromBody]string stringJson)
        {
            string[] jsonStringArray = stringJson.Split(new char[] { '|' });
            string materialStringJson = jsonStringArray[0];
            string employeeStringJson = jsonStringArray[1];
            Materials materials = JsonConvert.DeserializeObject<Materials>(materialStringJson);
            Employees employees = JsonConvert.DeserializeObject<Employees>(employeeStringJson);

            _context.Materials.Add(materials);
            
            await _context.SaveChangesAsync();
            var materialTerm = _context.Materials.Find(materials.MaterialId);
            var employeeTerm = _context.Employees.Find(employees.EmployeeId);
            _context.MaterialEmployees.Add(new MaterialEmployees { EmployeeEmployee = employeeTerm, MaterialMaterial = materialTerm });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Materials/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMaterials(int id)
        {
            var materials = await _context.Materials.FindAsync(id);
            if (materials != null)
            {
                var materialEmployees = _context.MaterialEmployees.FirstOrDefault(x => x.MaterialMaterial == materials);
                if (materialEmployees != null)
                    _context.MaterialEmployees.Remove(materialEmployees);

                var victimMaterials = _context.VictimMaterials.Where(x => x.MaterialMaterial == materials).ToList();
                _context.VictimMaterials.RemoveRange(victimMaterials);

                _context.Materials.Remove(materials);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
                return NotFound();
        }



        private bool MaterialsExists(int id)
        {
            return _context.Materials.Any(e => e.MaterialId == id);
        }
    }
}

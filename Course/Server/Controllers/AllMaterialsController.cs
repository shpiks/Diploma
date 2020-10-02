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
    public class AllMaterialsController : ControllerBase
    {
        private readonly DB_for_CourseContext _context;

        public AllMaterialsController(DB_for_CourseContext context)
        {
            _context = context;
        }

        // GET: api/AllMaterials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Materials>>> GetMaterials()
        {
            return await _context.Materials.Where(x => x.ExecutedOrNotExecuted != true).ToListAsync();
        }


        [Route("GetMaterialsByFilterSelectedEmployeeAndSelectedDecision/")]
        public async Task<ActionResult<IEnumerable<Materials>>> GetMaterialsByFilterSelectedEmployeeAndSelectedDecision([FromBody]string stringJson)
        {
            string[] jsonStringArray = stringJson.Split(new char[] { '|' });
            string StartDataStringJson = jsonStringArray[0];
            string FinishDataStringJson = jsonStringArray[1];
            string EmployeeIdStringJson = jsonStringArray[2];
            string SelectedDecisionStringJson = jsonStringArray[3];
            DateTime StartData = JsonConvert.DeserializeObject<DateTime>(StartDataStringJson);
            DateTime FinishData = JsonConvert.DeserializeObject<DateTime>(FinishDataStringJson);
            int EmployeeId = JsonConvert.DeserializeObject<int>(EmployeeIdStringJson);
            string SelectedDecision = JsonConvert.DeserializeObject<string>(SelectedDecisionStringJson);
            return await _context.MaterialEmployees.Include(x => x.MaterialMaterial).Where(x => x.EmployeeEmployeeId == EmployeeId).Select(x => x.MaterialMaterial)
                .Where(x => x.DateOfRegistration >= StartData && x.DateOfRegistration <= FinishData && x.Decision == SelectedDecision).ToListAsync();
            //await _context.Materials.Where(x => x.ExecutedOrNotExecuted != true).ToListAsync();
        }

        [Route("GetMaterialsByFilterSelectedDecision/")]
        public async Task<ActionResult<IEnumerable<Materials>>> GetMaterialsByFilterSelectedDecision([FromBody]string stringJson)
        {
            string[] jsonStringArray = stringJson.Split(new char[] { '|' });
            string StartDataStringJson = jsonStringArray[0];
            string FinishDataStringJson = jsonStringArray[1];
            string SelectedDecisionStringJson = jsonStringArray[2];
            DateTime StartData = JsonConvert.DeserializeObject<DateTime>(StartDataStringJson);
            DateTime FinishData = JsonConvert.DeserializeObject<DateTime>(FinishDataStringJson);
            string SelectedDecision = JsonConvert.DeserializeObject<string>(SelectedDecisionStringJson);
            return await _context.Materials.Where(x => x.DateOfRegistration >= StartData && x.DateOfRegistration <= FinishData && x.Decision == SelectedDecision).ToListAsync();
                //await _context.MaterialEmployees.Include(x => x.MaterialMaterial).Where(x => x.EmployeeEmployeeId == EmployeeId).Select(x => x.MaterialMaterial)
                //.Where(x => x.DateOfRegistration >= StartData && x.DateOfRegistration <= FinishData && x.Decision == SelectedDecision).ToListAsync();

        }

        [Route("GetMaterialsByFilterEmployeeId/")]
        public async Task<ActionResult<IEnumerable<Materials>>> GetMaterialsByFilterEmployeeId([FromBody]string stringJson)
        {
            string[] jsonStringArray = stringJson.Split(new char[] { '|' });
            string StartDataStringJson = jsonStringArray[0];
            string FinishDataStringJson = jsonStringArray[1];
            string EmployeeIdStringJson = jsonStringArray[2];
            
            DateTime StartData = JsonConvert.DeserializeObject<DateTime>(StartDataStringJson);
            DateTime FinishData = JsonConvert.DeserializeObject<DateTime>(FinishDataStringJson);
            int EmployeeId = JsonConvert.DeserializeObject<int>(EmployeeIdStringJson);
            
            return await _context.MaterialEmployees.Include(x => x.MaterialMaterial).Where(x => x.EmployeeEmployeeId == EmployeeId).Select(x => x.MaterialMaterial)
                .Where(x => x.DateOfRegistration >= StartData && x.DateOfRegistration <= FinishData).ToListAsync();
            //await _context.Materials.Where(x => x.ExecutedOrNotExecuted != true).ToListAsync();
        }


        [Route("GetMaterialsByFilterDates/")]
        public async Task<ActionResult<IEnumerable<Materials>>> GetMaterialsByFilterDates([FromBody]string stringJson)
        {
            string[] jsonStringArray = stringJson.Split(new char[] { '|' });
            string StartDataStringJson = jsonStringArray[0];
            string FinishDataStringJson = jsonStringArray[1];
            
            DateTime StartData = JsonConvert.DeserializeObject<DateTime>(StartDataStringJson);
            DateTime FinishData = JsonConvert.DeserializeObject<DateTime>(FinishDataStringJson);
            
            return await _context.Materials.Where(x => x.DateOfRegistration >= StartData && x.DateOfRegistration <= FinishData).ToListAsync();
            //await _context.MaterialEmployees.Include(x => x.MaterialMaterial).Where(x => x.EmployeeEmployeeId == EmployeeId).Select(x => x.MaterialMaterial)
            //.Where(x => x.DateOfRegistration >= StartData && x.DateOfRegistration <= FinishData && x.Decision == SelectedDecision).ToListAsync();

        }


        // GET: api/AllMaterials/5
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

        // PUT: api/AllMaterials/5
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

        // POST: api/AllMaterials
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Materials>> PostMaterials(Materials materials)
        {
            _context.Materials.Add(materials);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterials", new { id = materials.MaterialId }, materials);
        }

        // DELETE: api/AllMaterials/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Materials>> DeleteMaterials(int id)
        {
            var materials = await _context.Materials.FindAsync(id);
            if (materials == null)
            {
                return NotFound();
            }

            _context.Materials.Remove(materials);
            await _context.SaveChangesAsync();

            return materials;
        }

        private bool MaterialsExists(int id)
        {
            return _context.Materials.Any(e => e.MaterialId == id);
        }
    }
}

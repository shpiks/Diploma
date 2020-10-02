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
    public class VictimsController : ControllerBase
    {
        private readonly DB_for_CourseContext _context;

        public VictimsController(DB_for_CourseContext context)
        {
            _context = context;
        }

        // GET: api/Victims
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Victims>>> GetVictims()
        {
            return await _context.Victims.ToListAsync();
        }

        // GET: api/Victims/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Victims>>> GetVictims(int id)
        {

            var victims = await _context.VictimMaterials.Include(x => x.VictimVictim).Where(x => x.MaterialMaterialId == id).Select(x => x.VictimVictim).ToListAsync();



            if (victims == null)
            {
                return NotFound();
            }

            return victims;



            //var victims = await _context.Victims.FindAsync(id);

            //if (victims == null)
            //{
            //    return NotFound();
            //}

            //return victims;
        }

        // PUT: api/Victims/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVictims(int id, Victims victims)
        {
            if (id != victims.VictimId)
            {
                return BadRequest();
            }

            _context.Entry(victims).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VictimsExists(id))
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

        // POST: api/Victims
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostVictims([FromBody]string stringJson)
        {
            string[] jsonStringArray = stringJson.Split(new char[] { '|' });
            string materialStringJson = jsonStringArray[0];
            string victimStringJson = jsonStringArray[1];
            Materials materials = JsonConvert.DeserializeObject<Materials>(materialStringJson);
            Victims victims = JsonConvert.DeserializeObject<Victims>(victimStringJson);

            _context.Victims.Add(victims);

            await _context.SaveChangesAsync();
            var victimTerm = _context.Victims.Find(victims.VictimId);
            var materialTerm = _context.Materials.Find(materials.MaterialId);
            //materialTerm.Victims.Add(victimTerm);
            _context.VictimMaterials.Add(new VictimMaterials {  VictimVictim = victimTerm, MaterialMaterial = materialTerm });
            await _context.SaveChangesAsync();

            return NoContent();

        }

        // DELETE: api/Victims/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Victims>> DeleteVictims(int id)
        {
            
            
            
            var victims = await _context.Victims.FindAsync(id);
            if (victims == null)
            {
                return NotFound();
            }

            _context.Victims.Remove(victims);
            await _context.SaveChangesAsync();

            return victims;
        }

        private bool VictimsExists(int id)
        {
            return _context.Victims.Any(e => e.VictimId == id);
        }
    }
}

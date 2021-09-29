using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObj = _context.CelestialObjects.Find(id);

            if (celestialObj == null)
            {
                return NotFound();
            }

            celestialObj.Satellites = _context.CelestialObjects.Where(obj => obj.OrbitedObjectId == celestialObj.Id).ToList();
            return Ok(celestialObj);

        }

        // TODO: pass test
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = (from c in _context.CelestialObjects
                                    where c.Name == name
                                    select c).ToList();
            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }
            foreach (var obj in celestialObjects)

            {
                obj.Satellites = _context.CelestialObjects.Where(other => other.OrbitedObjectId == obj.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var obj in celestialObjects)
            {
                obj.Satellites = _context.CelestialObjects.Where(other => other.OrbitedObjectId == obj.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject newCelestialObject)
        {
            _context.Add(newCelestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = newCelestialObject.Id }, newCelestialObject);
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var updatedObj = _context.CelestialObjects.SingleOrDefault(x => x.Id == id);
            if (updatedObj == null)
            {
                return NotFound();
            }
            updatedObj.Name = name;
            _context.Update(updatedObj);
            _context.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deletedObj = _context.CelestialObjects.Find(id);
            if (deletedObj == null)
            {
                return NotFound();
            }
            _context.CelestialObjects.Remove(deletedObj);
            _context.SaveChanges();
            return NoContent();

        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var updatedObj = _context.CelestialObjects.SingleOrDefault(x => x.Id == id);
            if (updatedObj == null)
            {
                return NotFound();
            }
            updatedObj.Name = celestialObject.Name;
            updatedObj.OrbitalPeriod = celestialObject.OrbitalPeriod;
            updatedObj.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.SaveChanges();
            return NoContent();

        }

    }
}

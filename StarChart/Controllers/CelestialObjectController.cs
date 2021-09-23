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

        // TODO: pass test
        [HttpGet("{id:int}", Name="GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObj = _context.CelestialObjects.Find(id);
            if (celestialObj == null)
            {
                return NotFound();
            }
            // var orbited = from c in _context.CelestialObjects
            //               where c.OrbitedObjectId == celestialObj.Id
            //               select c;
            foreach (var obj in _context.CelestialObjects.ToList())
            {
                if(obj.Id == celestialObj.OrbitedObjectId)
                    obj.Satellites.Add(celestialObj);
            }
            return Ok(celestialObj);

        }

        // TODO: pass test
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObj = _context.CelestialObjects.FirstOrDefault(o => o.Name == name);
            if (celestialObj == null)
            {
                return NotFound();
            }

            return Ok(celestialObj);
        }

        // TODO: pass test
        [HttpGet]
        public IActionResult GetAll()
        {

            var celestialObj = _context.CelestialObjects.ToList();

            foreach(var obj in celestialObj)
            {
                if(obj.Id == obj.OrbitedObjectId)
                {
                    obj.Satellites.Add(obj);
                }
            }
            return Ok(celestialObj);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject newCelestialObject)
        {
            _context.Add(newCelestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", newCelestialObject);
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

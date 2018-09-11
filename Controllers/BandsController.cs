using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using react_dot_app.Db;
using react_dot_app.Models;

namespace react_dot_app.Controllers
{
    [Route("api/[controller]")]
    public class BandsController: ControllerBase
    {

        //* /api/bands */
        [HttpGet]
        public IEnumerable<Band> GetAll()
        {
            return Neo4jDb.GetAllBands();  
        }


        //* /api/bands/{name} */
        [HttpGet("{name}", Name = "GetBandByName")]
        public IEnumerable<Band> GetBandByName(string name)
        {
            return Neo4jDb.GetBandsByName(name);
        }

        //* /api/bands/rating/1-9 */
        [HttpGet("[action]/{rating}")]
        public IEnumerable<Band> Rating(int rating)
        {
            return Neo4jDb.GetBandsByRating(rating);
        }

        //* /api/bands/genre/{genre} */
        [HttpGet("[action]/{genre}")]
        public IEnumerable<Band> Genre(string genre)
        {
            return Neo4jDb.GetBandsByGenre(genre);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Band))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync([FromBody] Band band)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Neo4jDb.AddBandAsync(band);

            return CreatedAtAction(nameof(GetBandByName), new { name = band.Name }, band);
        }
    
    } 
}
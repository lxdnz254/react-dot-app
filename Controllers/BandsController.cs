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
        public IEnumerable<Band> GetAll(
            [FromQuery] string name = "",
            [FromQuery] string genre = "",
            [FromQuery] int rating = -1,
            [FromQuery] int cost = -1
        )
        {
            if (name != "") return GetBandByName(name);
            if (genre != "") return Genre(genre);
            if (rating > -1) return Rating(rating);
            if (cost > -1) return Cost(cost);
        
            return Neo4jBand.GetAllBands(); 
        }

        //* /api/bands/{name} */
        [HttpGet("{name}", Name = "GetBandByName")]
        public IEnumerable<Band> GetBandByName(string name)
        {
            return Neo4jBand.GetBandsByName(name);
        }

        //* /api/bands/rating/1-9 */
        [HttpGet("[action]/{rating}")]
        public IEnumerable<Band> Rating(int rating)
        {
            return Neo4jBand.GetBandsByRating(rating);
        }

        //* /api/bands/genre/{genre} */
        [HttpGet("[action]/{genre}")]
        public IEnumerable<Band> Genre(string genre)
        {
            return Neo4jBand.GetBandsByGenre(genre);
        }

        //* /api/bands/cost/{cost} */
        [HttpGet("[action]/{cost}")]
        public IEnumerable<Band> Cost(int cost)
        {
            return Neo4jBand.GetBandsByCost(cost);
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Band))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync(
            [FromBody] BandPost postBand
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Neo4jBand.AddBandAsync(
                postBand.band, postBand.genres, postBand.members);

            return CreatedAtAction(nameof(GetBandByName), 
                new { name = postBand.band.Name }, postBand.band);
        }

        public class BandPost
        {
            public Band band {get; set;}
            public List<Genre> genres {get; set;}
            public List<Member> members {get; set;}
        }
    
    } 
}
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
    public class VenuesController: ControllerBase
    {

        //* /api/venues */
        [HttpGet]
        public IEnumerable<Venue> GetAll(
            [FromQuery] string name = "",
            [FromQuery] string genre = "",
            [FromQuery] int capacity = -1,
            [FromQuery] string location = ""
        )
        {
            if (name != "") return GetVenueByName(name);
            if (genre != "") return Genre(genre);
            if (capacity > -1) return Capacity(capacity);
            if (location != "") return Location(location);
        
            return Neo4jVenue.GetAllVenues(); 
        }

        //* /api/venues/{name} */
        [HttpGet("{name}", Name = "GetVenueByName")]
        public IEnumerable<Venue> GetVenueByName(string name)
        {
            return Neo4jVenue.GetVenuesByName(name);
        }

        //* /api/venues/capacity/+1.. */
        [HttpGet("[action]/{capacity}")]
        public IEnumerable<Venue> Capacity(int capacity)
        {
            return Neo4jVenue.GetVenuesByCapacity(capacity);
        }

        //* /api/venues/genre/{genre} */
        [HttpGet("[action]/{genre}")]
        public IEnumerable<Venue> Genre(string genre)
        {
            return Neo4jVenue.GetVenuesByGenre(genre);
        }

        //* /api/venues/location/{location} */
        [HttpGet("[action]/{location}")]
        public IEnumerable<Venue> Location(string location)
        {
            return Neo4jVenue.GetVenuesByLocation(location);
        }


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Venue))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAsync(
            [FromBody] VenuePost postVenue
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Neo4jVenue.AddVenueAsync(
                postVenue.venue, postVenue.genres);

            return CreatedAtAction(nameof(GetVenueByName), 
                new { name = postVenue.venue.Name }, postVenue.venue);
        }

        public class VenuePost
        {
            public Venue venue {get; set;}
            public List<Genre> genres {get; set;}
        }
    
    } 
}
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
    public class GenresController: ControllerBase
    {
        //* /api/genres */ 
        [HttpGet]
        public IEnumerable<Genre> GetAll(
            [FromQuery] string band = "",
            [FromQuery] string venue = ""
        )
        {
            if (band != "") return GetGenresByBand(band);
            if (venue != "") return GetGenresByVenue(venue);
            
            return Neo4jGenre.GetAllGenres();
        }

        public IEnumerable<Genre> GetGenresByBand(string band)
        {
            return Neo4jGenre.GetGenresByBand(band);
        }

        public IEnumerable<Genre> GetGenresByVenue(string venue)
        {
            return Neo4jGenre.GetGenresByVenue(venue);
        }
    }
}
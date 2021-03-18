using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyStore.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private readonly IProductRepo _repo;
        
        public SearchController(IProductRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{searchString}", Name = "SearchProducts")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public ActionResult<IList<Product>> Search(string searchString)
        {
            var results = _repo.Search(searchString).ToList();
            if (results.Count == 0)
            {
                return NoContent();
            }
            return results;
        }

    }
}

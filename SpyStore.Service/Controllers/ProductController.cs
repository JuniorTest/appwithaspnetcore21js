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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _repo;

        public ProductController(IProductRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetProduct")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Product> Get(int id)
        {
            Product item = _repo.GetOneWithCategoryName(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet("featured", Name = "GetFeaturedProducts")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<Product>> GetFeatured()
            => Ok(_repo.GetFeaturedWithCategoryName().ToList());

    }
}

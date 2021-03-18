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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repo;

        public OrdersController(IOrderRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{customerId}", Name = "GetOrderHistory")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrderHistory(int customerId)
        {
            _repo.Context.CustomerId = customerId;
            IList<Order> orderWithTotals = _repo.GetOrderHistory();
            return orderWithTotals == null ? (IActionResult)NotFound() : new ObjectResult(orderWithTotals);
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpyStore.Dal.Repos.Interfaces;
using SpyStore.Models.Entities;
using SpyStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyStore.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepo _repo;
        
        public ShoppingCartController(IShoppingCartRepo repo)
        {
            _repo = repo;
        }

        [HttpGet(Name = "GetShoppingCart")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public ActionResult<CartWithCustomerInfo> GetShoppingCart(int customerId)
            => _repo.GetShoppingCartRecordsWithCustomer(customerId);

        [HttpPost("buy", Name = "Purchase")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public IActionResult Purchase(int customerId, Customer customer)
        {
            if (customer == null || customer.Id != customerId || !ModelState.IsValid)
            {
                return BadRequest();
            }

            int orderId;
            orderId = _repo.Purchase(customerId);
            return CreatedAtRoute("GetOrderDetails", routeValues: new { orderId = orderId }, null);
        }

        }
}

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
    public class ShoppingCartRecordController : ControllerBase
    {
        private readonly IShoppingCartRepo _repo;

        public ShoppingCartRecordController(IShoppingCartRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{recordId}", Name = "GetShoppingCartRecord")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<CartRecordWithProductInfo> GetShoppingCartRecord(int recordId)
        {
            CartRecordWithProductInfo cartRecordWithProductInfo = _repo.GetShoppingCartRecord(recordId);
            return cartRecordWithProductInfo ?? (ActionResult<CartRecordWithProductInfo>)NotFound();
        }

        [HttpPost("{customerId}", Name = "AddCartRecord")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult AddShoppingCartRecord(int customerId, ShoppingCartRecord record)
        {
            if (record == null || customerId != record.CustomerId
            || !ModelState.IsValid)
            {
                return BadRequest();
            }
            record.DateCreated = DateTime.Now;
            record.CustomerId = customerId;
            _repo.Context.CustomerId = customerId;
            _repo.Add(record);
            CreatedAtRouteResult createdAtRouteResult = CreatedAtRoute("GetShoppingCart", new { controller = "ShoppingCart", customerId = customerId }, null);
            return createdAtRouteResult;
        }

        [HttpPut("{recordId}", Name = "UpdateCartRecord")]
        public ActionResult UpdateShoppingCartRecord(int recordId, ShoppingCartRecord item)
        {
            if (item == null || item.Id != recordId || !ModelState.IsValid)
            {
                return BadRequest();
            }
            item.DateCreated = DateTime.Now;
            _repo.Context.CustomerId = item.CustomerId;
            _repo.Update(item);
            return CreatedAtRoute("GetShoppingCartRecord", new { controller = "ShoppingCartRecord", recordId = item.Id }, null);
        }

        [HttpDelete("{recordId}", Name = "DeleteCartRecord")]
        //HTTP 1.1 spec allows for body in delete statement
        public IActionResult DeleteCartRecord(int recordId, ShoppingCartRecord item)
        {
            if (recordId != item.Id)
            {
                return NotFound();
            }
            _repo.Context.CustomerId = item.CustomerId;
            _repo.Delete(item);
            return NoContent();
        }

    }
}

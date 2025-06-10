using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CartController(ICartServices cartServices) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {

            var cart = await cartServices.GetCartAsync(id);

            return Ok(cart ?? new ShoppingCart { Id = id });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart = await cartServices.SetCartAsync(cart);
            if (updatedCart == null)
            {
                return BadRequest("Failed to update the cart.");
            }
            return Ok(updatedCart);
        }

        [HttpDelete]
        public async Task<ActionResult<ShoppingCart>> DeleteCart(string id)
        {
            var deleted = await cartServices.DeleteCartAsync(id);
            if (!deleted)
            {
                return NotFound("Cart not found.");
            }
            return Ok();
        }
    }
}
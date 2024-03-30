using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDb.API.Models;
using MongoDb.API.Repositories;
using System.Net;

namespace MongoDb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService) =>
            this.productService = productService;

        [HttpGet]
        public async Task<List<ProductDetails>> Get()
        {
            var response = await productService.ProductListAsync();

            if (!response.IsSuccessful)
            {
                return new List<ProductDetails>();
            }

            return response.Result!;
        }

        [HttpGet("{productId:length(24)}")]
        public async Task<ProductDetails> Get(string productId)
        {
            var response = await productService.GetProductDetailByIdAsync(productId);

            if (!response.IsSuccessful)
            {
                return new ProductDetails() { };
            }

            return response.Result!;
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductDetails productDetails)
        {
            var response = await productService.AddProductAsync(productDetails);

            if (!response.IsSuccessful)
            {
                return BadRequest();
            }

            CreatedAtAction(nameof(Get), new { id = productDetails.Id }, productDetails);

            return Ok();
        }

        [HttpPut("{productId:length(24)}")]
        public async Task<IActionResult> Update(string productId, ProductDetails productDetails)
        {
            var productDetail = await productService.GetProductDetailByIdAsync(productId);

            if (!productDetail.IsSuccessful)
            {
                return NotFound();
            }

            productDetails.Id = productDetail.Result!.Id;

            var response = await productService.UpdateProductAsync(productId, productDetails);

            if (!response.IsSuccessful)
            {
               return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{productId:length(24)}")]
        public async Task<IActionResult> Delete(string productId)
        {
            var productDetails = await productService.GetProductDetailByIdAsync(productId);

            if (productDetails is null)
            {
                return NotFound();
            }

            var response = await productService.DeleteProductAsync(productId);

            if (!response.IsSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}

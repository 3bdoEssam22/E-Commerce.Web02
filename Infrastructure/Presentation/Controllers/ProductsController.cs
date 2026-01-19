using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTtansferObjects;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController(IServiceManager _serviceManager) : ControllerBase
    {
        //Get All Products
        // Get BaseUrl/api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var Products = await _serviceManager.ProductService.GetAllProductsAsync();
            return Ok(Products);
        }

        //Get Product By Id
        // Get BaseUrl/api/Products/10
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var Product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Product);
        }

        //Get All Brands
        // Get BaseUrl/api/Products/brands
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

        //Get All Products
        // Get BaseUrl/api/Products/types
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);
        }


    }
}

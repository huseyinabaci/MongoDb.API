using MongoDb.API.DTOs;
using MongoDb.API.Models;

namespace MongoDb.API.Repositories
{
    public interface IProductService
    {
        public Task<ResponseDto<List<ProductDetails>>> ProductListAsync();

        public Task<ResponseDto<ProductDetails>> GetProductDetailByIdAsync(string productId);

        public Task<ResponseDto<ProductDetails>> AddProductAsync(ProductDetails productDetails);

        public Task<ResponseDto<ProductDetails>> UpdateProductAsync(string productId, ProductDetails productDetails);

        public Task<ResponseDto<ProductDetails>> DeleteProductAsync(String productId);
    }
}

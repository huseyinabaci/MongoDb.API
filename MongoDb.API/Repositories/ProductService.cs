using Microsoft.Extensions.Options;
using MongoDb.API.Configurations;
using MongoDb.API.DTOs;
using MongoDb.API.Models;
using MongoDB.Driver;
using System.Net;

namespace MongoDb.API.Repositories
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<ProductDetails> productCollection;

        public ProductService(IOptions<ProductDBSettings> productDatabaseSetting)
        {
            var mongoClient = new MongoClient(productDatabaseSetting.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(productDatabaseSetting.Value.DatabaseName);

            productCollection = mongoDatabase.GetCollection<ProductDetails>(productDatabaseSetting.Value.ProductCollectionName);

        }

        public async Task<ResponseDto<List<ProductDetails>>> ProductListAsync()
        {
            var response = await productCollection.Find(_ => true).ToListAsync();

            if (response is null)
            {
                return ResponseDto<List<ProductDetails>>.Fail("Ürünler Bulunamadı", HttpStatusCode.NoContent);
            }

            return ResponseDto<List<ProductDetails>>.Success(response, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDetails>> GetProductDetailByIdAsync(string productId)
        {
            var response = await productCollection.Find(x => x.Id == productId).FirstOrDefaultAsync();

            if (response is null)
            {

                return ResponseDto<ProductDetails>.Fail("Ürün Bulunamadı", HttpStatusCode.NoContent);
            }

            return ResponseDto<ProductDetails>.Success(response, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDetails>> AddProductAsync(ProductDetails productDetails)
        {
            try
            {
                await productCollection.InsertOneAsync(productDetails);
                return ResponseDto<ProductDetails>.Success(new ProductDetails { }, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {

                return ResponseDto<ProductDetails>.Fail(ex.Message + "/ Ürün eklenemedi.", HttpStatusCode.BadRequest);
            }

        }

        public async Task<ResponseDto<ProductDetails>> UpdateProductAsync(string productId, ProductDetails productDetails)
        {

            try
            {
                await productCollection.ReplaceOneAsync(x => x.Id == productId, productDetails);
                return ResponseDto<ProductDetails>.Success(new ProductDetails { }, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {

                return ResponseDto<ProductDetails>.Fail(ex.Message + "/ Ürün güncellenemedi.", HttpStatusCode.BadRequest);
            }
        }

        public async Task<ResponseDto<ProductDetails>> DeleteProductAsync(string productId)
        {

            try
            {
                await productCollection.DeleteOneAsync(x => x.Id == productId);
                return ResponseDto<ProductDetails>.Success(new ProductDetails { }, HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {

                return ResponseDto<ProductDetails>.Fail(ex.Message + "/ Ürün silinemedi.", HttpStatusCode.BadRequest);
            }


        }
    }
}

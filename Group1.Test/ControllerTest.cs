using ProductsService.Tests;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Products.Service.Tests
{
    public class ControllerTest
    {
        [Fact]
        public async Task GetAllProducts_Succeed()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/product/getall");

                response.EnsureSuccessStatusCode();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }
        [Fact]
        public async Task GetProductById_Returns_NotFound()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync($"/api/product/getone");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
        [Fact]
        public async Task GetProductById_Returns_ProductId()
        {
            using (var client = new TestClientProvider().Client)
            {
                var productsResponse = await client.GetAsync($"/api/product/getone?id=1");

                using (var responseStream = await productsResponse.Content.ReadAsStreamAsync())
                {
                    var product = await System.Text.Json.JsonSerializer.DeserializeAsync<Product.Service.Models.Product>(responseStream,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    Assert.Equal(1, product.Id);
                }
            }
        }
        [Fact]
        public async Task CreateProduct_Returns_CreatedProductId()
        {
            using (var client = new TestClientProvider().Client)
            {

                var payload = JsonSerializer.Serialize(new Product.Service.Models.Product()

                {
                    Name = "NyProdukt",
                    Quantity = 1,
                    Price = 15,
                    ImageUrl = "https://henryfuentes.com/wp-content/uploads/2015/06/New-Products-1.png",
                    Description = "Detta är en ny produkt"
                }
                    );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/api/product/createproduct", content);

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var product = await JsonSerializer.DeserializeAsync<Product.Service.Models.Product>(responseStream,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    Assert.NotNull(product);
                    Assert.NotEqual(0, product.Id);
                }
            }
        }
        [Fact]
        public async Task DeleteProduct_Returns_Notfound()
        {
            using (var client = new TestClientProvider().Client)
            {
                var payload = JsonSerializer.Serialize(new Product.Service.Models.Product());

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.DeleteAsync($"/api/product/");

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
        [Fact]
        public async Task DeleteProduct_Returns_DeletedProductId()
        {
            using (var client = new TestClientProvider().Client)
            {
                //Create product
                var product = JsonSerializer.Serialize(new Product.Service.Models.Product()
                {
                    Name = "TabortProdukt",
                    Quantity = 10,
                    Price = 100,
                    ImageUrl = "https://www.stadshop.se/image/8171/648139.jpg",
                    Description = ""
                });

                HttpContent content = new StringContent(product, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/api/product/createproduct", content);
                Product.Service.Models.Product newProduct = null;

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    newProduct = await JsonSerializer.DeserializeAsync<Product.Service.Models.Product>(responseStream,
                                 new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                }

                //Delete product 
                var deleteresponse = await client.DeleteAsync($"/api/product/DeleteProduct?productId={newProduct.Id}");
                using (var responseStream = await deleteresponse.Content.ReadAsStreamAsync())
                {
                    var deletedProduct = await JsonSerializer.DeserializeAsync<Product.Service.Models.Product>(responseStream,
                       new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    Assert.Equal(newProduct.Id, deletedProduct.Id);
                }




            }
        }

        [Fact]
        public async Task UpdateProduct_Returns_OK()
        {
            using (var client = new TestClientProvider().Client)
            {
                var product1 = new Product.Service.Models.Product() { Id = 1, Price = 50, Quantity = 10 };
                var product2 = new Product.Service.Models.Product() { Id = 2, Price = 75, Quantity = 15 };

                var cartItems = new List<Product.Service.Models.CartItem>()
                { new Product.Service.Models.CartItem(){Product=product1,Quantity=1},
                  new Product.Service.Models.CartItem(){Product=product2,Quantity=2}
                };

                var payload = JsonSerializer.Serialize(new Product.Service.Models.ShoppingCart()
                {
                    cartItems = cartItems
                }
                );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/product/UpdateQuantity", content);

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {

                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
            }
        }
    }
}



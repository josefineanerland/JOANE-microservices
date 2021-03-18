﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Order.Service.Test
{
    public class ControllerTests
    {
        [Fact]
        public async Task CreateOrder_Returns_CreatedOrder()
        {
            int createdorderId = 0;
            using (var client = new TestClientProvider().Client)
            {
                var product1 = new Models.Product() { Id = 1, Price = 50, Quantity = 10 };
                var product2 = new Models.Product() { Id = 2, Price = 75, Quantity = 15 };

                var cartItems = new List<Models.CartItem>()
                { new Models.CartItem(){Product=product1,Quantity=1},
                  new Models.CartItem(){Product=product2,Quantity=2}
                };

                var payload = JsonSerializer.Serialize(new Models.Cart()
                {
                    UserId = Guid.NewGuid(),
                    PaymentId = 1,
                    DeliveryId = 1,
                    CartItems = cartItems
                }
                );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/api/order/CreateOrder", content);

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var order = await JsonSerializer.DeserializeAsync<Models.Order>(responseStream,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    createdorderId = order.Id;
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
            }
        }

        [Fact]
        public async Task CreateOrder_Returns_BadRequest()
        {
            using (var client = new TestClientProvider().Client)
            {
                var payload = JsonSerializer.Serialize(new Models.Cart()
                { }
                );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/api/order/createorder", content);

                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetAllOrders_Succeed()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/order/getallorders");

                response.EnsureSuccessStatusCode();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }


        [Fact]
        public async Task UpdateOrder_Returns_BadRequest()
        {
            using (var client = new TestClientProvider().Client)
            {
                int orderId = 1;
                bool deliverStatus = true;

                var payload = JsonSerializer.Serialize(new Models.Order()
                {
                    Id = orderId,
                    Deliverd = deliverStatus
                }
                );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/order/UpdateOrderDeliveryStatus?id={orderId + 1}", content);

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var order = await JsonSerializer.DeserializeAsync<Models.Order>(responseStream,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                }
            }
        }

        [Fact]
        public async Task UpdateOrder_Returns_NotFound()
        {
            using (var client = new TestClientProvider().Client)
            {
                int orderId = 500;
                bool deliverStatus = true;

                var payload = JsonSerializer.Serialize(new Models.Order()
                {
                    Id = orderId,
                    Deliverd = deliverStatus
                }
                );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/order/UpdateOrderDeliveryStatus?id={orderId}", content);

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var order = await JsonSerializer.DeserializeAsync<Models.Order>(responseStream,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                }
            }
        }

        [Fact]
        public async Task DeleteOrder_Returns_Deleted_Id()
        {
            using (var client = new TestClientProvider().Client)
            {
                var product1 = new Models.Product() { Id = 1, Price = 50, Quantity = 10 };
                var product2 = new Models.Product() { Id = 2, Price = 75, Quantity = 15 };

                var cartItems = new List<Models.CartItem>()
                {
                   new Models.CartItem(){Product=product1,Quantity=1},
                   new Models.CartItem(){Product=product2,Quantity=2}
                };

                var payload = JsonSerializer.Serialize(new Models.Cart()
                {
                    UserId = Guid.NewGuid(),
                    PaymentId = 2,
                    DeliveryId = 2,
                    CartItems = cartItems
                }
                );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/api/order/createorder", content);

                Models.Order order = null;

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    order = await JsonSerializer.DeserializeAsync<Models.Order>(responseStream,
                          new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                }
                var deleteResponse = await client.DeleteAsync($"/api/order/deleteorder?orderId={order.Id}");
                using (var responseStream = await deleteResponse.Content.ReadAsStreamAsync())
                {
                    var deletedId = await JsonSerializer.DeserializeAsync<int>(responseStream,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    Assert.Equal(order.Id, deletedId);
                }
            }

        }

        [Fact]
        public async Task DeleteOrder_Returns_Notfound()
        {
            using (var client = new TestClientProvider().Client)
            {
                var payload = JsonSerializer.Serialize(new Models.Cart());

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/api/order/createorder", content);

                Models.Order order = null;

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    order = await JsonSerializer.DeserializeAsync<Models.Order>(responseStream,
                          new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                }
                var deleteResponse = await client.DeleteAsync($"/api/order/deleteorder?orderId={order.Id}");

                Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);


            }
        }
        [Fact]
        public async Task UpdateOrder_Returns_Ok()
        {
            using (var client = new TestClientProvider().Client)
            {
                var product1 = new Models.Product() { Id = 1, Price = 50, Quantity = 10 };
                var product2 = new Models.Product() { Id = 2, Price = 75, Quantity = 15 };

                var cartItems = new List<Models.CartItem>()
                {
                   new Models.CartItem(){Product=product1,Quantity=1},
                   new Models.CartItem(){Product=product2,Quantity=2}
                };

                var payload = JsonSerializer.Serialize(new Models.Cart()
                {
                    UserId = Guid.NewGuid(),
                    PaymentId = 2,
                    DeliveryId = 2,
                    CartItems = cartItems
                }
                );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/api/order/createorder", content);
            }
            using (var client = new TestClientProvider().Client)
            {
                int updatedOrderId = 0;
                int orderId = 1;
                bool deliverStatus = true;

                var payload = JsonSerializer.Serialize(new Models.Order()
                {
                    Id = orderId,
                    Deliverd = deliverStatus
                }
                );

                HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/order/UpdateOrderDeliveryStatus?id={orderId}", content);

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var order = await JsonSerializer.DeserializeAsync<Models.Order>(responseStream,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                    updatedOrderId = order.Id;
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
            }
        }

    }
}

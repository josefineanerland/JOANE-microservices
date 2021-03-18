﻿using Group1.Web.Models;
using Group1.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Group1.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly string _cartName;
        private readonly OrderServiceHandler _orderService;
        private readonly ProductServiceHandler _productService;
        private readonly UserManager<CustomUser> _userManager;
        private readonly string _orderServiceRootUrl;
        private readonly string _productServiceRootUrl;
        public OrderController(OrderServiceHandler orderServiceHandler, IConfiguration config, UserManager<CustomUser> userManager, ProductServiceHandler productServiceHandler)
        {
            _orderService = orderServiceHandler;
            _productService = productServiceHandler;
            this._userManager = userManager;
            _orderServiceRootUrl = config["OrderServiceURL"];
            this._cartName = config["CartSessionCookie:Name"];
            _productServiceRootUrl = config["ProductServiceURL"];

        }

        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder([Bind("TotalPrice", "cartItems")] ShoppingCart cart, IFormCollection form)
        {
            var user = await _userManager.GetUserAsync(User);

            var order = new Order()
            {
                CartItems = cart.cartItems,
                UserId = Guid.Parse(user.Id),
                TotalPrice = cart.TotalPrice,
                Deliverd = false,
                DeliveryId = int.Parse(form["deliveryMethod"]),
                PaymentId = int.Parse(form["Paymentmethod"]),
                Address = user.Street + user.City + user.PostalCode
            };

            var createdorder = await _orderService.PostAsync(order, $"{_orderServiceRootUrl}/api/order/createorder");
            var isQuantityUpdated = await _productService.UpdateProductQuantity(cart, $"{_productServiceRootUrl}/api/product/Updatequantity");

            order.User = user;
            order.Id = createdorder.Id;

            //Clear the session cookies once the order is created
            if (HttpContext.Session.GetString(_cartName) != null)
                HttpContext.Session.Remove(_cartName);
            return View(order);

        }

        [Authorize]
        public async Task<ActionResult<List<Order>>> GetAllOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            List<Models.Order> allorders = await _orderService.GetAllAsync<Models.Order>($"{_orderServiceRootUrl}/api/order/getallorders");

            return View(allorders);
        }

        [Authorize]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            await _orderService.DeleteOneAsync<Models.Order>($"{_orderServiceRootUrl}/api/order/DeleteOrder?orderId=" + orderId);
            return RedirectToAction("GetAllOrder", "Order");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UpdateDeliveryStatus(int orderId)
        {

            var order = await _orderService.GetOneAsync<Models.Order>($"{_orderServiceRootUrl}/api/order/Getone?id={orderId}");
            order.Deliverd = !order.Deliverd;

            await _orderService.UpdateDeliveryStatus(order, $"{_orderServiceRootUrl}/api/order/UpdateOrderDeliveryStatus?id={orderId}");

            return View();
        }


    }

}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FBPlusOneBuy.Repositories;
using FBPlusOneBuy.Services;
using Newtonsoft.Json;
using FBPlusOneBuy.Models;
using FBPlusOneBuy.ViewModels;

namespace FBPlusOneBuy.Controllers
{
    public class listController : Controller
    {
        [HttpGet]
        // GET: list
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string livePageID, string liveName, string keywordPattern)
        {
            //新增直播進資料庫
            LivePostService.CreateLivePost(livePageID, liveName);
            //新增商品進資料庫
            ProductRepositories product_repo = new ProductRepositories();
            var products = ProductService.GetCurrentProducts().ProductItems;
            foreach (var item in products)
            {
                if (!product_repo.SelectProduct(item.SkuId))
                {
                    product_repo.InsertProduct(item);
                }
            }
            ViewData["products"] = products;
            ViewData["livePageID"] = livePageID;
            ViewData["keywordPattern"] = keywordPattern;
            return View();
        }

        [HttpPost]
        public ActionResult GetPlusOneBuyOrders(string livePageID, string keywordPattern)
        {
            string token = Session["token"].ToString();
            //string token =
            //    "EAASxbKYYpHoBAI27CZBoK8ZBzFmJjEMIR30woKcIfDPx4mtljSUOsGxVGsKHmy1JgCay8KTilT9l3nbkSfGzBZC6wVSDUcl3ZAa7C5OyZAv8CV7K0duuyW2jHFGqZCwhIKiM6jPonrHLp7s5UEudWL5UHkT8IuZBGmBTOEHS0IjYZCsYbcQfo3j9";
            var products = ProductService.GetCurrentProducts().ProductItems;
            var OrderList = CommentFilterService.getNewOrderList(livePageID, token, products, keywordPattern);
            if (OrderList.Count > 0)
            {
                FBSendMsgService.OrderListToSendMsg(OrderList, token);
            }
            var result = JsonConvert.SerializeObject(OrderList);
            return Json(result);
        }
        [HttpPost]
        public void SetPostEndtime(string livePageID, int QtyOfOrders, decimal Amount)
        {
            var live_repo = new LivePostsRepository();
            int liveid = live_repo.Select(livePageID);
            live_repo.UpdatePost(liveid, QtyOfOrders, Amount, DateTime.Now);
        }

        [HttpPost]
        public void SendMsgByButton(string livePageID)
        {
            var live_repo = new LivePostsRepository();
            int liveid = live_repo.Select(livePageID);
            var order_repo = new OrderRepositories();
            //List<string> ids = new List<string> { "3032519476788720", "2762673820474754" };
            List<MsgTextViewModel> ordersinfo = order_repo.SelectAllOrdersInfo(liveid);
            string token = (string)Session["token"];
            var orders = new List<OrderList>();
            foreach (var orderinfo in ordersinfo)
            {
                var order = new OrderList();
                order.CustomerID = orderinfo.CustomerID;
                order.CustomerName = orderinfo.CustomerName;
                order.OrderID = orderinfo.OrderID;
                order.Product = new ProductViewModel { Salepage_id = orderinfo.ProductPageID, SkuId = orderinfo.ProductID, ProductName = orderinfo.ProductName };
                order.Quantity = orderinfo.Quantity;
                orders.Add(order);
            }
            FBSendMsgService.OrderListToSendMsg(orders, token);

        }
    }
}
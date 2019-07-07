﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FBPlusOneBuy.Repositories;
using FBPlusOneBuy.Services;
using Newtonsoft.Json;
using FBPlusOneBuy.Models;

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
        public ActionResult Index(string token,string keyWord, string ProductName, string liveID)
        {
            ViewData["token"] = token;
            ViewData["keyWord"] = keyWord;
            ViewData["ProductName"] = ProductName;
            ViewData["liveID"] = liveID;
            ProductRepositories productRepositories = new ProductRepositories();
            var product = new List<Product>();
            product=productRepositories.FindByName(ProductName).ToList();
            ViewData["product"] = product;
            return View();
        }
        //[HttpPost]
        //public ActionResult GetAll(string liveID)
        //{
        //    ViewData["liveID"] = liveID;
        //    OrderRepositories orderRepositories = new OrderRepositories();
        //    var order = orderRepositories.GetAll();
        //    order = order.Where((x) => x.LiveID == liveID);
        //    var result = JsonConvert.SerializeObject(order);
        //    return Json(result);
        //}

        [HttpPost]
        public ActionResult GetPlusOneBuyOrders(string liveID,string token,string keywords)
        {
            var OrderList = CommentFilterService.getNewOrderList(liveID,token,keywords);
            var result = JsonConvert.SerializeObject(OrderList);
            return Json(result);
        }
    }
}
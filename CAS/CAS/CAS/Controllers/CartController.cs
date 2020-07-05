using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CAS.Models;
using Model.EF;
using Model.Dao;
using Common;
using System.Web.Script.Serialization;
using System.Configuration;

namespace CAS.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ActionResult ApplyDiscount(string discountcode)
        {
            var dc = new DiscountCodeDao().GetByID(discountcode);
            if (dc != null)
            {
                if(dc.StartDate <= DateTime.Now && dc.EndDate>= DateTime.Now)
                {
                   Session.Add(CommonConstants.DISCOUNT_SESSION, dc);
                }
                else
                {
                    TempData["errorcode"] = "Mã giảm giá đã hết hạn";
               
                }
            }
            return Redirect("Payment");
        }

        public JsonResult AddToCart(long productID, int quantity)
        {
            bool alreadyhave = false;
            int itemquantity = 0;
            var product = new ProductDao().GetById(productID);
            var cart = Session[CommonConstants.CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productID))
                {
                    alreadyhave = true;
                }
                else
                {
                    // Tạo mới item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    itemquantity = item.Quantity;
                    list.Add(item);
                }
                Session[CommonConstants.CartSession] = list;

            }
            else
            {
                // Tạo mới item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                itemquantity = item.Quantity;
                // Gán session
                Session[CommonConstants.CartSession] = list;
            }
            var itemcount = (List<CartItem>)Session[CommonConstants.CartSession];
            var price = product.Price;
            if (product.PromotionPrice.HasValue)
            {
                price = product.PromotionPrice;
            }
            return Json(new
            {
                count = itemcount.Count(),
                status = true,
                entity = product,
                quantity = itemquantity,
                //promoPrice = product.PromotionPrice.GetValueOrDefault(0).ToString("N0"),
                Price = price.GetValueOrDefault(0).ToString("N0"),
                lastPrice = (price * itemquantity).GetValueOrDefault(0).ToString("N0"),
                alrhave = alreadyhave
            }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Additem(long productID, int quantity)
        //{
        //    var product = new ProductDao().GetById(productID);
        //    var cart = Session[CommonConstants.CartSession];
        //    if (cart != null)
        //    {
        //        var list = (List<CartItem>)cart;
        //        if (list.Exists(x => x.Product.ID == productID))
        //        {
        //            foreach (var item in list)
        //            {
        //                if (item.Product.ID == productID)
        //                {
        //                    item.Quantity += quantity;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // Tạo mới item   
        //            var item = new CartItem();
        //            item.Product = product;
        //            item.Quantity = quantity;
        //            list.Add(item);
        //        }
        //        Session[CommonConstants.CartSession] = list;

        //    }
        //    else
        //    {
        //        // Tạo mới item
        //        var item = new CartItem();
        //        item.Product = product;
        //        item.Quantity = quantity;
        //        var list = new List<CartItem>();
        //        list.Add(item);
        //        // Gán session
        //        Session[CommonConstants.CartSession] = list;
        //    }
        //    return RedirectToAction("Index");
        //}


        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CommonConstants.CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    var product = new ProductDao().GetById(jsonItem.Product.ID);
                    if (product.Quantity >= jsonItem.Quantity)
                    {
                        item.Quantity = jsonItem.Quantity;
                    }
                    else
                    {
                        return Json(new
                        {
                            status = false,
                            productname = product.Name,
                            curquantity = product.Quantity
                        });
                    }
                }
            }
            Session[CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult DeleteAll()
        {
            Session[CommonConstants.CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CommonConstants.CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        [HttpGet]
        public ActionResult Payment()
        {
            if (Session[CommonConstants.USER_SESSION] == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Bạn cần phải đăng nhập!');  window.location.href = '/dang-nhap'</script>");
            }
            else
            {
                var cart = Session[CommonConstants.CartSession];
                var list = new List<CartItem>();
                if (cart != null)
                {
                    list = (List<CartItem>)cart;
                }
                if(TempData["errorcode"]!=null)
                {
                    ViewBag.errorcode = TempData["errorcode"].ToString();
                }
                return View(list);
            }
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email, decimal total_order, string discountcode = "")
        {
            if (Session[CommonConstants.USER_SESSION] == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Bạn cần phải đăng nhập!');  window.location.href = '/dang-nhap'</script>");
            }
            else
            {
                var order = new Order();
                var user = (UserLogin)Session[CommonConstants.USER_SESSION];
                order.CustomerID = user.UserID;
                order.CreateDate = DateTime.Now;
                order.ShipName = shipName;
                order.ShipMobile = mobile;
                order.ShipAddress = address;
                order.ShipEmail = email;
                if(discountcode=="")
                {

                    order.DiscountCodeID = null;
                }
                else
                {
                    order.DiscountCodeID = discountcode;
                }
                order.Total = total_order;

                try
                {
                    var id = new OrderDao().Insert(order);
                    var cart = (List<CartItem>)Session[CommonConstants.CartSession];
                    var detailDao = new OrderDetailDao();
                    //decimal total = 0;

                    foreach (var item in cart)
                    {
                        var orderDetail = new OrderDetail();
                        orderDetail.ProductID = item.Product.ID;
                        orderDetail.OrderID = id;
                        if (item.Product.PromotionPrice.HasValue)
                        {
                            orderDetail.Price = item.Product.PromotionPrice;
                        }
                        else
                        {
                            orderDetail.Price = item.Product.Price;
                        }
                        orderDetail.Quantity = item.Quantity;
                        detailDao.Insert(orderDetail);
                        
                    }
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/WebPage1.cshtml"));
                    content = content.Replace("{{CustomerName}}", shipName);
                    content = content.Replace("{{Phone}}", mobile);
                    content = content.Replace("{{Email}}", email);
                    content = content.Replace("{{Address}}", address);
                    content += "<table border=\"1\" frame=\"hsides\" rules=\"columns\" style=\"text-align:center;\">" +
                        "<tr>" +
                            "<td>Mã_SP</td>" +
                            "<td>Tên SP</td>" +
                            "<td>Số lượng</td>" +
                            "<td>Đơn giá</td>" +
                            "<td>Thành tiền</td>" +
                        "</tr>";


                    foreach (var item in cart)
                    {
                        string realprice = "";
                        string totalprice = "";
                        if (item.Product.PromotionPrice.HasValue)
                        {
                            realprice = item.Product.PromotionPrice.GetValueOrDefault(0).ToString("N0");
                            totalprice = (item.Product.PromotionPrice.GetValueOrDefault(0) * item.Quantity).ToString("N0");
                        }
                        else
                        {
                            realprice = item.Product.Price.GetValueOrDefault(0).ToString("N0");
                            totalprice = (item.Product.Price.GetValueOrDefault(0) * item.Quantity).ToString("N0");
                        }

                        content += "<tr>" +
                            "<td>" + item.Product.ID + "</td>" +
                            "<td>" + item.Product.Name + "</td>" +
                            "<td>" + item.Quantity + "</td>" +
                            "<td>" + realprice + "₫</td>" +
                            "<td>" + totalprice + "₫</td>" +
                            "</tr>";
                    }
                    content += "</table>";
                    content = content.Replace("{{DiscountCode}}", discountcode);
                    content = content.Replace("{{Total}}", total_order.ToString("N0"));

                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    //new MailHelper().SendMail(email, "Đơn hàng mới từ CAS", content);
                    //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ CAS", content);
                }
                catch (Exception)
                {

                    return Redirect("/loi-thanh-toan");
                }
                return Redirect("/hoan-thanh");
            }
        }

        public ActionResult Success()
        {
            Session[CommonConstants.CartSession] = null;
            Session[CommonConstants.DISCOUNT_SESSION] = null;
            return View();
        }

        public ActionResult Fail()
        {
            return View();
        }
    }
}
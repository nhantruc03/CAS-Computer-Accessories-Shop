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
            if(product.PromotionPrice.HasValue)
            {
                price = product.PromotionPrice;
            }
            return Json(new
            {
                count = itemcount.Count(),
                status = true,
                entity = product,
                quantity = itemquantity,
                promoPrice = product.PromotionPrice.GetValueOrDefault(0).ToString("N0"),
                Price = price.GetValueOrDefault(0).ToString("N0"),
                lastPrice = (price * itemquantity).GetValueOrDefault(0).ToString("N0"),
                alrhave = alreadyhave
            }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult UpdateFromCart(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CommonConstants.CartSession];
            if (sessionCart != null)
            {
                foreach (var item in sessionCart)
                {
                    var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                    if (jsonItem != null)
                    {
                        item.Quantity = jsonItem.Quantity;
                    }
                }
                Session[CommonConstants.CartSession] = sessionCart;
            }
            return Json(new
            {
                status = true
            });
        }

        public ActionResult Additem(long productID, int quantity)
        {
            var product = new ProductDao().GetById(productID);
            var cart = Session[CommonConstants.CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    // Tạo mới item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
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
                // Gán session
                Session[CommonConstants.CartSession] = list;
            }
            return RedirectToAction("Index");
        }


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
                            productname= product.Name,
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
                return View(list);
            }
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            if(Session[CommonConstants.USER_SESSION] == null)
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

                try
                {
                    var id = new OrderDao().Insert(order);
                    var cart = (List<CartItem>)Session[CommonConstants.CartSession];
                    var detailDao = new OrderDetailDao();
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
            return View();
        }
    }
}
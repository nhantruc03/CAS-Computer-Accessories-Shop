using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.EF;
namespace CAS.Models
{
    public class CartItem
    {
        public Product Product { set; get; }
        public int Quantity { set; get; }
    }
}
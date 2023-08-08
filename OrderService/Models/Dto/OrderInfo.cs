using OrderService.Models.Enums;
using System;
using System.Collections.Generic;

namespace OrderService.Models.Dto
{
    public class OrderInfo
    {
        public Guid OrderIdentifier { get; set; }

        public OrderStates OrderState { get; set; }

        public List<ProductInfo> Products { get; set; }

        public bool ManagerApproval { get; set; }

        public bool UserApproval { get; set; }
    }
}

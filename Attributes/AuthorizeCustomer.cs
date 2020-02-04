using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NWBA.Models;


namespace NWBA.Attributes
{
    public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var customerID = context.HttpContext.Session.GetInt32(nameof(Customer.CustomerID));
            if(!customerID.HasValue)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}

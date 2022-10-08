using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVPortal.App_Code
{
    public enum EmailTypeEnum
    {
        OTPVendor = 1,
        OTPCustomer = 2,
        OTPUser = 3,
        User = 4,
        Vendor = 5,
        Customer = 6,
        ForgotPassword = 7
    }
}
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerTLA.BLL.DTO
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        //public string ReturnUrl { get; set; }

    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerTLA.Models
{
    public class Role
    {
        [Display(Name = "Ролі")]
        public string UserRole { get; set; }
    }
}

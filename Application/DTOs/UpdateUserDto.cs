﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateUserDto
    {
       // public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public List<int> CategoryIds { get; set; }

    }
}

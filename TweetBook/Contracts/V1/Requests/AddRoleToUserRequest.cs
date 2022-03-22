﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetBook.Contracts.V1.Requests
{
    public class AddRoleToUserRequest
    {
        public string UserEmail { get; set; }

        public string RoleName { get; set; }
    }
}
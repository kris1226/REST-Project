﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartAgent.Web.Controllers
{
    public class HomeController : ApiController
    {
                
        public IHttpActionResult Get()
        {
            return Ok("Hello Web Api");
        }
    }
}

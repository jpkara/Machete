﻿using AutoMapper;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    public class ConfigsController : ApiController
    {
        private readonly IConfigService serv;
        private readonly IMapper map;
        // GET: api/Configs

        public ConfigsController(IConfigService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public IHttpActionResult Get()
        {
            var result = serv.GetAll();
            return Json(new { data = result });
        }

        // GET: api/Configs/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public IHttpActionResult Get(int id)
        {
            var result = serv.Get(id);
            return Json(new { data = result });
        }

        // POST: api/Configs
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Configs/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Configs/5
        [ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = "Administrator")]
        public void Delete(int id)
        {
        }
    }
}
﻿using Api.ViewModels;
using AutoMapper;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Machete.Api.Controllers
{
    public class TransportRulesController : MacheteApiController
    {
        private readonly ITransportRuleService serv;
        private readonly IMapper map;

        public TransportRulesController(ITransportRuleService serv, IMapper map)
        {
            this.serv = serv;
            this.map = map;
        }

        // GET: api/TransportRule
        //[ClaimsAuthorization(ClaimType = CAType.Role, ClaimValue = new[] { CV.Admin, CV.Hirer })]
        [AllowAnonymous]
        public IHttpActionResult Get()
        {

            // TODO this is a hacky workaround until I have viewmodels
            //result.Select(a => { a.costRules = null; return a; }).ToList();
            //result.Select(a => a.costRules.Select(b => { b.transportRule = null; return b; }).ToList()).ToList();
            try
            {
                var result = serv.GetAll()
    .Select(e => map.Map<Domain.TransportRule, TransportRule>(e))
    .AsEnumerable();
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        // GET: api/TransportRule/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TransportRule
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TransportRule/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TransportRule/5
        public void Delete(int id)
        {
        }
    }
}
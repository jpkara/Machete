﻿using Api.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Machete.Api.Maps
{
    public class TransportRulesMap : MacheteProfile
    {
        public TransportRulesMap()
        {
            CreateMap<Domain.TransportRule, TransportRule>()
                .ForMember(v => v.id, opt => opt.MapFrom(d => d.ID))
                ;
            CreateMap<Domain.TransportCostRule, TransportCostRule>();
        }
    }
}
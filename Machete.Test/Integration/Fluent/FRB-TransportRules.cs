﻿using Machete.Data;
using Machete.Domain;
using Machete.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Test.Integration
{
    public partial class FluentRecordBase : IDisposable
    {
        private TransportRuleRepository _repoTR;
        private TransportRuleService _servTR;
        private TransportRule _tr;

        public FluentRecordBase AddRepoTransportRule()
        {
            if (_dbFactory == null) AddDBFactory();

            _repoTR = new TransportRuleRepository(_dbFactory);
            return this;
        }

        public TransportRuleRepository ToRepoTransportRule()
        {
            if (_repoTR == null) AddRepoTransportRule();
            return _repoTR;
        }

        public FluentRecordBase AddServTransportRule()
        {
            //
            // Dependencies
            if (_repoTR == null) AddRepoTransportRule();
            if (_uow == null) AddUOW();
            if (_apiMap == null) AddMapper();

            _servTR = new TransportRuleService(_repoTR, _uow, _apiMap );
            return this;
        }

        public TransportRuleService ToServTransportRule()
        {
            if (_servTR == null) AddServTransportRule();
            return _servTR;
        }
    }
}
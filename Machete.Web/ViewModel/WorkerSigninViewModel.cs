﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Machete.Domain.Resources;
using Machete.Domain;
using Machete.Data;

namespace Machete.Web.ViewModel
{
    public class WorkerSigninViewModel
    {
        [Required(ErrorMessageResourceName = "dwccardnum", ErrorMessageResourceType = typeof(Domain.Resources.Worker))]
        [RegularExpression("^[0-9]{5,5}$", ErrorMessageResourceName = "dwccardnumerror", ErrorMessageResourceType = typeof(Domain.Resources.Worker))]
        public int dwccardentry { get; set; }
        public DateTime dateforsignin { get; set; }
        public Image last_chkin_image { get; set; }
        public DateTime last_chkin_memberexpirationdate { get; set; }
        public bool memberexpired { get; set; }
        public IEnumerable<WorkerSigninView> workersignins { get; set; }
    }
}
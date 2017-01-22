﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machete.Service.DTO
{
    public class WorkerSigninList
    {
        public int? lotterySequence { get; set; }
        public int? skill1 { get; set; }
        public int? skill2 { get; set; }
        public int? skill3 { get; set; }
        public int? waid { get; set; }
        public int englishlevel { get; set; }
        public int typeOfWorkID { get; set; }
        public int dwccardnum { get; set; }
        public string firstname1 { get; set; }
        public string firstname2 { get; set; }
        public string lastname1 { get; set; }
        public string lastname2 { get; set; }
        public string fullname { get; set; }
        public int signinID { get; set; }
        public DateTime dateforsignin { get; set; }
        public int? imageID { get; set; }
        public DateTime expirationDate { get; set; }
        public int? memberStatusID { get; set; }
        public string memberStatusEN { get; set; }
        public string memberStatusES { get; set; }
    }
}

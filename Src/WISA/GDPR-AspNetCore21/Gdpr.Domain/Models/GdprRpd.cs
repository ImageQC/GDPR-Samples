using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprRpd
    {
        public GdprRpd()
        {
            Cxps = new List<GdprCxp>();
            Utas = new List<GdprUta>();
        }
        public List<GdprCxp> Cxps { get; private set; }
        public List<GdprUta> Utas { get; private set; }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int ChildFlag { get; set; }
        public int StopComms { get; set; }
        public int RestrictProc { get; set; }
        public int LoginCount { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? AccountDeleted { get; set; }
        public int UpdateFlag { get; set; }
        public string NotificationUrl { get; set; }
        public string UserId { get; set; }
        public Guid UrdId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastUpdate { get; set; }
        public DateTime? NextUpdate { get; set; }
        public string Source { get; set; }

        public GdprUrd Urd { get; set; }
        public AspNetUsers User { get; set; }
        public GdprCxp GdprCxp { get; set; }
        public GdprUta GdprUta { get; set; }


        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprRxp
    {
        public Guid Id { get; set; }
        public int ProcFlags { get; set; }
        public Guid FpdId { get; set; }
        public Guid UrdId { get; set; }

        public GdprFpd Fpd { get; set; }
        public GdprUrd Urd { get; set; }

        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

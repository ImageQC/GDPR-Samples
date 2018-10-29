using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprUta
    {
        public Guid Id { get; set; }
        public DateTime Accepted { get; set; }
        public DateTime? Revoked { get; set; }
        public Guid RpdId { get; set; }
        public Guid WstId { get; set; }

        public GdprRpd Rpd { get; set; }
        public GdprWst Wst { get; set; }

        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

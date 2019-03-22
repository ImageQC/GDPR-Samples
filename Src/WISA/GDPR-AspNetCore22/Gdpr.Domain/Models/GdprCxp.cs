using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprCxp
    {
        public Guid Id { get; set; }
        public int ConsentCode { get; set; }
        public DateTime Granted { get; set; }
        public DateTime? Revoked { get; set; }
        public Guid FpdId { get; set; }
        public Guid RpdId { get; set; }

        public virtual GdprFpd Fpd { get; set; }
        public virtual GdprRpd Rpd { get; set; }

        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

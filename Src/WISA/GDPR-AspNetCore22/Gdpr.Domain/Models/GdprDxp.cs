using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprDxp
    {
        public Guid Id { get; set; }
        public int ProcFlags { get; set; }
        public Guid FpdId { get; set; }
        public Guid PdsId { get; set; }

        public virtual GdprFpd Fpd { get; set; }
        public virtual GdprPds Pds { get; set; }


        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

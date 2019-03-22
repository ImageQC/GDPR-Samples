using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprDtd
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Details { get; set; }

        public virtual GdprPds GdprPds { get; set; }

        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

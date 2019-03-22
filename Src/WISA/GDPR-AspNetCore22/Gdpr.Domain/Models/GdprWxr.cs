using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprWxr
    {
        public Guid Id { get; set; }
        public Guid UrdId { get; set; }
        public Guid WstId { get; set; }

        public virtual GdprUrd Urd { get; set; }
        public virtual GdprWst Wst { get; set; }


        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

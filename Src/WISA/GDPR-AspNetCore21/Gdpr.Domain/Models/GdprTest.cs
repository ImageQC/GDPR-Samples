using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprTest
    {
        public Guid Id { get; set; }
        public string Ninumber { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastUpdate { get; set; }
        public DateTime? NextUpdate { get; set; }
        public string Source { get; set; }

        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

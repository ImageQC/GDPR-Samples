using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprWst
    {
        public enum StatusVal { NotImplemented = 0, Published = 1 };    //03-12-18
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Published { get; set; }
        public DateTime? Updated { get; set; }
        public int Status { get; set; }
        public string Link { get; set; }
        public string Hash { get; set; }

        public GdprUta GdprUta { get; set; }
        public GdprWxr GdprWxr { get; set; }

        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

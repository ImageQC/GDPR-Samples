using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprEdt
    {
        public GdprEdt()
        {
            Fpds = new List<GdprFpd>();
        }
        public List<GdprFpd> Fpds { get; private set; }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Details { get; set; }
        public int TransferFlag { get; set; }
        public Guid FpdId { get; set; }

        public GdprFpd Fpd { get; set; }


        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprFpd
    {
        public GdprFpd()
        {
            Edts = new List<GdprEdt>();
        }
        public List<GdprEdt> Edts { get; private set; }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProcessingBasis { get; set; }
        public string ProcessingPurpose { get; set; }
        public string ConsentRefusal { get; set; }
        public int ConsentCode { get; set; }
        public int Status { get; set; }
        public int ProcFlag { get; set; }

        public GdprCxp GdprCxp { get; set; }
        public GdprDxp GdprDxp { get; set; }
        public GdprEdt GdprEdt { get; set; }
        public GdprRxp GdprRxp { get; set; }


        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

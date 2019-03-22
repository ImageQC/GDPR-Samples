using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprPdf
    {
        public Guid Id { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public int Value { get; set; }
        public int Combine { get; set; }
        public string Description { get; set; }

        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

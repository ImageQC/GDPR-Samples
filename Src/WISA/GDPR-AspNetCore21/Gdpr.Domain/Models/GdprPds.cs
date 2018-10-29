using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprPds
    {
        public Guid Id { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public int Status { get; set; }
        public int UserUpdateFlag { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Sources { get; set; }
        public string StorageProtection { get; set; }
        public string StorageArchiving { get; set; }
        public int? StorageDur { get; set; }
        public string StorageJustification { get; set; }
        public DateTime Created { get; set; }
        public Guid DtdId { get; set; }
        public Guid DcdId { get; set; }

        public GdprDcd Dcd { get; set; }
        public GdprDtd Dtd { get; set; }
        public GdprDxp GdprDxp { get; set; }

        internal bool IsNew { get { return this.Id == null; } }
        public bool IsDeleted { get; set; }
    }
}

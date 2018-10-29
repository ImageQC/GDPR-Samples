using System;
using System.Collections.Generic;

namespace Gdpr.Domain.Models
{
    public class GdprUrd
    {
        public enum StatusVal { NotImplemented=0, Published=1};
        public GdprUrd()
        {
            Rpds = new List<GdprRpd>();
        }
        public List<GdprRpd> Rpds { get; private set; }
        internal bool IsNew { get { return this.Id == Guid.Empty; } }
        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return String.Format("Name={0}, RoleCode={1}, Status={2}, Purpose.Length={3}, Description.Length={4}, Id={5}", Name, RoleCode, Status, ((Purpose == null) ? -1 : Purpose.Length), (Description == null) ? -1 : Description.Length, Id);
        }

        public static bool IsValidRoleCode(int code) { return (code >= 0) ? true : false; }
        public static bool IsValidStatus(int status) { return Enum.IsDefined(typeof(StatusVal), status); }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RoleCode { get; set; }
        public int Status { get; set; }
        public string Purpose { get; set; }
        public string Description { get; set; }

        public GdprRpd GdprRpd { get; set; }
        public GdprRxp GdprRxp { get; set; }
        public GdprWxr GdprWxr { get; set; }


    }
}

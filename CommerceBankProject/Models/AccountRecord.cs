using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceBankProject.Models
{
    public class AccountRecord
    {
        public string actID { get; set; }
        public string actType { get; set; }

        public override bool Equals(object obj)
        {
            AccountRecord a = obj as AccountRecord;
            return (this.actID == a.actID) && (this.actType == a.actType);
        }
        public override int GetHashCode()
        {
            return this.actID.GetHashCode() ^ this.actType.GetHashCode();
        }
    }
}

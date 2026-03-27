using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Enums
{
    public enum AccountStatus
    {
        Pending = 0,   // chờ verify
        Active = 1,    // đã kích hoạt
        Disabled = 2   // bị khóa
    }
}

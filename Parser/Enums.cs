using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public enum TypeOfAction
    {
        reduce=1,
        shift=2,
        error=3,
        accept=4
    }
}

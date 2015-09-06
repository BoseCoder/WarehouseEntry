using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEntry.Database.DataDefine
{
    public enum FlowStatus
    {
        Start,
        Processing,
        Complete
    }

    public enum CommonTaskSection
    {
        Create,
        Edit,
        Complete
    }
}

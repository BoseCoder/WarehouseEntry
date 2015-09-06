using System;
using System.IO;
using WarehouseEntry.Database.DataModel;

namespace WarehouseEntry.Database.DataDefine
{
    public static class FlowProcessExtention
    {
        public static FlowStatus GetStatusValue(this FlowProcess flow)
        {
            if (!Enum.IsDefined(typeof(FlowStatus), flow.Status))
            {
                throw new InvalidDataException("FlowProcess.Status");
            }
            return (FlowStatus)flow.Status;
        }

        public static void SetStatusValue(this FlowProcess flow, FlowStatus value)
        {
            flow.Status = (int)value;
        }
    }
}

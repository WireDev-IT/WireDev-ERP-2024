using System;

namespace WireDev.Erp.V1.Client.Windows.Classes
{
    public class CommandParameters
    {
        public class YearQueryFilter
        {
            public readonly uint MinYear;
            public readonly uint MaxYear;
        }

        public class TimeStatQuery
        {
            public ushort Year;
            public byte Month = 0;
            public byte Day = 0;
        }
    }
}
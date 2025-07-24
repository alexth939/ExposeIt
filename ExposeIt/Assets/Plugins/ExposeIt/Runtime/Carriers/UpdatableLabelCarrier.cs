using System;

namespace ExposeIt.Runtime.Carriers
{
    /// <summary>
    /// Represents a data carrier for a dynamically updating label in the Play Mode Zone.
    /// </summary>
    public struct UpdatableLabelCarrier
    {
        public Func<string> TextGetter { get; set; }

        public int UpdateIntervalInMS { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MmoCommon
{
    public enum EventCode : byte
    {
        ItemDestroyed = 1,

        ItemMoved,

        ItemPropertiesSet,

        WorldExited,

        ItemSubscribed,

        ItemUnsubscribed,

        ItemProperties,

        RadarUpdate,

        CounterData,

        ItemGeneric,
    }
}

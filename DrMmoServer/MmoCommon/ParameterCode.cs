using System;
using System.Collections.Generic;
using System.Text;

namespace MmoCommon
{
    public enum ParameterCode : byte
    {
        EventCode = 60,

        Username = 91,

        OldPosition = 92,

        Position = 93,

        Properties = 94,

        ItemId = 95,

        ItemType = 96,

        PropertiesRevision = 97,

        CustomEventCode = 98,

        EventData = 99,

        BoundingBox = 100,

        TileDimensions = 101,

        WorldName = 103,

        ViewDistanceEnter = 104,

        PropertiesSet = 105,

        PropertiesUnset = 106,

        EventReliability = 107,

        EventReceiver = 108,

        Subscribe = 109,

        ViewDistanceExit = 110,

        InterestAreaId = 111,

        CounterReceiveInterval = 112,

        CounterName = 113,

        CounterTimeStamps = 114,

        CounterValues = 115,

        Rotation = 116,

        OldRotation = 117,

        Remove = 118

    }
}

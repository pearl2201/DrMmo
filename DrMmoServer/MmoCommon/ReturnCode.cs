using System;
using System.Collections.Generic;
using System.Text;

namespace MmoCommon
{
    public enum ReturnCode
    {
        Ok = 0,

        Fatal = 1,

        ParameterOutOfRange = 51,

        OperationNotSupported,

        InvalidOperationParameter,

        InvalidOperation,

        ItemAccessDenied,

        InterestAreaNotFound,

        InterestAreaAlreadyExists,

        WorldAlreadyExists = 101,

        WorldNotFound,

        ItemAlreadyExists,

        ItemNotFound
    }
}

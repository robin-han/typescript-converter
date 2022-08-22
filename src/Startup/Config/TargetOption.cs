using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace TypeScript.Converter
{
    class TargetOption
    {
        public string GroupId { get; set; }

        public TargetOption()
        {
            this.GroupId = null;
        }
    }
}

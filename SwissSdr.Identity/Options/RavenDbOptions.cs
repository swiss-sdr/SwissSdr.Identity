﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwissSdr.Identity.Options
{
    public class RavenDbOptions
    {
        public const string Name = "RavenDb";

        public string Url { get; set; }
        public string ApiKey { get; set; }
    }
}

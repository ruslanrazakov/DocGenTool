﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core.Interfaces
{
    interface IParagraphBuilder
    {
        string ReplaceWithTemplateAndMetadata(string docTemplate, string docMetadata);
    }
}
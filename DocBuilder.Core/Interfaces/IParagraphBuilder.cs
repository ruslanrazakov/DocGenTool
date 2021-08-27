using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core.Interfaces
{
    interface IParagraphBuilder
    {
        List<OpenXmlElement> Build(string docTemplate, string docMetadata);
    }
}

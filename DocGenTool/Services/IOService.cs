using System;
using DocBuilder.Core;

namespace DocGenTool.Services
{
    interface IOService
    {
        string Open(DocType docType);
    }
}

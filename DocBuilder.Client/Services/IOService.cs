using System;
using System.Collections.Generic;
using DocBuilder.Core;

namespace DocBuilder.Client.Services
{
    interface IOService
    {
        string Open(DocType docType);
        List<string> OpenMultiple();
    }
}

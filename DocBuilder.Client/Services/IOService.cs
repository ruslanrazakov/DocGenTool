using System;
using DocBuilder.Core;

namespace DocBuilder.Client.Services
{
    interface IOService
    {
        string Open(DocType docType);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core.Interfaces
{
    interface IPropertyBuilder
    {
        public void ReplaceProperties(string docTemplate, string docMetadata);
    }
}

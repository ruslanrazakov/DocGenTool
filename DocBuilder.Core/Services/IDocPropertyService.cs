using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core
{
    interface IDocPropertyService
    {
        public void ReplaceGeneralPropsAndSaveTo(string path);
        public void ReplacePackItemPropsAndSaveTo(string path);
    }
}

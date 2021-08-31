using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core
{
    public class BuilderOptions
    {
        public List<string> DocPackageTemplatePaths { get; set; }
        public string DocMetadataPath { get; set; }
        public string DocAnswersPath { get; set; }
    }
}

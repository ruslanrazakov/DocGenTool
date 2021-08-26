using System;

namespace DocBuilder.Core
{
    public class Builder
    {
        private BuilderOptions builderOptions;
        public Builder(BuilderOptions options)
        {
            builderOptions = options;
        }

        public void BuildAndSaveTo(string path)
        {
            throw new NotImplementedException();
        }
    }

    public class BuilderOptions
    {
        public string DocTemplatePath { get; set; }
        public string DocMetadataPath { get; set; }
        public string DocAnswersPath { get; set; }
    }
}

using System;
using System.IO;
using System.Text.Json;
using DocBuilder.Core.Enitites;
using DocBuilder.Core.Services;

namespace DocBuilder.Core
{
    public class Builder
    {
        private BuilderOptions builderOptions;
        private readonly string destinationFolder;

        public Builder(BuilderOptions options)
        {
            builderOptions = options;
            destinationFolder = AppDomain.CurrentDomain.BaseDirectory;
        }
        
        public void BuildAndSave()
        {
            var docPackageAnswers = GetAnswers(builderOptions.DocAnswersPath);
            IDocPropertyService propertyService = new DocPropertyService(docPackageAnswers);
           
            foreach (var filePath in builderOptions.DocPackageTemplatePaths)
            {
                var destinationPath = CopyTemplate(filePath, destinationFolder);
                propertyService.ReplaceGeneralPropsAndSaveTo(destinationPath);
                propertyService.ReplacePackItemPropsAndSaveTo(destinationPath);
            }
        }

        private string CopyTemplate(string filePath, string destinationFolder)
        {
            var destinationPath = $"{destinationFolder}{Path.GetFileName(filePath)}";
            File.Copy(filePath, destinationPath, overwrite: false);
            return destinationPath;
        }

        private DocPackageAnswersEntity GetAnswers(string docAnswersPath)
        {
            var answersJson = File.ReadAllText(docAnswersPath);
            var jsonOptions = new JsonSerializerOptions { AllowTrailingCommas = true };
            return JsonSerializer.Deserialize<DocPackageAnswersEntity>(answersJson, jsonOptions);
        }
    }
}

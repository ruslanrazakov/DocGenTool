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
        private bool overwriteDestinationFiles = true;

        public Builder(BuilderOptions options)
        {
            builderOptions = options;
            destinationFolder = AppDomain.CurrentDomain.BaseDirectory;
        }
        
        /// <summary>
        /// Создает копии шаблонов документов и проводит их через ряд бизнес-сервисов
        /// обрабатывающих их в соответствии с пакетом ответов шаблона документов.
        /// </summary>
        public void BuildAndSave()
        {
            var docPackageAnswers = GetAnswers(builderOptions.DocAnswersPath);
            IDocPropertyService propertyService = new DocPropertyService(docPackageAnswers);
            IDocSubsectionService subsectionService = new DocSubsectionService(docPackageAnswers);
           
            foreach (var filePath in builderOptions.DocPackageTemplatePaths)
            {
                var destinationPath = CopyTemplate(filePath, destinationFolder);

                propertyService.ReplaceGeneralPropsIn(destinationPath);
                propertyService.ReplacePackItemPropsIn(destinationPath);
                subsectionService.RemoveNeedlessSubsectionsFrom(destinationPath);
            }
        }

        /// <summary>
        /// Копирует шаблон документа в destinationFolder для его дальнейшей обработки
        /// Возвращает полный путь до файла
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        private string CopyTemplate(string filePath, string destinationFolder)
        {
            var destinationPath = Path.Combine(destinationFolder, Path.GetFileName(filePath));
            File.Copy(filePath, destinationPath, overwrite: true);
            return destinationPath;
        }

        /// <summary>
        /// Парсит Json файл с пакетом ответов, и возвращает содержимое в виде объектной модели
        /// </summary>
        /// <param name="docAnswersPath"></param>
        /// <returns></returns>
        private DocPackageAnswersEntity GetAnswers(string docAnswersPath)
        {
            var answersJson = File.ReadAllText(docAnswersPath);
            var jsonOptions = new JsonSerializerOptions { AllowTrailingCommas = true };
            return JsonSerializer.Deserialize<DocPackageAnswersEntity>(answersJson, jsonOptions);
        }
    }
}

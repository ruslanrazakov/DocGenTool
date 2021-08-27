using System;
using System.Collections.Generic;
using System.Linq;
using DocBuilder.Core.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

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
            IParagraphBuilder paragraphBuilder = new ParagraphBuilder();
            var paragraphs = paragraphBuilder.Build(builderOptions.DocTemplatePath, builderOptions.DocAnswersPath);
            CreateAndSaveDoc(paragraphs, path);
        }

        private void CreateAndSaveDoc(List<OpenXmlElement> paragraphs, string path)
        {
            using (WordprocessingDocument package = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document))
            { 
                package.AddMainDocumentPart();
                package.MainDocumentPart.Document = new Document( new Body());

                foreach(var par in paragraphs)
                {
                    //we must Clone() every node in paragraphs, cuz they are parts of source tree yet
                    //see: https://stackoverflow.com/questions/16320537/cannot-insert-the-openxmlelement-newchild-because-it-is-part-of-a-tree
                    package.MainDocumentPart.Document.Body.AppendChild(par.CloneNode(deep: true));
                }
                DeleteAllComments(package);
                package.MainDocumentPart.Document.Save();
            }
        }

        void DeleteAllComments(WordprocessingDocument package)
        {
            // Delete CommentRangeStart for each
            // deleted comment in the main document.
            List<CommentRangeStart> commentRangeStartToDelete =
                package.MainDocumentPart.Document.Descendants<CommentRangeStart>().ToList();
            foreach (CommentRangeStart c in commentRangeStartToDelete)
            {
                c.Remove();
            }

            // Delete CommentRangeEnd for each deleted comment in the main document.
            List<CommentRangeEnd> commentRangeEndToDelete =
                package.MainDocumentPart.Document.Descendants<CommentRangeEnd>().ToList();
            foreach (CommentRangeEnd c in commentRangeEndToDelete)
            {
                c.Remove();
            }

            // Delete CommentReference for each deleted comment in the main document.
            List<CommentReference> commentRangeReferenceToDelete = package.MainDocumentPart.Document.Descendants<CommentReference>().ToList();
            foreach (CommentReference c in commentRangeReferenceToDelete)
            {
                c.Remove();
            }
        }
    }

    public class BuilderOptions
    {
        public string DocTemplatePath { get; set; }
        public string DocMetadataPath { get; set; }
        public string DocAnswersPath { get; set; }
    }
}

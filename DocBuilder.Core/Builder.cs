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
            List<CommentRangeStart> commentRangeStartToDelete =
                package.MainDocumentPart.Document.Descendants<CommentRangeStart>().ToList();
            commentRangeStartToDelete.ForEach(c => c.Remove());

            List<CommentRangeEnd> commentRangeEndToDelete =
                package.MainDocumentPart.Document.Descendants<CommentRangeEnd>().ToList();
            commentRangeEndToDelete.ForEach(c => c.Remove());

            List<CommentReference> commentRangeReferenceToDelete = 
                package.MainDocumentPart.Document.Descendants<CommentReference>().ToList();
            commentRangeReferenceToDelete.ForEach(c => c.Remove());
        }
    }
}

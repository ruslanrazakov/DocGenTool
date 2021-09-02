using DocBuilder.Core.Enitites;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocBuilder.Core.Services
{
    class DocSubsectionService : IDocSubsectionService
    {
        private readonly DocPackageAnswersEntity docPackageAnswers;

        public DocSubsectionService(DocPackageAnswersEntity answers)
        {
            docPackageAnswers = answers;
        }

        /// <summary>
        /// Удаляет секции в документе, которые окруженны комментариями.
        /// Оставляет только те секции с комментариями, которые упомянуты в
        /// файле ответов пакета в разделе packItems[].Variants[]
        /// </summary>
        /// <param name="filePath"></param>
        public void RemoveNeedlessSubsectionsFrom(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var packItem = docPackageAnswers.PackItems.FirstOrDefault(pi => pi.Name == fileName);
            foreach (var variant in packItem.Variants)
            {
                SearchAndManageSubsections(filePath, variant);
            }
            DeleteComments(filePath);
        }

        private void SearchAndManageSubsections(string fileName, Variant variant)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                var document = mainPart.Document;
                var currentCommentMetadata = (Comment)mainPart.WordprocessingCommentsPart
                                                               .Comments.ChildElements
                                                               .FirstOrDefault(ccm => ccm.InnerText == variant.Id);
                var commentStart = document.MainDocumentPart.Document
                                                            .Descendants<CommentRangeStart>()
                                                            .FirstOrDefault(cs => cs.Id == currentCommentMetadata.Id);
                var commentEnd = document.MainDocumentPart.Document
                                                          .Descendants<CommentRangeEnd>()
                                                          .FirstOrDefault(cs => cs.Id == currentCommentMetadata.Id);

                ManageSubsection(document, variant, commentStart, commentEnd);

                wordDoc.Save();
            }
        }

        private void ManageSubsection(Document document, Variant variant, CommentRangeStart commentStart, CommentRangeEnd commentEnd)
        {
            var targetElements = GetTargetSubsection(document, commentStart, commentEnd);
            int subsectionCopiesAmount = variant.Values.First().CopiesAmountValue;

            var currentElement = targetElements.Last();

            //если запрашивается одна копия подраздела - оставляем как есть, подраздел уже существует
            //если больше - генерируем subsectionCopiesAmount - 1, т.к. один подраздел уже существует
            //есди ноль - удаляем все ссылки на собранные ноды в targetElements
            if (subsectionCopiesAmount > 1)
            {
                foreach (var subsectionNum in Enumerable.Range(0, subsectionCopiesAmount - 1))
                    foreach (var targetElement in targetElements)
                    {
                        if (targetElement is Paragraph)
                        {
                            var elementToInsert = targetElement.CloneNode(true);
                            currentElement.InsertAfterSelf(elementToInsert);
                            currentElement = currentElement.NextSibling();
                        }
                    }
            }
            if (subsectionCopiesAmount == 0)
            {
                foreach (var needless in targetElements)
                {
                    if (needless is Paragraph)
                    {
                        needless.RemoveAllChildren();
                        needless.Remove();
                    }
                }
            }
        }

        private List<OpenXmlElement> GetTargetSubsection(Document document, CommentRangeStart commentStart, CommentRangeEnd commentEnd)
        {
            bool isTargetElement = false;
            var targetElements = new List<OpenXmlElement>();
            foreach (var element in document.Body.Descendants())
            {
                if (element.Equals(commentStart) || element.Contains(commentStart))
                {
                    isTargetElement = true;
                }

                if (element.Equals(commentEnd) || element.Contains(commentEnd))
                {
                    targetElements.Add(element);
                    isTargetElement = false;
                    break;
                }

                if (isTargetElement)
                {
                    targetElements.Add(element);
                }
            }
            return targetElements;
        }

        // Delete comments by a specific author. Pass an empty string for the 
        // author to delete all comments, by all authors.
        public static void DeleteComments(string fileName)
        {
            // Get an existing Wordprocessing document.
            using (WordprocessingDocument document =
                WordprocessingDocument.Open(fileName, true))
            {
                // Set commentPart to the document WordprocessingCommentsPart, 
                // if it exists.
                WordprocessingCommentsPart commentPart =
                    document.MainDocumentPart.WordprocessingCommentsPart;

                // If no WordprocessingCommentsPart exists, there can be no 
                // comments. Stop execution and return from the method.
                if (commentPart == null)
                {
                    return;
                }

                // Create a list of comments by the specified author, or
                // if the author name is empty, all authors.
                List<Comment> commentsToDelete =
                    commentPart.Comments.Elements<Comment>().ToList();
                commentsToDelete = commentsToDelete.ToList();
                IEnumerable<string> commentIds =
                    commentsToDelete.Select(r => r.Id.Value);

                // Delete each comment in commentToDelete from the 
                // Comments collection.
                foreach (Comment c in commentsToDelete)
                {
                    c.Remove();
                }

                // Save the comment part change.
                commentPart.Comments.Save();

                Document doc = document.MainDocumentPart.Document;

                // Delete CommentRangeStart for each
                // deleted comment in the main document.
                List<CommentRangeStart> commentRangeStartToDelete =
                    doc.Descendants<CommentRangeStart>().
                    Where(c => commentIds.Contains(c.Id.Value)).ToList();
                foreach (CommentRangeStart c in commentRangeStartToDelete)
                {
                    c.Remove();
                }

                // Delete CommentRangeEnd for each deleted comment in the main document.
                List<CommentRangeEnd> commentRangeEndToDelete =
                    doc.Descendants<CommentRangeEnd>().
                    Where(c => commentIds.Contains(c.Id.Value)).ToList();
                foreach (CommentRangeEnd c in commentRangeEndToDelete)
                {
                    c.Remove();
                }

                // Delete CommentReference for each deleted comment in the main document.
                List<CommentReference> commentRangeReferenceToDelete =
                    doc.Descendants<CommentReference>().
                    Where(c => commentIds.Contains(c.Id.Value)).ToList();
                foreach (CommentReference c in commentRangeReferenceToDelete)
                {
                    c.Remove();
                }

                // Save changes back to the MainDocumentPart part.
                doc.Save();
            }
        }
    }
}

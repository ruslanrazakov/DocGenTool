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
                GetSubsectionsAndRemoveNeedless(filePath, variant.Id);
        }

        /// <summary>
        /// Создает объекты комментариев, и парсит все параграфы в документе,
        /// собирая те, что не окружены актуальными нодами commentStart и commentEnd
        /// и удаляя эти параграфы
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="commentInnerText"></param>
        private void GetSubsectionsAndRemoveNeedless(string fileName, string commentInnerText)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                var document = mainPart.Document;
                var currentCommentMetadata = (Comment)mainPart.WordprocessingCommentsPart
                                                               .Comments.ChildElements
                                                               .FirstOrDefault(ccm => ccm.InnerText == commentInnerText);
                var commentStart = document.MainDocumentPart.Document
                                                            .Descendants<CommentRangeStart>()
                                                            .FirstOrDefault(cs => cs.Id == currentCommentMetadata.Id);
                var commentEnd = document.MainDocumentPart.Document
                                                          .Descendants<CommentRangeEnd>()
                                                          .FirstOrDefault(cs => cs.Id == currentCommentMetadata.Id);

                var elementsToDelete = GetNeedlessParagraphsFromDoc(document, commentStart, commentEnd);
                foreach (var element in elementsToDelete)
                {
                    element.RemoveAllChildren();
                    element.Remove();
                }

                wordDoc.Save();
            }
        }

        /// <summary>
        /// Собирает параграфы, подлежащие удалению в список к удалению.
        /// Подлежащие удалению параграфы - это параграфы, которые
        /// находятся внутрии секции комментария, не упомянутого в packItem.Variants[].
        /// </summary>
        /// <param name="document"></param>
        /// <param name="commentStart"></param>
        /// <param name="commentEnd"></param>
        /// <returns></returns>
        private List<OpenXmlElement> GetNeedlessParagraphsFromDoc(Document document, CommentRangeStart commentStart, 
                                                                                   CommentRangeEnd commentEnd)
        {
            bool remove = false;
            var elementsToDelete = new List<OpenXmlElement>();

            foreach (var paragraph in document.Body.Descendants<Paragraph>())
            {
                if (ParagraphHasStartComment(paragraph) && CommentIsActual(paragraph, commentStart))
                {
                    remove = false;
                    Console.WriteLine("START: Dont remove this section...");
                }
                if (ParagraphHasStartComment(paragraph) && !CommentIsActual(paragraph, commentStart))
                {
                    remove = true;
                    Console.WriteLine("START: Removing...");
                }
                if (ParagraphHasEndComment(paragraph) && CommentIsActual(paragraph, commentEnd))
                {
                    Console.WriteLine("END");
                    remove = false;
                }
                if (ParagraphHasEndComment(paragraph) && !CommentIsActual(paragraph, commentEnd))
                {
                    elementsToDelete.Add(paragraph);
                    Console.WriteLine("END");
                    remove = false;
                }

                if (remove)
                {
                    elementsToDelete.Add(paragraph);
                }
            }
            return elementsToDelete;
        }

        /// <summary>
        /// Определяет, содержит ли параграф данный конкретный коммент
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        private bool CommentIsActual(Paragraph paragraph, OpenXmlElement comment)
        {
            foreach (var element in paragraph)
            {
                if (element.Equals(comment))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Определяет, есть ли в параграфе XML нода commentRangeStart
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="commentStart"></param>
        /// <returns></returns>
        private bool ParagraphHasStartComment(Paragraph paragraph)
        {
            foreach (var element in paragraph)
            {
                if (element is CommentRangeStart)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Определяет, есть ли в параграфе XML нода commentRangeEnd
        /// </summary>
        /// <param name="paragraph"></param>
        /// <returns></returns>
        private bool ParagraphHasEndComment(Paragraph paragraph)
        {
            foreach (var element in paragraph)
            {
                if (element is CommentRangeEnd)
                    return true;
            }
            return false;
        }
    }
}

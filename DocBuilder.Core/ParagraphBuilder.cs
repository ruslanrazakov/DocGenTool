using DocBuilder.Core.Enitites;
using DocBuilder.Core.Interfaces;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DocBuilder.Core
{
    class ParagraphBuilder : IParagraphBuilder
    {
        public List<OpenXmlElement> Build(string docTemplatePath, string docAnswersPath)
        {
            var docAnswers = GetAnswers(docAnswersPath);
            var paragraphs = GetParagraphInCommentSection(docTemplatePath, docAnswers.variants.First().id);
            return paragraphs;
        }

        private AnswersEntity GetAnswers(string docAnswersPath)
        {
            var answersJson = File.ReadAllText(docAnswersPath);
            var jsonOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };
            return JsonSerializer.Deserialize<AnswersEntity>(answersJson, jsonOptions);
        }

        public List<OpenXmlElement> GetParagraphInCommentSection(string fileName, string commentInnerText)
        {
            List<OpenXmlElement> resultParagraphs = new List<OpenXmlElement>();
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(fileName, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                var document = mainPart.Document;
                var comments = mainPart.WordprocessingCommentsPart.Comments.ChildElements
                                                                            .Where(c=>c.InnerText == commentInnerText);
                CommentRangeStart commentStart;
                CommentRangeEnd commentEnd;
                var paragraphs = document.Body.Descendants<Paragraph>();

                foreach (Comment comment in comments)
                {
                    commentStart = document.MainDocumentPart.Document.Descendants<CommentRangeStart>().FirstOrDefault(c => c.Id == comment.Id);
                    commentEnd = document.MainDocumentPart.Document.Descendants<CommentRangeEnd>().FirstOrDefault(c => c.Id == comment.Id);
                    resultParagraphs = InspectCommentSectionForParagraphs(paragraphs, commentStart, commentEnd);
                }
            };
            return resultParagraphs;
        }

        private List<OpenXmlElement> InspectCommentSectionForParagraphs(IEnumerable<Paragraph> paragraphs, CommentRangeStart commentStart,
                                                                                                           CommentRangeEnd commentEnd)
        {
            bool paragraphIsParsing = false;
            var resultParagraphs = new List<OpenXmlElement>();

            foreach (var paragraph in paragraphs)
            {
                if (paragraph.Contains(commentStart))
                    paragraphIsParsing = true;

                if (paragraphIsParsing)
                    resultParagraphs.Add(paragraph);

                if (paragraphIsParsing && paragraph.Contains(commentEnd))
                {
                    paragraphIsParsing = false;
                    break;
                }
            }
            return resultParagraphs;
        }
    }
}

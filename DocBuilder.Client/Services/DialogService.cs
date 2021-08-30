using System;
using Microsoft.Win32;
using DocBuilder.Core;

namespace DocBuilder.Client.Services
{
    class DialogService : IOService
    {
        public string Open(DocType doctype)
        {
            OpenFileDialog fileDialog = new();
            switch (doctype)
            {
                case DocType.Template:
                    fileDialog.Filter = "doc files (*.doc;*.docx;*.odt)|*.doc;*docx;*.odt";
                    break;
                case DocType.Answers:
                case DocType.Metadata:
                    fileDialog.Filter = "JSON files (*.json;*.txt;)|*.json;*.txt;";
                    break;
            };
            return (bool)fileDialog.ShowDialog() ? fileDialog.FileName : String.Empty;
        }
    }
}

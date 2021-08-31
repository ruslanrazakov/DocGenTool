using System;
using Microsoft.Win32;
using DocBuilder.Core;
using System.Collections.Generic;
using System.Linq;

namespace DocBuilder.Client.Services
{
    class DialogService : IOService
    {
        public string Open(DocType doctype)
        {
            OpenFileDialog fileDialog = new() { Multiselect = true };
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

        public List<string> OpenMultiple()
        {
            List<string> paths = new();
            OpenFileDialog fileDialog = new()
            { 
                Multiselect = true,
                Filter = "doc files (*.doc;*.docx;*.odt)|*.doc;*docx;*.odt"
            };

            if(fileDialog.ShowDialog().Value)
            {
                foreach (var path in fileDialog.FileNames)
                    paths.Add(path);
            }
            return paths;
        }
    }
}

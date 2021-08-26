using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;

namespace DocGenTool.Services
{
    class DialogService : IOService
    {
        public string Open()
        {
            OpenFileDialog fileDialog = new();
            fileDialog.Filter = "doc files (*.doc;*.docx;*.odt)|*.doc;*docx;*.odt";
            return (bool)fileDialog.ShowDialog() ? fileDialog.FileName : String.Empty;
        }
    }
}

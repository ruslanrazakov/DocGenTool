using DocGenTool.MVVM;
using DocGenTool.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DocBuilder.Core;

namespace DocGenTool.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        string _docTemplatePath;
        public string DocTemplatePath
        {
            get => _docTemplatePath;
            set => SetProperty(ref _docTemplatePath, value);
        }
        string _docMetadataPath;
        public string DocMetadataPath
        {
            get => _docMetadataPath;
            set => SetProperty(ref _docMetadataPath, value);
        }
        string _docAnswersPath;
        public string DocAnswersPath
        {
            get => _docAnswersPath;
            set => SetProperty(ref _docAnswersPath, value);
        }
        public IAsyncCommand OpenDocTemplateCommand { get; set; }
        public IAsyncCommand OpenDocMetadataCommand { get; set; }
        public IAsyncCommand OpenDocAnswersCommand { get; set; }
        public IAsyncCommand GenerateDocCommand { get; set; }
        readonly IOService _ioService;

        public MainViewModel(IOService ioService)
        {
            _ioService = ioService;
            OpenDocTemplateCommand = new AsyncCommand(() => OpenDoc(DocType.Template));
            OpenDocMetadataCommand = new AsyncCommand(() => OpenDoc(DocType.Metadata));
            OpenDocAnswersCommand = new AsyncCommand(() => OpenDoc(DocType.Answers));
            GenerateDocCommand = new AsyncCommand(() => GenerateDoc());
        }

        private async Task OpenDoc(DocType docType)
        {
            switch(docType)
            {
                case DocType.Template:
                    DocTemplatePath = _ioService.Open();
                    break;
                case DocType.Metadata:
                    DocMetadataPath = _ioService.Open();
                    break;
                case DocType.Answers:
                    DocAnswersPath = _ioService.Open();
                    break;
            };
            await Task.Delay(1000);
        }

        private async Task GenerateDoc()
        {
            Builder docBuilder = new Builder(new BuilderOptions()
            {
                DocTemplatePath = this.DocTemplatePath,
                DocMetadataPath = this.DocMetadataPath,
                DocAnswersPath = this.DocAnswersPath
            });
            docBuilder.BuildAndSaveTo(AppDomain.CurrentDomain.BaseDirectory + "OutputDoc.docx");
            await Task.Delay(1000);
        }
    }
}
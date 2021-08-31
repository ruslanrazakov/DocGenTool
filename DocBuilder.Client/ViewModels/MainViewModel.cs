using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DocBuilder.Client.MVVM;
using DocBuilder.Client.Services;
using DocBuilder.Core;

namespace DocBuilder.Client.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        ObservableCollection<string> _docPackageTemplatePaths;
        public ObservableCollection<string> DocPackageTemplatePaths
        {
            get => _docPackageTemplatePaths;
            set => SetProperty(ref _docPackageTemplatePaths, value);
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

            DocPackageTemplatePaths = new();
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
                    DocPackageTemplatePaths.AddRange(_ioService.OpenMultiple());
                    break;
                case DocType.Metadata:
                    DocMetadataPath = _ioService.Open(docType);
                    break;
                case DocType.Answers:
                    DocAnswersPath = _ioService.Open(docType);
                    break;
            };
            await Task.Delay(10);
        }

        private async Task GenerateDoc()
        {
            if (!String.IsNullOrWhiteSpace(DocAnswersPath) && DocPackageTemplatePaths.Any())
            {
                Builder docBuilder = new Builder(new BuilderOptions()
                {
                    DocPackageTemplatePaths = this.DocPackageTemplatePaths.ToList(),
                    DocMetadataPath = this.DocMetadataPath,
                    DocAnswersPath = this.DocAnswersPath
                });

                docBuilder.BuildAndSave();
            }
            await Task.Delay(10);
        }
    }
}
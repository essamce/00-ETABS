using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

using WpfApp1.ViewModel;

namespace WpfApp1.Utils
{


    public class MOpenFileDialog : ViewModelBase
    {
        #region Properties

        //public class Filters
        //{
        //    public const string AllFiles = "All Files|*.*";
        //    public const string TextFiles = "Text (.txt)|*.txt";
        //    public const string sdrFiles = "Text(.txt)|*.txt;*.sdr";
        //    public const string sdrFiles = "Text(.txt)|*.txt;*.sdr";
        //}

        private string _fileName;

        /// <summary>
        /// MOpenFileDialog property applies INotifyPropertyChanged.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set 
            {
                _fileName = value;
                RaisePropertyChanged(nameof(FileName));
            }
        }

        private string _dirName;

        /// <summary>
        /// MOpenFileDialog property applies INotifyPropertyChanged.
        /// </summary>
        public string DirName
        {
            get { return _dirName; }
            set
            {
                _dirName = value;
                RaisePropertyChanged(nameof(DirName));
            }
        }

        private string _fileNameColor = Colors.Black.ToString();

        /// <summary>
        /// MOpenFileDialog property applies INotifyPropertyChanged.
        /// </summary>
        public string FileNameColor
        {
            get { return _fileNameColor; }
            set
            {
                _fileNameColor = value;
                RaisePropertyChanged(nameof(FileNameColor));
            }
        }

        private bool _isFileSelected;

        /// <summary>
        /// MOpenFileDialog property applies INotifyPropertyChanged.
        /// </summary>
        public bool ShowDialValue
        {
            get { return _isFileSelected; }
            set
            {
                _isFileSelected = value;
                RaisePropertyChanged(nameof(ShowDialValue));
            }
        }

        private OpenFileDialog _openDialog;

        /// <summary>
        /// MOpenFileDialog property applies INotifyPropertyChanged.
        /// </summary>
        public OpenFileDialog Dialog
        {
            get { return _openDialog; }
            set
            {
                _openDialog = value;
                RaisePropertyChanged(nameof(Dialog));
            }
        }

        private string _sourceName;

        /// <summary>
        /// MOpenFileDialog property applies INotifyPropertyChanged.
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
            set 
            { 
                _sourceName = value;
                RaisePropertyChanged(nameof(SourceName));
            }
        }

        #endregion

        #region Constructors

        public MOpenFileDialog()
        {
            OpenFileCommand = new RelayCommand<object>(OpenFile);


            Dialog = new OpenFileDialog()
            {
                FileName = "Documen", // default name.
                Filter = "All Files|*.*", // filtrer based on extensions.
            };
        }


        #endregion

        #region Methods

        /// <summary>
        /// open file method
        /// </summary>
        /// <param name="param">value you can pass from
        /// a control (object,string, ...)</param>
        public void OpenFile(object param)
        {
            ModifyTheSourceName(param);
            ShowDialValue = Dialog.ShowDialog().Value;

            if (ShowDialValue != true)
            {
                OpenFileDialgFailed();
                return;
            }
            else
            {
                OpenFileDialgSucceeded();
            }
        }

        void OpenFileDialgSucceeded()
        {
            FileName = Dialog.FileName;
            DirName = Path.GetDirectoryName(Dialog.FileName);
            FileNameColor = Colors.Black.ToString();
        }

        void OpenFileDialgFailed()
        {
            FileName = "No file selected";
            DirName = "No file selected";
            FileNameColor = Colors.Red.ToString();
        }

        void ModifyTheSourceName(object source)
        {
            SourceName = "someanamee";
        }


        #endregion

        #region Commands
        public ICommand OpenFileCommand { get; set; }

        #endregion

    }
}

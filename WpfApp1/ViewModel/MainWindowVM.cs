using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Utils;
using System.IO;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {

        #region Properties
        public MOpenFileDialog OpenETABS { get; set; }
        public MOpenFileDialog OpenModel { get; set; }

        private string _fromETABS;
        public string FromETABSCode
        {
            get { return _fromETABS; }
            set 
            {
                _fromETABS = value;
                RaisePropertyChanged(nameof(FromETABSCode));
            }
        }


        //public ObservableCollection<Name> Names { get; set; }

        #endregion

        #region Constructor

        public MainWindowVM()
        {
            OpenETABS = new MOpenFileDialog();
            OpenETABS.Dialog.Filter = "app (.exe)|*.exe";

            OpenModel = new MOpenFileDialog();

            StartCommand = new RelayCommand<object>(ETABSCode);
        }

        #endregion

        #region Methods

        public void ETABSCode (object param)
        {
            // checks
            #region checks

            //if (!IsRequiredFileAndFolderSelected())
            //{
            //    FromETABSCode = "select the required folder and file filrst";
            //    return;
            //}
            //else
            //{
            //    FromETABSCode = string.Empty;
            //}
            #endregion

            #region Original ETABS code

            string result = string.Empty;

            //Get a reference to cSapModel to access all API classes and functions
            ETABSv17.cSapModel sapModel = default(ETABSv17.cSapModel) ;
            ETABS1.AttachToETABSRunningInstance(ref sapModel, ref result);

            if (result != "OK")
            {
                FromETABSCode += result;
                return;
            }
            //Use returnVal to check if functions return successfully (ret = 0) or fail (ret = nonzero) 
            int returnVal = 0;

            //sapModel.PointObj.


            //Save model
            //returnVal = sapModel.File.Save(ModelPath);

            //Run analysis
            //ret = sapModel.Analyze.RunAnalysis();

            //Clean up variables
            sapModel = null;

            //Check ret value 
            if (returnVal == 0)
            {
                //MessageBox.Show("API script completed successfully.");
                FromETABSCode += $"\nAPI script completed successfully.";
            }
            else
            {
                //MessageBox.Show("API script FAILED to complete.");
                FromETABSCode += $"\nAPI script FAILED to complete.";
            }

            #endregion
        }

        bool IsRequiredFileAndFolderSelected()
        {

            return /*OpenETABS.ShowDialValue &&*/ OpenModel.ShowDialValue;
        }

        #endregion

        #region Commands

        public ICommand StartCommand { get; set; }

        #endregion

        #region Classes

        #endregion



    }
}

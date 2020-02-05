using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Utils;
using System.IO;


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

            if (!IsRequiredFileAndFolderSelected())
            {
                FromETABSCode = "select the required folder and file filrst";
                return;
            }
            else
            {
                FromETABSCode = string.Empty;
            }
            #endregion

            #region Original ETABS code

            //set the following flag to true to attach to an existing instance of the program 
            //otherwise a new instance of the program will be started 
            bool IsETABsInstanceRunning = true;

            //set the following flag to true to manually specify the path to ETABS.exe
            //this allows for a connection to a version of ETABS other than the latest installation
            //otherwise the latest installed version of ETABS will be launched
            bool IsPathSpecified = false;

            //if the above flag is set to true, specify the path to ETABS below
            // "C:\Program Files\Computers and Structures\ETABS 18\ETABS.exe"
            string ProgramPath = @"C:\Program Files\Computers and Structures\ETABS 18\ETABS.exe";
            if (OpenETABS.ShowDialValue)
            {
                ProgramPath = OpenETABS.FileName;
                IsPathSpecified = true;
            }
                

            //full path to the model 
            //set it to an already existing folder ("C:\\CSi_ETABS_API_Example")
            string ModelDirectory = OpenModel.DirName;
            //try
            //{
            //    System.IO.Directory.CreateDirectory(ModelDirectory);
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show("Could not create directory: " + ModelDirectory);
            //    FromETABS += $"\nthe fucking source {OpenETABS.SourceName}";
            //}
            // the above try catch block already handled by OpenModel.
            string ModelName = "myFirstETABS-API-examplee.edb";
            string ModelPath = Path.Combine(ModelDirectory, ModelName);//ModelDirectory + Path.DirectorySeparatorChar + ModelName;

            //dimension the ETABS Object as cOAPI type
            ETABSv17.cOAPI myETABSObject = null;

            //Use returnVal to check if functions return successfully (ret = 0) or fail (ret = nonzero) 
            int returnVal = 0;

            if (IsETABsInstanceRunning)
            {
                //attach to a running instance of ETABS 
                try
                {
                    //get the active ETABS object
                    myETABSObject = (ETABSv17.cOAPI)System.Runtime.InteropServices.Marshal.GetActiveObject("CSI.ETABS.API.ETABSObject");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}--No running instance of the program found or failed to attach.");
                    return;
                }
            }
            else
            {
                //create API helper object
                ETABSv17.cHelper myHelper;
                try
                {
                    myHelper = new ETABSv17.Helper();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}--Cannot create an instance of the Helper object");
                    return;
                }


                if (IsPathSpecified)
                {
                    //'create an instance of the ETABS object from the specified path
                    try
                    {
                        //create ETABS object
                        myETABSObject = myHelper.CreateObject(ProgramPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Cannot start a new instance of the program from " + ProgramPath);
                        return;
                    }
                }
                else
                {
                    //'create an instance of the ETABS object from the latest installed ETABS
                    try
                    {
                        //create ETABS object
                        myETABSObject = myHelper.CreateObjectProgID("CSI.ETABS.API.ETABSObject");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Cannot start a new instance of the program.");
                        return;
                    }
                }

                //start ETABS application
                returnVal = myETABSObject.ApplicationStart();
            }

            //Get a reference to cSapModel to access all API classes and functions
            ETABSv17.cSapModel mySapModel = default(ETABSv17.cSapModel);
            mySapModel = myETABSObject.SapModel;

            ////Initialize model
            //returnVal = mySapModel.InitializeNewModel();


            //////////////////////////////////////////////////////////////////////////////////////
            ////Create steel deck template model
            //returnVal = mySapModel.File.NewSteelDeck(
            //    1 /*num of storys*/,
            //    3 /*typical story height*/,
            //    1 /*bottom story heiht*/,
            //    2 /*num of line x*/,
            //    2 /*num of line y*/,
            //    4 /*x spacing*/,
            //    4 /*y spacing*/);
            ////////////////////////////////////////////////////////////////////////////////////////

            //returnVal = mySapModel.File.NewBlank();


            //Save model
            returnVal = mySapModel.File.Save(ModelPath);

            //Run analysis
            //ret = mySapModel.Analyze.RunAnalysis();

            //Close ETABS
            myETABSObject.ApplicationExit(false);

            //Clean up variables
            mySapModel = null;
            myETABSObject = null;

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

            FromETABSCode += $"\n\n end of ETABSCode method.";
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

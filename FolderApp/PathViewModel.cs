using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FolderApp
{
    class PathViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Members
        private Path _Path;

        private int _minimum = 0;
        private int _maximum = 0;
        private int _currentProgress;
        #endregion

        #region Construction
        public PathViewModel()
        {
            _Path = new Path {};
        }
        #endregion

        #region Properties
        public Path Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }

        public string StringPath
        {
            get { return _Path.StringPath; }
            set
            {
                if (_Path.StringPath != value)
                {
                    _Path.StringPath = value;
                    RaisePropertyChanged("StringPath");
                    RaisePropertyChanged("Maximum");
                }
            }
        }

        public int Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
            }
        }

        public int Maximum
        {
            get
            {
                if (StringPath != "")
                {
                    List<String> files = SearchFiles(StringPath);
                    _maximum = files.Count;
                }
                return _maximum;      
            }
        }

        public int CurrentProgress
        {
            get
            {
                return this._currentProgress;
            }
            set
            {
                if (this._currentProgress != value)
                {
                    this._currentProgress = value;
                    RaisePropertyChanged("CurrentProgress");
                }
            }
        }

        #endregion

        #region Methods

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<String> SearchFiles(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(SearchFiles(d));
                }
            }
            catch (System.Exception excpt)
            {
                //MessageBox.Show(excpt.Message);
            }

            return files;
        }

        public void DoWork(string directoryPath)
        {
            DirectoryInfo Path = new DirectoryInfo(directoryPath);
            foreach (FileInfo file in Path.GetFiles())
            {
                file.Delete();
                Thread.Sleep(500);
                CurrentProgress = CurrentProgress + 1;
            }

            foreach (DirectoryInfo dir in Path.GetDirectories())
            {
                DoWork(dir.FullName);
                dir.Delete();
            }
        }

        public BackgroundWorker BackgroundWorker()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync();
            return (worker);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DoWork(this.StringPath);
            DirectoryInfo Path = new DirectoryInfo(this.StringPath);
            Path.Delete();
        }

        #endregion

        #region ICommand
        private ICommand comSelectFolder;

        public ICommand UpdateCommand
        {
            get
            {
                if (comSelectFolder == null)
                    comSelectFolder = new SelectFolder();
                return comSelectFolder;
            }
            set
            {
                comSelectFolder = value;
            }
        }

        private ICommand comDeleteFolder;

        public ICommand DeleteFolder

        {
            get
            {
                if (comDeleteFolder == null)
                    comDeleteFolder = new DeleteFolder();
                return comDeleteFolder;
            }
            set
            {
                comDeleteFolder = value;
            }
        }
        #endregion
    }
}

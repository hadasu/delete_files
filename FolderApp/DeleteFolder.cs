using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FolderApp
{
    class DeleteFolder : ICommand
    {
        private ProgressBar winProgressBar;
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (parameter != null && ((PathViewModel)parameter).StringPath != null)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public event EventHandler CanExecuteChanged

        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            ProgressBar winProgressBar = new ProgressBar();
            this.winProgressBar = winProgressBar;
            winProgressBar.DataContext = parameter;
            winProgressBar.Show();
            BackgroundWorker worker = ((PathViewModel)parameter).BackgroundWorker();
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            // winProgressBar.Close();
        }


        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.winProgressBar.Close();
        }
        #endregion
    }
}

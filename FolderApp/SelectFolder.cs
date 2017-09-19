using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace FolderApp
{
    class SelectFolder : ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged

        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                ((PathViewModel)parameter).StringPath = fbd.SelectedPath;
            }
        }

        #endregion
    }
}

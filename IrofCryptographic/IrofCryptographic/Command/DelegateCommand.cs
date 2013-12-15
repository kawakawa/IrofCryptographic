using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IrofCryptographic.Command
{
    public class DelegateCommand : ICommand
    {
        private Action _execute;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action execute)
        {
            //引数がNullの場合例外エラー
            if (execute == null)
            {
                throw new ArgumentNullException("Execute is Null");
            }

            this._execute = execute;
        }


        /// <summary> 
        /// コマンドを実行
        /// </summary> 
        private void Execute()
        {
            if (this._execute != null)
                this._execute();
        }

        /// <summary> 
        /// コマンドが実行可能な状態化どうか問い合わせ 
        /// </summary> 
        /// <returns>実行可能な場合はtrue</returns> 
        private bool CanExecute()
        {
            return true;
        }

        /// <summary>
        /// Executeメソッドに処理を委譲
        /// </summary>
        /// <param name="parameter"></param>
        void ICommand.Execute(object parameter)
        {
            if (this._execute != null)
                this._execute();
        }

        /// <summary>
        /// CanExecuteメソッドに処理を委譲する。 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///  CanExecuteの結果に変更があったことを通知するイベント。 
        ///  WPFのCommandManagerのRequerySuggestedイベントをラップする形で実装
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


    }
}

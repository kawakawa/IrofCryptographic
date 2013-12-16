using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IrofCryptographic.Annotations;

namespace IrofCryptographic.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary> 
        /// プロパティの変更があったときに発行 
        /// </summary> 
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> 
        /// PropertyChangedイベントを発行
        /// </summary> 
        /// <param name="propertyName">プロパティ名</param> 
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var h = this.PropertyChanged;
            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

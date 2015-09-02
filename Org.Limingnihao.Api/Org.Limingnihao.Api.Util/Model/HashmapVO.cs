using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Org.Limingnihao.Api.Util.Model
{
    public class HashmapVO : INotifyPropertyChanged
    {

        private String _key;
        private String _value;

        public HashmapVO() { }

        public HashmapVO(String key, String value)
        {
            this.Key = key;
            this.Value = value;
        }

        public String Key
        {
            get { return _key; }
            set { _key = value; this.OnPropertyChanged("Key"); }
        }

        public String Value
        {
            get { return _value; }
            set { _value = value; this.OnPropertyChanged("Value"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public override string ToString()
        {
            return "[Key=" + this.Key + ", value=" + this.Value + "]";
        }
    }
}

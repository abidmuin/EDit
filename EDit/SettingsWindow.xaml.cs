using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace EDit
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public SettingsWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private string publicKey;
        private string product;
        private string productVersion;
        private string publicKeyID;

        public string PublicKey
        {
            get
            {
                return this.publicKey;
            }
            set
            {
                if (value != this.publicKey)
                {
                    this.publicKey = value;
                    Notify("PublicKey");
                }
            }
        }

        public string Product
        {
            get
            {
                return this.product;
            }
            set
            {
                if (value != this.product)
                {
                    this.product = value;
                    Notify("Product");
                }
            }
        }

        public string ProductVersion
        {
            get
            {
                return this.productVersion;
            }
            set
            {
                if (value != this.productVersion)
                {
                    this.productVersion = value;
                    Notify("ProductVersion");
                }
            }
        }

        public string PublicKeyID
        {
            get
            {
                return this.publicKeyID;
            }
            set
            {
                if (value != this.publicKeyID)
                {
                    this.publicKeyID = value;
                    Notify("PublicKeyID");
                }
            }
        }

        private void bt_OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion INotifyPropertyChanged Members

        private void bt_importPublicKey_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".xml";
            ofd.Filter = EDit.Properties.Resources.XML_File_Type;
            ofd.Title = EDit.Properties.Resources.DialogTitle_SelectPublicKey;
            Nullable<bool> result = ofd.ShowDialog(this);
            if (result == true)
            {
                using (StreamReader sr = File.OpenText(ofd.FileName))
                {
                    this.PublicKey = sr.ReadToEnd();
                }
            }
        }
    }
}
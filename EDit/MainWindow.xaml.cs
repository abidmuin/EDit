using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Win32;

namespace EDit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.publicKey = null;

            this.freeEvent = new EventWaitHandle(true, EventResetMode.ManualReset);
        }

        private string PlainFilePath = "";

        //Selecting Plain file path button
        private void bt_selPlain_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".*";
            ofd.Filter = EDit.Properties.Resources.All_File_Type;
            ofd.Title = EDit.Properties.Resources.DialogTitle_SelectPlain;
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                this.tb_plainFilePath.Text = ofd.FileName;
            }
        }

        private void tb_output_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.tb_output.ScrollToEnd();
        }

        private void bt_encrypt_Click(object sender, RoutedEventArgs e)
        {
            if (!this.freeEvent.WaitOne(0))
            {
                MessageBox.Show(Properties.Resources.Backend_Busy);
                return;
            }

            if (string.IsNullOrEmpty(publicKey))
            {
                MessageBox.Show(this, Properties.Resources.Error_Need_PublicKey);
                return;
            }

            string plainFilePath = this.tb_plainFilePath.Text;
            PlainFilePath = plainFilePath;
            string encryptedFilePath = MakePath(plainFilePath, ".Encrypted");
            string manifestFilePath = MakePath(plainFilePath, ".manifest.xml");

            this.tb_output.Text = Properties.Resources.Out_msg_start_encryption;

            var t = Task.Factory.StartNew(() =>
            {
                freeEvent.Reset();
                string s = EDit.ED.Encrypt(plainFilePath,
                    encryptedFilePath,
                    manifestFilePath,
                    this.publicKey);

                freeEvent.Set();
                this.UpdateOutput(this.tb_output, Properties.Resources.Out_msg_encrypt_success + "\r\n" + s, true);
            });
        }

        private void UpdateOutput(TextBox tb, string message, bool append)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                string newMessage = append ? tb.Text + "\r\n" + message : message;
                tb.Text = newMessage;
            }), null);
        }

        private string publicKey;
        private string privateKey;
        private string manifestFilePath;

        private void bt_setting_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.PublicKey = this.publicKey;
            Nullable<bool> result = sw.ShowDialog();

            if (result == true)
            {
                this.publicKey = sw.PublicKey;
            }
        }

        private static string MakePath(string plainFilePath, string newSuffix)
        {
            string encryptedFileName = Path.GetFileNameWithoutExtension(plainFilePath) + newSuffix;
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(plainFilePath), encryptedFileName);
        }

        private void mi_genKeyPair_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = EDit.Properties.Resources.DialogTitle_CreateKey
            };
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string publicKeyPath = System.IO.Path.Combine(fbd.SelectedPath, "publicKey.xml");
                string privateKeyPath = System.IO.Path.Combine(fbd.SelectedPath, "privateKey.xml");

                string publicKey;
                string privateKey;
                EDit.ED.GenRSAKeyPair(out publicKey, out privateKey);
                using (StreamWriter sw = File.CreateText(publicKeyPath))
                {
                    sw.Write(publicKey);
                }

                using (StreamWriter sw = File.CreateText(privateKeyPath))
                {
                    sw.Write(privateKey);
                }
            }
        }

        private EventWaitHandle freeEvent;

        private void mi_switch_Click(object sender, RoutedEventArgs e)
        {
            if (!this.freeEvent.WaitOne(0))
            {
                MessageBox.Show(Properties.Resources.Backend_Busy);
                return;
            }

            if (this.grid_encrypt.Visibility == System.Windows.Visibility.Visible)
            {
                this.grid_encrypt.Visibility = System.Windows.Visibility.Collapsed;
                this.grid_decrypt.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.grid_encrypt.Visibility = System.Windows.Visibility.Visible;
                this.grid_decrypt.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void bt_selEncrypted_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".*";
            ofd.Filter = EDit.Properties.Resources.All_File_Type;
            ofd.Title = EDit.Properties.Resources.DialogTitle_SelectPlain;
            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {
                this.tb_encryptedFilePath.Text = ofd.FileName;
            }
        }

        private void bt_decrypt_Click(object sender, RoutedEventArgs e)
        {
            if (!this.freeEvent.WaitOne(0))
            {
                MessageBox.Show(Properties.Resources.Backend_Busy);
                return;
            }

            string rsaKey = this.privateKey;
            string encryptedFile = this.tb_encryptedFilePath.Text;
            string plainFile = MakePath(encryptedFile, ".Decrypted") + Path.GetExtension(PlainFilePath);
            string manifestFile = this.manifestFilePath;
            XDocument doc = XDocument.Load(manifestFile);
            XElement aesKeyElement = doc.Root.XPathSelectElement("./DataEncryption/AESEncryptedKeyValue/Key");
            byte[] aesKey = EDit.ED.RSADecryptBytes(Convert.FromBase64String(aesKeyElement.Value), rsaKey);
            XElement aesIvElement = doc.Root.XPathSelectElement("./DataEncryption/AESEncryptedKeyValue/IV");
            byte[] aesIv = EDit.ED.RSADecryptBytes(Convert.FromBase64String(aesIvElement.Value), rsaKey);

            this.tb_outputDecrypt.Text = Properties.Resources.Out_msg_start_decryption;
            var t = Task.Factory.StartNew(() =>
            {
                freeEvent.Reset();
                EDit.ED.DecryptFile(plainFile, encryptedFile, aesKey, aesIv);
                freeEvent.Set();
                this.UpdateOutput(this.tb_outputDecrypt, string.Format(Properties.Resources.Out_msg_decryption_success, plainFile), true);
            });
        }

        private void bt_settingDecrypt_Click(object sender, RoutedEventArgs e)
        {
            DecryptionWindow dsw = new DecryptionWindow();
            dsw.Key = this.privateKey;
            dsw.ManifestFilePath = this.manifestFilePath;
            Nullable<bool> result = dsw.ShowDialog();
            if (result == true)
            {
                this.privateKey = dsw.Key;
                this.manifestFilePath = dsw.ManifestFilePath;
            }
        }

        //private void mi_convertKey_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.DefaultExt = ".pem";
        //    ofd.Filter = EDit.Properties.Resources.PEM_File_Type;
        //    ofd.Title = EDit.Properties.Resources.DialogTitle_SelectPem;
        //    Nullable<bool> result = ofd.ShowDialog();
        //    if (result == true)
        //    {
        //        string pemStr = null;
        //        using (StreamReader sr = File.OpenText(ofd.FileName))
        //        {
        //            pemStr = sr.ReadToEnd().Trim();
        //        }

        //        string publicKey;
        //        string privateKey;
        //        opensslkey.DecodePEMKey(pemStr, out publicKey, out privateKey);

        //        string publicKeyFile = MakePath(ofd.FileName, ".xml");
        //        string privateKeyFile = MakePath(ofd.FileName, ".xml");

        //        if (publicKey != null)
        //        {
        //            using (StreamWriter sw = File.CreateText(publicKeyFile))
        //            {
        //                sw.Write(publicKey);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show(Properties.Resources.Error_convertToPublicKey);
        //        }

        //        if (privateKey != null)
        //        {
        //            using (StreamWriter sw = File.CreateText(privateKeyFile))
        //            {
        //                sw.Write(privateKey);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show(Properties.Resources.Error_convertToPrivateKey);
        //        }
        //    }
        //}

        private void tb_outputDecrypt_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.tb_outputDecrypt.ScrollToEnd();
        }
    }
}
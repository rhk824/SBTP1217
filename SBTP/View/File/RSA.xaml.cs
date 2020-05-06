using System.Security.Cryptography;
using System.Windows;
using SBTP.Data;

namespace SBTP.View.File
{
    /// <summary>
    /// RSA.xaml 的交互逻辑
    /// </summary>
    public partial class RSA : Window
    {
        public RSA()
        {
            InitializeComponent();
        }

        private void Key_Click(object sender, RoutedEventArgs e)
        {
            //RSACryptoServiceProvider rSA = new RSACryptoServiceProvider();
            //private_key.Text = rSA.ToXmlString(true);
            //public_key.Text = rSA.ToXmlString(false);
            private_key.Text = DESHelper.DesEncrypt("2019/9-XJXC-kgujhrbdsfw1446");
            public_key.Text = DESHelper.DesDecrypt(DESHelper.DesEncrypt("2019/9-XJXC-kgujhrbdsfw1446"));

        }
    }
}

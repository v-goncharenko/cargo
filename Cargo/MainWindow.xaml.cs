using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.DirectoryServices.AccountManagement;

namespace Cargo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private cargoDataSet dataSet;
        private FranchiseClass franchise;

        public MainWindow()
        {
            initDataSet();
            franchise = new FranchiseClass(dataSet)
            {
                typeID = 1,
                sizeID = 1,
                insSumID = 1,
                enabled = true
            };

            DataContext = dataSet;
            InitializeComponent();

            // hide it to xml
            insSumCombo.SelectedValue = franchise.insSumID;
            typeCombo.SelectedValue =  franchise.typeID;
            sizeCombo.SelectedValue =  franchise.sizeID;
        }

        private void initDataSet()
        {
            dataSet = new cargoDataSet();
            cargoDataSetTableAdapters.TypesTableAdapter tadapter
                = new cargoDataSetTableAdapters.TypesTableAdapter();
            tadapter.Fill(dataSet.Types);
            tadapter.Dispose();
            cargoDataSetTableAdapters.FranchiseSizesTableAdapter sadapter
                = new cargoDataSetTableAdapters.FranchiseSizesTableAdapter();
            sadapter.Fill(dataSet.FranchiseSizes);
            sadapter.Dispose();
            cargoDataSetTableAdapters.InsSumsTableAdapter isadapter
                = new cargoDataSetTableAdapters.InsSumsTableAdapter();
            isadapter.Fill(dataSet.InsSums);
            isadapter.Dispose();
            cargoDataSetTableAdapters.FranchiseRatesTableAdapter radapter
                = new cargoDataSetTableAdapters.FranchiseRatesTableAdapter();
            radapter.Fill(dataSet.FranchiseRates);
            radapter.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Envirovment.username :" + Environment.UserName + "\nSystem.Security.Principal.WindowsIdentity.GetCurrent().Name :" + System.Security.Principal.WindowsIdentity.GetCurrent().Name +
                "\nUserPrincipal.Current.EmailAddress" + UserPrincipal.Current.EmailAddress);
            double basicRate;
            basicRateText.Text = basicRateText.Text.Replace(".", ",");
            if(double.TryParse(basicRateText.Text, out basicRate)) {
                basicRate /= 100; // because inputed in percents
                franchise.insSumID = (int)insSumCombo.SelectedValue;
                franchise.typeID = (int)typeCombo.SelectedValue;
                franchise.sizeID = (int)sizeCombo.SelectedValue;
                try { franchise.assign(); }
                catch (InvalidOperationException ex) {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                var FR = basicRate * franchise.rate;
                FRLabel.Content = FR.ToString("P3");
                franchiseRateLabel.Content = franchise.rate.ToString("F3");
                resultGroup.Visibility = Visibility.Visible;
            } else {
                MessageBox.Show("Введите корректное число в поле Базовый тариф", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}

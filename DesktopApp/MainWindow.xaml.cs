using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShopBase;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Media3D;
using System.Runtime.CompilerServices;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private MainViewModel? model = null;



        private static Task _pictureShowing = default;

        private static Task _resetAllData = default;
        public MainWindow()
        {
            TestData.CreateDatabaseWithData();

            InitializeComponent();

            model = FindResource("mxmodel") as MainViewModel;

            this.SizeChanged += model.SetWindowStyle;

            this.MouseMove += model.SetHoverStyle;






            //_defaultListBoxFontSize = lbArtList.FontSize;

            //_defaultBtnHeight = bAdd.Height;

            //_defaultBtnWidth = bAdd.Width;

            //_defaultBtnMarginBottom = bAdd.Margin.Bottom;
        }

        // ########################  Begin Article Main Window and Add, Change, Delete and Hidden Dialog ######################## 

        private void lbArtList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            bArticleChange.IsEnabled = lbArtList.SelectedItems.Count == 1 ? true : false;
            bArticleDelete.IsEnabled = lbArtList.SelectedItems.Count > 0 ? true : false;
        }
        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            // Display the hidden ArticleAdd Window and disable all others till new article is added or process cancelled
            tiArticleAdd.Visibility = Visibility.Visible;
            tiArticleAdd.IsSelected = true;

            tiArticle.IsEnabled = false;
            tiCustomer.IsEnabled = false;
            tiOrder.IsEnabled = false;
            tiPicture.IsEnabled = false;


            model.SelArticle = null;
            model.SelArticlePicture = null;

            model.NewArticle ??= new Article();
        }
        private void bAddAdd(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;


            int aId = -1;
            bool isExe = false;


            try
            {
                if (model.NewArticle.Price == 9999.98m) // Is used because of converter and 0 not possible
                {
                    throw new ArgumentException();
                }

                aId = model.NewArticle.Insert();
                model.NewArticle.Id = aId;
                model.LArticle.Add(model.NewArticle);


                if (model != null && model.SelArticlePicture != null)
                {
                    if (model.SelArticlePicture.Data != null)
                    {
                        model.SelArticlePicture.Article = Article.Get(aId);
                        model.SelArticlePicture.Insert();
                    }
                }

                SetMessageLine(false, "Artikel erfolgreich gespeichert");
                isExe = true;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    SetMessageLine(true, "Preis ist nur zwischen 0,01 und 9999,97 gültig");
                }
                else
                {
                    SetMessageLine(true, "Ein unerwarteter Fehler ist aufgetreten");
                }

            }


            if (isExe)
            {
                //Opposite to func b_Add_click
                tiArticleAdd.Visibility = Visibility.Collapsed;
                tiArticleAdd.IsSelected = true;

                tiArticle.IsEnabled = true;
                tiArticle.IsSelected = true;
                tiCustomer.IsEnabled = true;
                tiOrder.IsEnabled = true;
                tiPicture.IsEnabled = true;

                model.NewArticle = null;
            }
        }
        private void bAddCancel(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            //Opposite to func b_Add_click
            tiArticleAdd.Visibility = Visibility.Collapsed;

            tiArticle.IsEnabled = true;
            tiArticle.IsSelected = true;
            tiCustomer.IsEnabled = true;
            tiOrder.IsEnabled = true;
            tiPicture.IsEnabled = true;

            model.NewArticle = null;
            model.SelArticlePicture = null;
        }
        private void bChange_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            Article a = null;

            // Display the hidden ArticleChange Window and disable all others till new article is added or process cancelled
            tiArticleChange.Visibility = Visibility.Visible;
            tiArticleChange.IsSelected = true;

            tiArticle.IsEnabled = false;
            tiCustomer.IsEnabled = false;
            tiOrder.IsEnabled = false;
            tiPicture.IsEnabled = false;

            // Values before user interaction

            a = model.SelArticle;

            tbChangeId.Text = a.Id.ToString();
            tbChangeName.Text = a.Name;
            tbChangeDesc.Text = a.Description;
            tbChangePrice.Text = a.Price.ToString();
        }
        private void bChangeChange(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            // set new values changed by user

            model.SelArticle.Id = Convert.ToInt32(tbChangeId.Text);
            model.SelArticle.Name = tbChangeName.Text;
            model.SelArticle.Description = tbChangeDesc.Text;
            model.SelArticle.Price = Convert.ToDecimal(tbChangePrice.Text);
            model.SelArticle.Change();
            model.LArticle = new System.Collections.ObjectModel.ObservableCollection<Article>(Article.GetAll());


            if (model.SelArticlePicture != null)
            {
                model.SelArticlePicture.Article = model.SelArticle;
                model.SelArticlePicture.Change();
            }

            SetMessageLine(false, "Artikel erfolgreich geändert");

            //Opposite to func b_Add_click
            tiArticleChange.Visibility = Visibility.Collapsed;

            tiArticle.IsEnabled = true;
            tiArticle.IsSelected = true;
            tiCustomer.IsEnabled = true;
            tiOrder.IsEnabled = true;
            tiPicture.IsEnabled = true;
            model.SelArticle = null;
        }
        private void bChangeCancel(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            model.SelArticle = null;
            model.SelArticlePicture = null;

            //Opposite to func bChange_click
            tiArticleChange.Visibility = Visibility.Collapsed;

            tiArticle.IsEnabled = true;
            tiArticle.IsSelected = true;
            tiCustomer.IsEnabled = true;
            tiOrder.IsEnabled = true;
        }
        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            var la = lbArtList.SelectedItems;
            int count = la.Count;

            try
            {

                while (la.Count > 0)
                {
                    Article? a = la[0] as Article;

                    if (a != null)
                    {
                        a.Delete();
                        model.LArticle.Remove(a);
                    }
                }
                if (count > 0)
                {
                    SetMessageLine(false, $"{count} Artikel erfolgreich gelöscht");
                }

            }
            catch (Exception)
            {
                SetMessageLine(true, "Ein unerwarteter Fehler ist aufgetreten");
            }
        }
        private void articleAddPicture_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files | *.jpg;*.JPG;*.png;*.gif";

            if (dlg != null && dlg.ShowDialog() == true)
            {
                Picture p = new Picture(dlg.FileName);

                model.SelArticlePicture = p;
            }
        }


        //************************   Begin Tab Customer and Change hidden                              ************************
        private void CustomerDelete(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            var la = lbCustomer.SelectedItems;
            int count = la.Count;

            try
            {

                while (la.Count > 0)
                {
                    Customer? c = la[0] as Customer;

                    if (c != null)
                    {
                        c.Delete();
                        if (model != null && model.LCustomer != null)
                        {
                            model.LCustomer.Remove(c);
                        }
                    }
                }
                if (count > 0)
                {
                    SetMessageLine(false, la.Count == 1 ? "1 Kunde erfolgreich gelöscht" : $"{count} Kunden erfolgreich gelöscht");
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException && ex.Message.ToLower().Contains("tina"))
                {
                    SetMessageLine(true, "Kunde Tina Test darf nicht gelöscht werden");
                }
                else
                {
                    SetMessageLine(true, "Ein Artikel ist bereits einen Kunden zugeordnet");
                }
            }
        }
        private void CustomerChange(object sender, RoutedEventArgs e)
        {
            model = FindResource("mxmodel") as MainViewModel;
            Customer c = model.SelCustomer as Customer;

            tiCustomerChange.Visibility = Visibility.Visible;
            tiCustomerChange.IsSelected = true;
            tiArticle.IsEnabled = false;
            tiCustomer.IsEnabled = false;
            tiOrder.IsEnabled = false;

            tbCustomerId.Text = c.Id.ToString();
            tbCustomerFirstname.Text = c.Firstname.ToString();
            tbCustomerLastname.Text = c.Lastname.ToString();
            tbCustomerEMail.Text = c.EMail.ToString();
        }
        private void bCustomerChangeChange_Click(object sender, RoutedEventArgs e)
        {
            Customer c = null;
            Random r = new Random();
            string s = default;
            List<char> avChar4Pw = new List<char>() { '!', '#', '$', '%', '/', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'G' };

            try
            {
                if (cbCustomerChangePassword.IsChecked == false)
                {
                    c = new Customer(Convert.ToInt32(tbCustomerId.Text), tbCustomerEMail.Text, tbCustomerFirstname.Text, tbCustomerLastname.Text, model.SelCustomer.Gender, model.SelCustomer.Adress);
                }
                else
                {
                    while (true)
                    {
                        bool hasLetter = false;
                        bool hasDigit = false;
                        bool hasSymbol = false;
                        s = "";

                        while (s.Length < 9)
                        {
                            s += avChar4Pw[r.Next(avChar4Pw.Count - 1)].ToString();
                        }

                        foreach (var item in s)
                        {
                            if (avChar4Pw.IndexOf(item) < 5)
                            {
                                hasSymbol = true;
                            }
                            else if (avChar4Pw.IndexOf(item) < 15)
                            {
                                hasDigit = true;
                            }
                            else
                            {
                                hasLetter = true;
                            }
                        }

                        if (hasSymbol && hasDigit && hasLetter)
                        {
                            break;
                        }

                    }

                    c = new Customer(Convert.ToInt32(tbCustomerId.Text), tbCustomerEMail.Text, Customer.GetHash(s + tbCustomerEMail), tbCustomerFirstname.Text, tbCustomerLastname.Text, model.SelCustomer.Gender, model.SelCustomer.Adress);
                    MessageBoxResult mr = MessageBox.Show(this, $"Passwort {s} wurde generiert in die Zwischenablage kopieren?", "Neues Passwort gesetzt", MessageBoxButton.YesNo);

                    if (mr == MessageBoxResult.Yes)
                    {
                        Clipboard.SetText(s);
                    }

                }

                c.Password = Customer.GetHash(s + c.EMail);

                c.Change();
                model.LCustomer = new System.Collections.ObjectModel.ObservableCollection<Customer>(Customer.GetAll());

                bCustomerChangeCancel(null, null);

                tiCustomer.IsSelected = true;
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException && ex.Message.ToLower().Contains("tina"))
                {
                    SetMessageLine(true, "Kunde Tina Test darf nicht geändert werden");
                }
                else
                {
                    SetMessageLine(true, "Ein unerwarteter Fehler ist aufgetreten");
                }
            }

        }
        private void bCustomerChangeCancel(object sender, RoutedEventArgs e)
        {
            tiCustomerChange.Visibility = Visibility.Collapsed;
            tiArticle.IsEnabled = true;
            tiArticle.IsSelected = true;
            tiCustomer.IsEnabled = true;
            tiOrder.IsEnabled = true;
            tiCustomer.Focus();
            model.SelCustomer = null;
        }



        //''''''''''''''''''''''''   Begin Tab Order                                                   ''''''''''''''''''''''''

        private void bStatusChange(object sender, RoutedEventArgs e)
        {
            Order o = default;
            Status c = default;
            bool statusChangeIsOk = false;

            o = (Order)lbOrder.SelectedItem;
            c = (Status)cbStatusChange.SelectedItem;

            // If Order is Chanceled or Sendet no Change is allowed!

            switch (o.Status)
            {
                case Status.Warenkorb:
                    statusChangeIsOk = c == Status.Bestellt || c == Status.Storniert;
                    break;
                case Status.Bestellt:
                    statusChangeIsOk = c == Status.Versendet || c == Status.Storniert;
                    break;
                default:
                    break;
            }

            if (statusChangeIsOk)
            {
                o.Status = c;
                o.Change();
                model = FindResource("mxmodel") as MainViewModel;
                model.LOrder.RemoveAt(model.LOrder.ToList().FindIndex(item => item.Id == o.Id));
            }
            else
            {
                lOrderHidden.Content = "Die gewählte Kombination ist ungültig!";
                lOrderHidden.Visibility = Visibility.Visible;
            }


        }



        //----------------------     Begin TiPictures                                                 -------------------------

        private void Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded && tiPicture.IsSelected == true)
            {

                Func<Task<bool>> displayList = async () =>
                {
                    while (true)
                    {
                        try
                        {
                            List<Picture> pl = Picture.GetAll();
                            foreach (Picture p in pl)
                            {
                                if (Application.Current != null && Application.Current.Dispatcher != null)
                                {
                                    Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        if (imagesDisplay != null && p != null)
                                        {
                                            imagesDisplay.Source = GetBitmapImage(p);
                                            tiPicturePictureName.Content = p.Filename;
                                        }
                                    });
                                }
                                await Task.Delay(3000);
                            }
                        }
                        catch (Exception)
                        {
                            return false;
                        }

                    }
                };


                if (_pictureShowing == null)
                {
                    _pictureShowing = new Task(() => { displayList(); });

                    _pictureShowing.Start();
                }
            }
        }
        private void bimageToByteCode_Click(object sender, RoutedEventArgs e)
        {
            string data = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files | *.jpg;*.JPG;*.png;*.gif";

            if (dlg != null && dlg.ShowDialog() == true)
            {
                Picture p = new Picture(dlg.FileName);

                if (p != null && p.Data != null)
                {
                    imageToByteCode.Source = GetBitmapImage(p);

                    p.Data.ToList().ForEach(d => data += (d + ","));

                    Clipboard.SetText(data.Remove(data.Length - 1));
                }

            }
        }



        //+++++++++++++++++++++++    Begin Global Functions                                            +++++++++++++++++++++++++ 

        public BitmapImage? GetBitmapImage(Picture p)
        {

            if (p == null)
                return null;

            MemoryStream ms = new MemoryStream();

            BitmapImage bitmap = new BitmapImage();
            ms.Write(p.Data, 0, p.Data.Length);
            ms.Seek(0, SeekOrigin.Begin);
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();
            return bitmap;
        }
        private void SetBackButton_Click(object sender, RoutedEventArgs e)
        {
            Func<Task<bool>> f = async () =>
            {
                model = FindResource("mxmodel") as MainViewModel;
                model.DeLoadData();

                TestData.DeleteAllData();
                TestData.CreateDatabaseWithData();
                model.LoadData();
                await Task.Delay(100);
                SetMessageLine(false, "Daten wurden erfolgreich zurück gesetzt"); // Does not work Foreground is used by other task?
                return true;
            };

            _resetAllData = new Task(() => f());

            _resetAllData.Start();
        }
        private void SetMessageLine(bool isError, string text)
        {
            try
            {
                MessageLine.Foreground = isError ? Brushes.DarkRed : Brushes.DarkGreen;
                MessageLine.Text = text;
            }
            catch (Exception)
            {
            }

        }



        //************************   Begin MainWindow Functions                                             ************************


        private void DynamicFontSizeChange(object sender, SizeChangedEventArgs e)
        {


        }
        private void DynamicFontSizeChange(object sender, EventArgs e)
        {


            //this.Width = this.WindowState == WindowState.Maximized ? SystemParameters.MaximizedPrimaryScreenWidth : _defaultWindowsWidt;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBoxResult.Yes == MessageBox.Show("Beenden bestätigen?", "Beenden?", MessageBoxButton.YesNo, MessageBoxImage.Question))
            {
                _pictureShowing = null;
                _resetAllData = null;
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void mouse_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageLine.Text = null;
        }
        private void bSetBack_MouseEnter(object sender, MouseEventArgs e)
        {
            // Does not work
            //bSetBack.Background = Brushes.Red;
        }
        private void bSetBack_MouseLeave(object sender, MouseEventArgs e)
        {
            //bSetBack.Background = (Brush)new BrushConverter().ConvertFrom("#59FF0000");

        }

        private void bArticleAdd_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void bArticleAdd_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void bArticleAdd_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void title_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }

}

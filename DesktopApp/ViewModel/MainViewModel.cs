using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ShopBase;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DesktopApp
{

    public class MainViewModel : BaseModel
    {
        [System.ComponentModel.Bindable(true)]

        private ObservableCollection<Article> _lAricle = new();

        private ObservableCollection<Customer> _lCustomer = new();

        private ObservableCollection<Order> _lOrder = new();

        private Customer? _selCustomer;

        private Order? _selOrder;

        private Article? _selArticle;

        private Article? _newArticle = new Article();

        private Picture? _selArticlePicture;

        private List<Status> _lStatus = new();

        private Status _selStatus;

        private ObservableCollection<Status>? _lchangeStatus;

        private Status? _changedSelStatus;


        public ObservableCollection<Article> LArticle { get => _lAricle; set { _lAricle = value; OnPropertyChanged(nameof(LArticle)); } }
        public ObservableCollection<Customer> LCustomer { get => _lCustomer; set { _lCustomer = value; OnPropertyChanged(nameof(LCustomer)); } }
        public ObservableCollection<Order> LOrder { get => _lOrder; set { _lOrder = value; OnPropertyChanged(nameof(LOrder)); } }
        public Customer? SelCustomer { get => _selCustomer; set { _selCustomer = value; OnPropertyChanged(nameof(SelCustomer)); if (SelCustomer != null) LOrder = new ObservableCollection<Order>(Order.GetAllFromCustomer(SelCustomer.Id).FindAll(x => x.Status == SelStatus)); OnPropertyChanged(nameof(LCustomer)); } }
        public Order? SelOrder { get => _selOrder; set { _selOrder = value; OnPropertyChanged(nameof(SelOrder)); OnPropertyChanged(nameof(LOrder)); } }
        public Article? SelArticle
        {
            get => _selArticle;
            set
            {
                _selArticle = value; if (value != null) SelArticlePicture = Picture.GetFromArticle(value); OnPropertyChanged(nameof(SelArticle));
                OnPropertyChanged(nameof(LArticle));
            }
        }
        public Picture? SelArticlePicture { get => _selArticlePicture; set { _selArticlePicture = value; OnPropertyChanged(nameof(SelArticlePicture)); OnPropertyChanged(nameof(SelArticle)); } }
        public Article? NewArticle
        {
            get
            {
                if (_newArticle != null && _newArticle.Price == 0)
                {
                    _newArticle.Price = 9999.98m;
                }

                return _newArticle;
            }
            set { _newArticle = value; OnPropertyChanged(nameof(NewArticle)); }
        }
        public List<Status> LStatus { get => _lStatus; private set => _lStatus = value; }
        public Status SelStatus
        {
            get => _selStatus; set
            {
                _selStatus = value;
                if (SelCustomer != null)
                {
                    LChangeStatus = SelStatus == Status.Warenkorb ? new ObservableCollection<Status>() { Status.Bestellt, Status.Storniert } : SelStatus == Status.Bestellt ? new ObservableCollection<Status>() { Status.Versendet, Status.Storniert } : null; LOrder = new ObservableCollection<Order>(Order.GetAllFromCustomer(SelCustomer.Id).FindAll(x => x.Status == SelStatus));
                }
            }
        }
        public ObservableCollection<Status>? LChangeStatus
        {
            get => _lchangeStatus; set
            {
                _lchangeStatus = value; ChangedSelStatus = value != null && (value as ObservableCollection<Status>).Count > 0 ? (value as ObservableCollection<Status>)[0] : null; OnPropertyChanged(nameof(ChangedSelStatus)); OnPropertyChanged(nameof(LChangeStatus));
            }
        }
        public Status? ChangedSelStatus { get => _changedSelStatus; set { _changedSelStatus = value; } }


        private double DeafultTabHeight => 30.0;
        private double DefaultTabWith => 90.0;
        private double DefaultListBoxFontSize => 12.0;
        private double DefaultBtnHeight => 35.0;
        private double DefaultBtnWidth => 140.0;
        private double DefaultBtnMarginBottom => 33.0;
        private double DefaultFont => 14;









        public MainViewModel()
        {

            LoadData();

        }

        public void SetWindowStyle(object sender, SizeChangedEventArgs e)
        {
            MainWindow? mw = sender as MainWindow;

            if (mw != null)
            {
                if (e.PreviousSize.Width != 0.00) // To avoid infinity
                {
                    double factor = (e.NewSize.Width / e.PreviousSize.Width);

                    // Page Article
                    mw.lbArtList.FontSize *= factor;
                    mw.bArticleAdd.Width *= factor;
                    mw.bArticleChange.Width = mw.bArticleAdd.Width;
                    mw.bArticleDelete.Width = mw.bArticleAdd.Width;
                    mw.bSetBack.FontSize *= factor;

                    mw.bArticleAdd.Margin = new(mw.bArticleAdd.Margin.Left, mw.bArticleAdd.Margin.Top, mw.bArticleAdd.Margin.Right, mw.bArticleAdd.Margin.Bottom / factor);
                    mw.bArticleChange.Margin = new(mw.bArticleChange.Margin.Left, mw.bArticleChange.Margin.Top, mw.bArticleChange.Margin.Right, mw.bArticleChange.Margin.Bottom / factor);
                    mw.bArticleDelete.Margin = new(mw.bArticleDelete.Margin.Left, mw.bArticleDelete.Margin.Top, mw.bArticleDelete.Margin.Right, mw.bArticleDelete.Margin.Bottom / factor);

                    //Page Customer
                    mw.lbCustomer.FontSize *= factor;
                    mw.bCustomerChange.FontSize *= factor;
                    mw.bCustomerDelete.FontSize *= factor;
                    mw.bCustomerChange.Width *= factor;
                    mw.bCustomerDelete.Width *= factor;

                    mw.bCustomerChange.Margin = new(mw.bCustomerChange.Margin.Left, mw.bCustomerChange.Margin.Top, mw.bCustomerChange.Margin.Right, mw.bCustomerChange.Margin.Bottom / factor);
                    mw.bCustomerDelete.Margin = new(mw.bCustomerDelete.Margin.Left, mw.bCustomerDelete.Margin.Top, mw.bCustomerDelete.Margin.Right, mw.bCustomerDelete.Margin.Bottom / factor);

                    //Page Order
                    mw.lbOrder.FontSize *= factor;
                    mw.lbCustomerOrder.FontSize *= factor;
                    mw.lOrderSelection.FontSize *= factor;
                    mw.lOrderChange.FontSize *= factor;
                    mw.lOrderChange.Width *= factor;
                    mw.bOrderChange.FontSize *= factor;
                    mw.bOrderChange.Width *= factor;
                    mw.lOrderSelection.Width *= factor;
                    mw.cbStatus.Margin = new(mw.cbStatus.Margin.Left * factor * 0.98, mw.cbStatus.Margin.Top, mw.cbStatus.Margin.Right, mw.cbStatus.Margin.Bottom);
                    mw.cbStatus.Width *= factor;
                    mw.cbStatus.FontSize *= factor;
                    mw.cbStatusChange.Margin = new(mw.cbStatusChange.Margin.Left * factor * 0.98, mw.cbStatusChange.Margin.Top, mw.cbStatusChange.Margin.Right, mw.cbStatusChange.Margin.Bottom);
                    mw.cbStatusChange.Width *= factor;
                    mw.cbStatusChange.FontSize *= factor;
                    mw.bOrderChange.Width *= factor;
                }

                if (e.PreviousSize.Height != 0.00)
                {
                    double factor = (e.NewSize.Height / e.PreviousSize.Height);

                    mw.bArticleAdd.Height *= factor;
                    mw.bArticleChange.Height = mw.bArticleAdd.Height;
                    mw.bArticleDelete.Height = mw.bArticleAdd.Height;
                    mw.bCustomerChange.Height = mw.bArticleAdd.Height;
                    mw.bCustomerDelete.Height = mw.bArticleAdd.Height;
                    mw.bSetBack.Height *= factor;
                    mw.cbStatus.Height *= factor;
                    mw.cbStatusChange.Height *= factor;
                    mw.lOrderSelection.Height *= factor;
                    mw.bOrderChange.Height *= factor;
                    mw.lOrderChange.Height *= factor;
                }
            }





            //if (!this.IsLoaded)
            //    return;

            //double factor = this.Width / _defaultWindowsWidt;

            //lbArtList.FontSize = _defaultListBoxFontSize * Math.Pow(factor, 1.27);

            //lbOrder.FontSize = _defaultListBoxFontSize * Math.Pow(factor, 1.27);

            //bAdd.Height = _defaultBtnHeight * Math.Pow(factor, 1.03);
            //bChange.Height = _defaultBtnHeight * Math.Pow(factor, 1.03);
            //bDelete.Height = _defaultBtnHeight * Math.Pow(factor, 1.03);

            //bAdd.Width = _defaultBtnWidth * Math.Pow(factor, 1.14);
            //bChange.Width = _defaultBtnWidth * Math.Pow(factor, 1.14);
            //bDelete.Width = _defaultBtnWidth * Math.Pow(factor, 1.14);

            //Thickness margin = bAdd.Margin;
            //margin.Bottom = _defaultBtnMarginBottom * Math.Pow(1 / factor, 1.04);
            //bAdd.Margin = margin;

            //margin = bChange.Margin;
            //margin.Bottom = _defaultBtnMarginBottom * Math.Pow(1 / factor, 1.04);
            //bChange.Margin = margin;

            //margin = bDelete.Margin;
            //margin.Bottom = _defaultBtnMarginBottom * Math.Pow(1 / factor, 1.04);
            //bDelete.Margin = margin;

            //bAdd.FontSize = lbArtList.FontSize * 0.70;
            //bChange.FontSize = lbArtList.FontSize * 0.70;
            //bDelete.FontSize = lbArtList.FontSize * 0.70;


            //lbCustomer.FontSize = _defaultListBoxFontSize * Math.Pow(factor, 1.27);

            //bChangeCustomer.Height = _defaultBtnHeight * Math.Pow(factor, 1.03);
            //bDeleteCustomer.Height = _defaultBtnHeight * Math.Pow(factor, 1.03);

            //bChangeCustomer.Width = _defaultBtnWidth * Math.Pow(factor, 1.14);
            //bDeleteCustomer.Width = _defaultBtnWidth * Math.Pow(factor, 1.14);

            //margin = bChangeCustomer.Margin;
            //margin.Bottom = _defaultBtnMarginBottom * Math.Pow(1 / factor, 1.04);
            //bChangeCustomer.Margin = margin;

            //margin = bDeleteCustomer.Margin;
            //margin.Bottom = _defaultBtnMarginBottom * Math.Pow(1 / factor, 1.04);
            //bDeleteCustomer.Margin = margin;

            //bChangeCustomer.FontSize = lbArtList.FontSize * 0.70;
            //bDeleteCustomer.FontSize = lbArtList.FontSize * 0.70;

        }

        public void SetHoverStyle(object sender, MouseEventArgs e)
        {
            Button? b = sender as Button;

            if (b != null)
            {
                b.Background = b.Background == Brushes.White ? Brushes.LightGreen : Brushes.White;
            }
        }



        public void LoadData()
        {
            LArticle = new ObservableCollection<Article>(Article.GetAll().Where(item => item.Active));
            LCustomer = new ObservableCollection<Customer>(Customer.GetAll());
            SelCustomer = LCustomer[0];
            LOrder = new ObservableCollection<Order>(Order.GetAllFromCustomer(SelCustomer.Id).FindAll(x => x.Status == Status.Warenkorb));
            LStatus = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            SelStatus = Status.Warenkorb;
        }
        public void DeLoadData()
        {
            LArticle = new();
            LCustomer = new();
            SelCustomer = new();
            LOrder = new();
            LStatus = new();
        }

    }



}


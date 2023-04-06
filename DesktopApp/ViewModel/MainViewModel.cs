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

namespace DesktopApp
{

    public class MainViewModel : BaseModel
    {
        [System.ComponentModel.Bindable(true)]
        public System.Collections.IEnumerable ItemsSource { get; set; }

        private ObservableCollection<Article> _lAricle;

        private ObservableCollection<Customer> _lCustomer;

        private ObservableCollection<Order> _lOrder;

        private Customer _selCustomer;

        private Order _selOrder;

        private Article _selArticle;

        private Article _newArticle = new Article();

        private Picture _selArticlePicture;

        private List<Status> _lStatus;

        private Status _selStatus;

        private ObservableCollection<Status>? _lchangeStatus;

        private Status? _changedSelStatus;



        public ObservableCollection<Article> LArticle { get => _lAricle; set { _lAricle = value; OnPropertyChanged(nameof(LArticle)); } }
        public ObservableCollection<Customer> LCustomer { get => _lCustomer; set { _lCustomer = value; OnPropertyChanged(nameof(LCustomer)); } }
        public ObservableCollection<Order> LOrder { get => _lOrder; set { _lOrder = value; OnPropertyChanged(nameof(LOrder)); } }
        public Customer SelCustomer { get => _selCustomer; set { _selCustomer = value; OnPropertyChanged(nameof(SelCustomer)); if (SelCustomer != null) LOrder = new ObservableCollection<Order>(Order.GetAllFromCustomer(SelCustomer.Id).FindAll(x => x.Status == SelStatus)); OnPropertyChanged(nameof(LCustomer)); } }
        public Order SelOrder { get => _selOrder; set { _selOrder = value; OnPropertyChanged(nameof(SelOrder)); OnPropertyChanged(nameof(LOrder)); } }
        public Article SelArticle
        {
            get => _selArticle;
            set
            {
                _selArticle = value; if (value != null) SelArticlePicture = Picture.GetFromArticle(value); OnPropertyChanged(nameof(SelArticle));
                OnPropertyChanged(nameof(LArticle));
            }
        }
        public Picture SelArticlePicture { get => _selArticlePicture; set { _selArticlePicture = value; OnPropertyChanged(nameof(SelArticlePicture)); OnPropertyChanged(nameof(SelArticle)); } }
        public Article NewArticle
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
        public Status SelStatus { get => _selStatus; set { _selStatus = value; 
                LChangeStatus = SelStatus == Status.Warenkorb ? new ObservableCollection<Status>() { Status.Bestellt, Status.Storniert } : SelStatus == Status.Bestellt ? new ObservableCollection<Status>() { Status.Versendet, Status.Storniert } : null; LOrder = new ObservableCollection<Order>(Order.GetAllFromCustomer(SelCustomer.Id).FindAll(x => x.Status == SelStatus)); } }
        public ObservableCollection<Status>? LChangeStatus { get => _lchangeStatus; set {
                _lchangeStatus = value; ChangedSelStatus = value != null && (value as ObservableCollection<Status>).Count > 0 ? (value as ObservableCollection<Status>)[0] : null; OnPropertyChanged(nameof(ChangedSelStatus)); OnPropertyChanged(nameof(LChangeStatus)); } }
        public Status? ChangedSelStatus { get => _changedSelStatus; set { _changedSelStatus = value;  } }
        public MainViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            LArticle = new ObservableCollection<Article>(Article.GetAll());
            LCustomer = new ObservableCollection<Customer>(Customer.GetAll());
            SelCustomer = LCustomer[0];
            LOrder = new ObservableCollection<Order>(Order.GetAllFromCustomer(SelCustomer.Id).FindAll(x => x.Status == Status.Warenkorb));
            LStatus = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            SelStatus = Status.Warenkorb;
        }
        public void DeLoadData()
        {
            LArticle = null;
            LCustomer = null;
            SelCustomer = null;
            LOrder = null;
            LStatus = null;
        }

    }



}


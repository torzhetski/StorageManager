using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Storage_Manager.MVVM.Model;
using static Storage_Manager.MVVM.Model.Company;
using static Storage_Manager.MVVM.Model.Detail;
using static Storage_Manager.MVVM.Model.Material;



namespace Storage_Manager.MVVM.ViewModel
{
    class BuyMaterialsVM : ObservableObject
    {
        private ObservableCollection<Company> _companies;
        private ObservableCollection<Material> _materials;
        private ObservableCollection<Detail> _details;
        private ObservableCollection<Detail> _purchizedDetails;
        private Company _selectedCompany;
        private Detail _selectedDetail;
        private bool _isPopupOPen;

        public ObservableCollection<Company> Companies 
        { 
            get { return _companies; } 
        }
        public ObservableCollection<Material> Materials
        {
            get => _materials;
            set => Set(ref _materials, value);
        }
        public ObservableCollection<Detail> Details
        {
            get => _details;
            set => Set(ref _details, value);
        }
        public ObservableCollection<Detail> PurchizedDetails
        {
            get => _purchizedDetails;
            set => Set(ref _purchizedDetails, value);
        }
        public Company SelectedCompany 
        {
            get => _selectedCompany;
            set 
            { 
                Set(ref _selectedCompany, value);
                OnPropertyChanged(nameof(SelectedCompany),value,value);
            }
        }
        public Detail SelectedDetail
        {
            get => _selectedDetail;
            set => Set(ref _selectedDetail, value);
        }
        public bool IsPopupOpen
        {
            get => _isPopupOPen;
            set => Set(ref _isPopupOPen, value);
        }

        public RelayCommand AddMaterilasCommand {  get; set; }
        public RelayCommand PurchaizeCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public BuyMaterialsVM() 
        {
            _companies = GetCompanies();
            _materials = GetMaterials();
            _details = GetDetails();
            _purchizedDetails = GetDetails();
            foreach (var e in _purchizedDetails)
                e.Size = 0;
            AddMaterilasCommand = new RelayCommand(o => 
            {
                if(o!=null)
                    BuyMaterials(Materials, (o as Company).Materials);
                else 
                    IsPopupOpen = true;
            });
            CancelCommand = new RelayCommand(o =>
            {
                SelectedCompany = null;
                SelectedDetail = null;
            });
            PurchaizeCommand = new RelayCommand(o =>
            {
                PurchizingDetails(Details,PurchizedDetails);
            });
        }
        protected override void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnPropertyChanged(propertyName, oldValue, newValue);
            IsPopupOpen = false;
            RaisePropertyChanged(nameof(IsPopupOpen));
        }
    }
}

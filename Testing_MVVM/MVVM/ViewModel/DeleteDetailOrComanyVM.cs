using Storage_Manager.MVVM.Model;
using static Storage_Manager.MVVM.Model.Company;
using static Storage_Manager.MVVM.Model.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Storage_Manager.MVVM.ViewModel
{
    class DeleteDetailOrComanyVM : ObservableObject
    {
        private ObservableCollection<Detail> _details;
        private ObservableCollection<Company> _companies;

        public ObservableCollection<Detail> Details
        {
            get => _details;
            set => Set(ref _details, value);
        }
        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set => Set(ref _companies, value);
        }

        public RelayCommand DeleteDetailCommand { get; set; }
        public RelayCommand DeleteCompanyCommand { get; set; }

        public DeleteDetailOrComanyVM()
        {
            Details = GetDetails();
            Companies = GetCompanies();
            DeleteDetailCommand = new(o =>
            {
                DeleteDetail(o as Detail, Details);
            }, o => o != null);
            DeleteCompanyCommand = new(o =>
            {
                DeleteCompany(o as Company, Companies);
            }, o => o!=null);

        }
    }
}

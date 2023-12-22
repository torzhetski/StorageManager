using Storage_Manager.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Storage_Manager.MVVM.Model.DetailScheme;
using static Storage_Manager.MVVM.Model.Material;
using static Storage_Manager.MVVM.Model.Detail;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;

namespace Storage_Manager.MVVM.ViewModel
{
    class ProduceDetailsVM : ObservableObject
    {
        private ObservableCollection<DetailScheme> _detailsScheme;
        private ObservableCollection<Material> _materials;
        private ObservableCollection<Detail> _details;
        private DetailScheme _selectedScheme;
        private string _info;
        private bool _isCheked = false;
        private bool _isPopupOpen = false;
        private bool _isPopup2Open = false;



        public ObservableCollection<DetailScheme> DetailsScheme
        {
            get => _detailsScheme;
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
        public DetailScheme SelectedScheme
        {
            get => _selectedScheme;
            set => Set(ref _selectedScheme, value);          
        }
        public string Info 
        {
            get => _info;
            set => Set(ref _info, value);
        }
        public bool IsCheked 
        {
            get => _isCheked;
            set => Set(ref _isCheked, value);
        }
        public bool IsPopupOpen 
        {
            get => _isPopupOpen;
            set => Set (ref _isPopupOpen, value);
        }
        public bool IsPopup2Open
        {
            get => _isPopup2Open;
            set => Set(ref _isPopup2Open, value);
        }
        public RelayCommand ProduceCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public ProduceDetailsVM () 
        {
            _detailsScheme = GetDetailsScheme();
            _materials = GetMaterials();
            _details = GetDetails();
            ProduceCommand = new RelayCommand(o => 
            {
                uint n = 10;
                if (!IsCheked)
                {
                    GetMaterialsAfterBatch(Materials, (o as DetailScheme).Materials, n);
                    AmountOfDetailsAfterBatch(Details, (o as DetailScheme).Name, n);
                    
                }
                else 
                {
                    n = 5;
                    GetMaterialsAfterBatch(Materials, (o as DetailScheme).Materials, n);
                    AmountOfDetailsAfterBatch(Details, (o as DetailScheme).Name, n);
                }
            
            }, 
            o => 
            {
                if (o == null)
                {
                    Info = "Scheme is not selected";
                    IsPopupOpen = true;
                }
                else if (!CanBeProduced(Materials, (o as DetailScheme).Materials, 10))
                {
                    Info = "There is no reqired materials";
                    IsPopup2Open = true;
                }
                else
                {
                    IsPopupOpen = false;
                    IsPopup2Open = false;
                }
                return o != null && CanBeProduced(Materials, (o as DetailScheme).Materials, 10);
            });
            CancelCommand = new RelayCommand(o => 
            {
                IsCheked = false;
                SelectedScheme = null;
            });
            
        }
    }
}

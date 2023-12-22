using Storage_Manager.MVVM.Model;
using System;
using static Storage_Manager.MVVM.Model.Material;
using static Storage_Manager.MVVM.Model.Company;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage_Manager.MVVM.ViewModel
{
    class AddCompanyVM : ObservableObject
    {
        private string _nameOfCompany;
        private Material _selectedMaterial;
        private ObservableCollection<Material> _materials;

        public string NameOfCompany
        {
            get => _nameOfCompany;
            set => Set(ref _nameOfCompany, value);
        }
        public Material SelectedMaterial
        {
            get => _selectedMaterial;
            set => Set(ref _selectedMaterial, value);
        }
        public ObservableCollection<Material> Materials
        {
            get => _materials;
            set => Set(ref _materials, value);
        }

        public RelayCommand AddCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public AddCompanyVM()
        {
            Materials = GetMaterials();
            foreach (var material in Materials)
                material.Amount = null;
            AddCommand = new(o =>
            {
                if(AddCompany(NameOfCompany, Materials))
                {
                    NameOfCompany = default(string);
                    SelectedMaterial = null;
                    foreach (var material in Materials)
                        material.Amount = null;
                }
            });
            CancelCommand = new(o =>
            {
                NameOfCompany = default(string);
                SelectedMaterial = null;
                foreach (var material in Materials)
                    material.Amount = null;
            });
        }
    }
}

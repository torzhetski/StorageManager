using Storage_Manager.MVVM.Model;
using static Storage_Manager.MVVM.Model.Material;
using static Storage_Manager.MVVM.Model.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using System.ComponentModel;

namespace Storage_Manager.MVVM.ViewModel
{
    class AddDetailVM : ObservableObject
    {
        private string _nameOfDetail;
        private uint? _size ;
        private Material _selectedMaterial;
        private ObservableCollection<Material> _materials;

        public string NameOfDetail 
        {
            get => _nameOfDetail;
            set => Set(ref _nameOfDetail, value);
        }
        public uint? Size
        {
            get => _size;
            set => Set(ref _size, value);
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


        
        public AddDetailVM()
        {
            Materials = GetMaterials();
            foreach(var material in Materials) 
                material.Amount = null;
            AddCommand = new(o =>
            {
               if( AddDetail(NameOfDetail,Size, Materials))
                {
                    NameOfDetail = default(string);
                    Size = default(uint?);
                    SelectedMaterial = null;
                    foreach (var material in Materials)
                        material.Amount = null;
                }
            });
            CancelCommand = new(o =>
            {
                NameOfDetail = default(string);
                Size = default(uint?);
                SelectedMaterial = null;
                foreach (var material in Materials)
                    material.Amount = null;
            });
        }
        
    }
}

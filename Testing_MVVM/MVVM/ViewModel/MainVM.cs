using Storage_Manager.MVVM.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Storage_Manager.MVVM.ViewModel
{
    class MainVM : ObservableObject
    {
        public RelayCommand NextPageCommand { get; set; }

        private object _currentView = new ProduceDetailsVM();

        public object CurrentView
        {
            get { return _currentView; }
            set => Set(ref _currentView, value);
        }
        public MainVM()
        {          
            this.NextPageCommand = new RelayCommand(ExecuteNextPageCommand);  
        }
        private void ExecuteNextPageCommand(object commandParameter)
        {
            if (commandParameter is not ID pageId)
            {
                throw new ArgumentException($"Unexpected parameter type. ICommand.CommandParameter must be of type {typeof(ID)}", nameof(commandParameter));
            }

            LoadPage(pageId);
        }
        private void LoadPage(ID pageId)
        {
            object oldPage = this.CurrentView;

            SavePageData(oldPage);
            switch (pageId)
            {
                case ID.DetailsProduce:
                    this.CurrentView = new ProduceDetailsVM();
                    break;
                case ID.MaterialsBuy:
                    this.CurrentView = new BuyMaterialsVM();
                    break;
                case ID.AddDetail:
                    this.CurrentView = new AddDetailVM();
                    break;
                case ID.AddCompany:
                    this.CurrentView = new AddCompanyVM();
                    break;
                case ID.DeleteDetailOrCompany:
                    this.CurrentView = new DeleteDetailOrComanyVM();
                    break;
            }
        }
       
        private void SavePageData(object page)
        {
            if (page is ProduceDetailsVM)
            {
                Detail.SaveDetails((page as ProduceDetailsVM).Details);
                Material.SaveMaterials((page as ProduceDetailsVM).Materials);
            }
            if (page is BuyMaterialsVM)
            {
                Detail.SaveDetails((page as BuyMaterialsVM).Details);
                Material.SaveMaterials((page as BuyMaterialsVM).Materials);
            }
        }
    }
}

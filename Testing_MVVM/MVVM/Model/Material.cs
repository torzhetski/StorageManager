using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage_Manager.MVVM.Model
{
    class Material : ObservableObject
    {
        private const string _path = "Materials.json";
        private string _name;
        private uint? _amount;

        public uint? Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public Material(string name, uint amount)
        {
            _name = name;
            _amount = amount;
        }
        /// <summary>
        /// Получить список материалов необходимых для детали
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static ObservableCollection<Material> GetAmountMaterial(DetailScheme detail)
        {
            return detail.Materials;
        }
        /// <summary>
        /// Получить из файла доступные материалы
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ObservableCollection<Material> GetMaterials()
        {
            ObservableCollection<Material> materials ;
            if (!File.Exists(_path))
                File.Create(_path).Close();
            using (StreamReader sr = new StreamReader(_path))
            {
                materials = JsonConvert.DeserializeObject<ObservableCollection<Material>>(sr.ReadToEnd());
            }
            return materials;
        }
        public static void GetMaterialsAfterBatch(ObservableCollection<Material> allMaterials, ObservableCollection<Material> requiredMaterials, uint n)
        {
            double coefficient = 0;
            if (n == 5)
                coefficient = 0.1;
            foreach (var material in allMaterials)
            {
                foreach (var req in requiredMaterials)
                {
                    if (material.Name == req.Name)
                        material.Amount -=  Convert.ToUInt32(n * (req.Amount + coefficient * req.Amount));
                }
            }
        }
        public static void BuyMaterials(ObservableCollection<Material> allMaterials, ObservableCollection<Material> addedMaterials)
        {

            foreach (var material in allMaterials)
            {
                foreach (var add in addedMaterials)
                {
                    if (material.Name == add.Name)
                    {
                        material.Amount += add.Amount;
                        break;
                    }
                }
            }

        }
        public static bool CanBeProduced(ObservableCollection<Material> allMaterials, ObservableCollection<Material> requiredMaterials, int n)
        {
            bool flag = true;
            foreach (var material in allMaterials)
                foreach (var req in requiredMaterials)
                    if (material.Name == req.Name)
                        if (material.Amount < n * req.Amount)
                            flag = false;
            return flag;
        }
        public static void SaveMaterials(ObservableCollection<Material> materials)
        {
            using (StreamWriter sw = new StreamWriter(_path))
            {
                sw.WriteLine(JsonConvert.SerializeObject(materials));
            }
        }
        public override string ToString()
        {
            return Name + $" : {Amount}";
        }

    }
}

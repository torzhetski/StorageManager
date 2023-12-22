using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Storage_Manager.MVVM.Model
{
    class DetailScheme:ObservableObject
    {
        private const string _path = "DetailsScheme.json";
        private ObservableCollection<Material> _materials;
        private string _name;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        public ObservableCollection<Material> Materials
        {
            get => _materials;
            set => Set(ref _materials, value);
        }

        public DetailScheme(string name, ObservableCollection<Material> materials)
        {
            _name = name;
            _materials = materials;
        }

        public static ObservableCollection<DetailScheme> GetDetailsScheme()
        {
            ObservableCollection<DetailScheme> details = new();
            if (!File.Exists(_path))
                File.Create(_path).Close();
            using (StreamReader sr = new StreamReader(_path))
            {
                //details = JsonConvert.DeserializeObject<ObservableCollection<DetailScheme>>(sr.ReadToEnd());
                while (!sr.EndOfStream)
                {
                    DetailScheme detail = JsonConvert.DeserializeObject<DetailScheme>(sr.ReadLine());
                    details.Add(detail);
                }

            }
            return details;
        }
        public static bool AddDetailScheme(string name, ObservableCollection<Material> materials)
        {
            try
            {
                for (int i = 0; i < materials.Count; i++)
                    if (materials[i].Amount == null || materials[i].Amount == 0)
                    {
                        materials.Remove(materials[i]);
                        i--;
                    }
                if (materials.Count == 0) throw new IndexOutOfRangeException();
                DetailScheme detailScheme = new DetailScheme(name, materials);
                using (StreamWriter sw = new StreamWriter(_path, true))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(detailScheme));
                }
                return true;
            }
            catch (IndexOutOfRangeException ex) 
            {
                return false;
            }
        }
        public static void DeleteDetailscheme(string name)
        {
            var collection = GetDetailsScheme();
            for (int i = 0; i < collection.Count; i++)
                if (collection[i].Name == name)
                    collection.RemoveAt(i);
            using (StreamWriter sw = new StreamWriter(_path))
            {
                foreach (var e in collection)
                    sw.WriteLine(JsonConvert.SerializeObject(e));
            }
        }
        private string GetStringMaterials()
        {
            string str = "";
            foreach (var e in _materials)
            {
                str += e.ToString() + "   ";
            }
            return str;
        }
        /// <summary>
        /// Выводит строку из материалов необходимых для изготовления
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetStringMaterials();
        }

    }

}

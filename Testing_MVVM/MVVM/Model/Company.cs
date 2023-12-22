using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Storage_Manager.MVVM.Model
{
    class Company : ObservableObject
    {
        private const string _path = "Companies.json";
        private string _name;
        private ObservableCollection<Material> _materials;

        public string Name 
        { 
            get { return _name; } 
            set { _name = value; }
        }
        public ObservableCollection<Material> Materials
        { 
            get { return _materials; } 
            set { _materials = value; }
        }
        public Company( string name, ObservableCollection<Material> materials) 
        {
            _materials = materials;
            _name = name;   
        }
        /// <summary>
        /// получить список компаний
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<Company> GetCompanies() 
        {
            ObservableCollection<Company> companies = new();
            if (!File.Exists(_path))
                File.Create(_path).Close();
            using (StreamReader sr = new StreamReader(_path))
            {
                
                {
                    
                        while (!sr.EndOfStream)
                        {
                            Company company = JsonConvert.DeserializeObject<Company>(sr.ReadLine());
                            companies.Add(company);
                        }
                    
                   
                }
            }
            return companies;
        }
        public static bool AddCompany(string name, ObservableCollection<Material> materials)
        {
            try
            {
                if (name == null)  throw new ArgumentNullException();
                for (int i = 0; i < materials.Count; i++)
                    if (materials[i].Amount == null || materials[i].Amount == 0)
                    {
                        materials.Remove(materials[i]);
                        i--;
                    }
                if (materials.Count == 0) throw new IndexOutOfRangeException();
                Company company = new Company(name, materials);
                using (StreamWriter sw = new StreamWriter(_path, true))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(company));
                }
                MessageBox.Show("Successfully added");
                return true;
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("You need to add any materials amount");
                return false;
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Name cant be empty");
                return false;
            }
        }
        public static void DeleteCompany( Company company, ObservableCollection<Company> companies)
        {
            companies.Remove(company);
            using (StreamWriter sw = new StreamWriter(_path))
            {
                foreach (var e in companies)
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
        public override string ToString()
        {
            return GetStringMaterials();
        }
    }
}

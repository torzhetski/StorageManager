using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Storage_Manager.MVVM.Model
{
    class Detail : ObservableObject
    {
        private const string _path = "Details.json";
        private string _name;
        private uint _amount;
        private uint _size;
        
        public string Name
        {
            get => _name;
            set { _name = value; }
        }
        public uint Amount
        {
            get => _amount;
            set => Set(ref _amount, value);
        }
        public uint Size
        {
            get => _size; 
            set => Set (ref _size, value);
        }
 
        public Detail(uint size, string name, uint amount)
        {
            _amount = amount;
            _name = name;
            _size = size;
        }
        public static ObservableCollection<Detail>GetDetails()
        {
            ObservableCollection<Detail> details = new();
            if (!File.Exists(_path))
                File.Create(_path).Close();
            using (StreamReader sr = new StreamReader(_path))
            {
                
                while (!sr.EndOfStream)
                {
                    Detail detail = JsonConvert.DeserializeObject<Detail>(sr.ReadLine());
                    details.Add(detail);
                }
            }
            return details;
        }
        public static void AmountOfDetailsAfterBatch(ObservableCollection<Detail> allDetails, string detailName, uint n)
        {
            foreach (Detail detail in allDetails) 
                if(detail.Name == detailName)
                    detail.Amount += n;
            

        }
        public static void SaveDetails(ObservableCollection<Detail> details)
        {
            using (StreamWriter sw = new StreamWriter(_path)) 
            { 
                foreach(Detail detail in details)
                sw.WriteLine(JsonConvert.SerializeObject(detail));
            }
        }
        public static void PurchizingDetails(ObservableCollection<Detail> details , ObservableCollection<Detail> purchizedDetails)
        {
            uint sum = 0;
            uint max = 1000;
            foreach (var item in purchizedDetails)
                sum += item.Size;
            if (sum <= 0.2 * max)
                MessageBox.Show("You need to purchize minimum 20% of storage :)");
            else
                for (int i = 0; i < details.Count; i++)
                    if ((sum = purchizedDetails[i].Size / details[i].Size) > details[i].Amount)
                        MessageBox.Show($"there is no that many {details[i].Name} on our storage");
                    else
                    {
                        details[i].Amount -= sum;
                        MessageBox.Show("Details purchized");
                    }
        }
        public static bool AddDetail(string name, uint? size, ObservableCollection<Material> materials)
        {
            try
            {
                if (name == null || size == null) throw new ArgumentNullException();
                Detail detail = new Detail(Convert.ToUInt32(size), name, 0);
                if (DetailScheme.AddDetailScheme(name, materials))
                    using (StreamWriter sw = new StreamWriter(_path, true))
                    {
                        sw.WriteLine(JsonConvert.SerializeObject(detail));
                    }
                else
                {
                    throw new IndexOutOfRangeException();
                }
                MessageBox.Show("Successfully added");
                return true;
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("You need to add any materials cost");
                return false;
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Name and size cant be empty");
                return false;

            }

        }
        public static void DeleteDetail (Detail detail, ObservableCollection<Detail> details)
        {
            details.Remove(detail);
            SaveDetails(details);
            DetailScheme.DeleteDetailscheme(detail.Name);
        }
        public override string ToString()
        {
            return Name + $" : {Amount}";
        }
    }
}

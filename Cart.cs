using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2025._04._03._cukrászda
{
    public class Cart
    {
        public static ObservableCollection<Cake> cart = new ObservableCollection<Cake>();   // azért ObservableCollection, hogy a UI automatikusan tudjon frissülni.
        public static ObservableCollection<Cake> delivery = new ObservableCollection<Cake>();
    }
}

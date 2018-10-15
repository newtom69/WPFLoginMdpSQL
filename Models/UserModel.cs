using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Ressources.Models
{
    public class UserModel : BindableBase
    {
        private string _Name;

        public string Name { get => _Name; set => _Name = value; }
    }
}

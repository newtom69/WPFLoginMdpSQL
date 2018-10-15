using Prism.Mvvm;
using Ressources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ressources.ViewModel
{
    class UserViewModel : BindableBase
    {
        private UserModel _UserModelData;

        public UserModel UserModelData
        {
            get => _UserModelData;
            set => SetProperty(ref _UserModelData, value);
        }

        public UserViewModel()
        {
            _UserModelData = new UserModel();
            _UserModelData.Prenom = "Thomas";
            _UserModelData.Nom = "Vuille";
            _UserModelData.DateNaissance = DateTime.Parse("1978-11-13");
        }
    }
}

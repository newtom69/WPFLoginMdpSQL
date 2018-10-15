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
        private string _Nom;
        private string _Prenom;
        private DateTime _DateNaissance;

        public string Nom
        {
            get => _Nom;
            set => SetProperty(ref _Nom, value);
        }

        public string Prenom
        {
            get => _Prenom;
            set => SetProperty(ref _Prenom, value);
        }

        public DateTime DateNaissance
        {
            get => _DateNaissance;
            set => SetProperty(ref _DateNaissance, value);
        }

        public int Age => (DateTime.Today - _DateNaissance).Days / 365;

        public string PrenomNomAge => $"{Prenom} {Nom} {Age} ans";
    }
}

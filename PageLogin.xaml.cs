using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using static System.Console;
using System.Security.Cryptography;




namespace Ressources
{
    /// <summary>
    /// Logique d'interaction pour PageLogin.xaml
    /// </summary>
    public partial class PageLogin : Window
    {
        public PageLogin()
        {
            InitializeComponent();
            string prenomAuteur1 = ConfigurationManager.AppSettings["prenom1"];
            string prenomAuteur2 = ConfigurationManager.AppSettings["prenom2"];
            string prenomAuteur3 = ConfigurationManager.AppSettings["prenom3"];
            MessageBox.Show($"Programme écrit par {prenomAuteur1} {prenomAuteur2} {prenomAuteur3}");
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if ((e.Source as TextBox).Text == "Login") (e.Source as TextBox).Text = "";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            switch ((e.Source as TextBox).Name)
            {
                case "Login":
                    if ((e.Source as TextBox).Text == "")
                    {
                        (e.Source as TextBox).Text = "Login";
                    }
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login;
            int idPersonne;
            string nomPersonne;
            string prenomPersonne;
            string mail;
            DateTime dateNaissance;
            int age;
            string mdpHash;

            (sender as Button).Content = "Connexion en cours...";

            using (SHA256 sha256Hash = SHA256.Create())
                mdpHash = GetHash(sha256Hash, Mdp.Password);

            try
            {
                using (SqlConnection connection = new SqlConnection())
                {

                    ConnectionStringSettings connex = ConfigurationManager.ConnectionStrings["ServeurTestUser"];
                    connection.ConnectionString = connex.ConnectionString;
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = " SELECT id, Login, Nom, Prenom, DateNaissance, Mail" +
                                              " FROM GodwinWPF1" +
                                              $" WHERE Login = '{Login.Text}' and Mdp = '{mdpHash}'";


                        SqlDataReader reader = command.ExecuteReader();
                        bool mdpOk = false;
                        while (reader.Read())
                        {
                            mdpOk = true;   
                            idPersonne = (int)reader["id"];
                            login = (string)reader["Login"];
                            nomPersonne = (string)reader["Nom"]; ;
                            prenomPersonne = (string)reader["Prenom"]; ;
                            mail = (string)reader["Mail"]; ;
                            dateNaissance = (DateTime)reader["DateNaissance"];
                            var nbjours = DateTime.Today - dateNaissance;
                            age = nbjours.Days/365;
                            MessageBox.Show("Vos informations :\n" +
                                $"Nom : {nomPersonne}\nPrénom : {prenomPersonne}\nmail : {mail}\nVotre age : {age} ans","Connexion Réussie");
                        }
                        if (!mdpOk)
                        {
                            MessageBox.Show("Utilisateur ou mot de passe incorrect");

                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données");
            }
            catch (Exception ex)
            {
                //WriteLine("Erreur non SQL");
                //WriteLine(e.Message);
            }
            //finally
            //{

            //}

            (sender as Button).Content = "Se connecter";
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

    }
}
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Configuration;
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

        private void Connexion(object sender, RoutedEventArgs e)
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

            using (SHA256 Hash = SHA256.Create())
                mdpHash = GetHash(Hash, Mdp.Password);

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
                            age = (DateTime.Today - dateNaissance).Days / 365;
                            MessageBox.Show("Vos informations :\n" +
                                            $"Nom : {nomPersonne}\n" +
                                            $"Prénom : {prenomPersonne}\n" +
                                            $"mail : {mail}\n" +
                                            $"Votre age : {age} ans"
                                ,"Connexion Réussie");
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
                MessageBox.Show("Erreur générique non SQL dans le programme");
            }
            finally
            {
                (sender as Button).Content = "Se connecter";
            }
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

        private void ToucheClavier(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Connexion(BoutonConnexion, null);
            }
        }
    }
}
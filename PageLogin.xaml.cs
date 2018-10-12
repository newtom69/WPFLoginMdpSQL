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
                default:
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("connexion en cours");
            string login;
            string mdp;
            int idPersonne;
            string nomPersonne;
            string prenomPersonne;
            string mail;
            DateTime dateNaissance;
            int age;

            try
            {
                using (SqlConnection connection = new SqlConnection())
                {

                    ConnectionStringSettings connex = ConfigurationManager.ConnectionStrings["ServeurTestUser"];
                    connection.ConnectionString = connex.ConnectionString;
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = " SELECT id" +
                                              " FROM GodwinWPF1" +
                                              $" WHERE Login = '{Login.Text}' and Mdp = 'toto'";

                        object retourExecScalar = command.ExecuteScalar();
                        if (retourExecScalar != null)
                        {
                            idPersonne = (int)retourExecScalar;
                            MessageBox.Show("Connexion réussie");
                        }
                        else
                        {
                            MessageBox.Show("Utilisateur ou mot de passe incorrect");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                //WriteLine("Erreur SQL");
                //WriteLine(e.Message);

            }
            catch (Exception ex)
            {
                //WriteLine("Erreur non SQL");
                //WriteLine(e.Message);
            }
            //finally
            //{

            //}
        }
    }
}
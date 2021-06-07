using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;
        private SqlConnection sqlConnection1;

        public Form1()
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["Matthieu"].ConnectionString;
            sqlConnection = new SqlConnection(connectionstring);


            string connectionstring1 = ConfigurationManager.ConnectionStrings["Matthieu"].ConnectionString;
            sqlConnection1 = new SqlConnection(connectionstring);

            InitializeComponent();

            //Execute la methode au demarrage du programme

            DisplayPersonne();

             
            dG1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dG1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

             
        }

        private void DisplayPersonne()
        {
            try
            {
                string query = "SELECT * FROM [MiseApplicationLearning].[dbo]. Utilisateurs_Archiv ";

                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    SqlDataReader rdr = sqlCommand.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(rdr);
                        dG1.DataSource = dt;
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
        }


        //verifie si le nom de l'utilisateur existe deja dans la base de donnée si oui alors retourne un message
        private void button1_Click(object sender, EventArgs e)
        {
             
            string query = "select prenom from Utilisateurs_Archiv where Prenom = @Prenom";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                //sqlCommand.Parameters.Add(new SqlParameter("@Prenom", SqlDbType.NChar, 20));
                //sqlCommand.Parameters["@Prenom"].Value = textBox1.Text;

                sqlCommand.Parameters.AddWithValue("@Prenom", textBoxPrenom.Text);

                sqlConnection.Open();

                SqlDataReader rdr = sqlCommand.ExecuteReader();

                if (rdr.HasRows)
                {
                    MessageBox.Show("Déja récuperé");
                    sqlConnection.Close();
                     
                }
                else
                {
                    string query1 = "insert into Utilisateurs_Archiv values (@Prenom ,@Nom, @Age)";

                    using (SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection1))
                    {

                        sqlConnection1.Open();

                        sqlCommand1.Parameters.AddWithValue("@Prenom", textBoxPrenom.Text);
                        sqlCommand1.Parameters.AddWithValue("@Nom", textBoxNom.Text);
                        sqlCommand1.Parameters.AddWithValue( "@Age", textBoxAge.Text);
                        
                        sqlCommand1.ExecuteScalar();

                        sqlConnection1.Close();

                        
                    }
                }
            }

            sqlConnection.Close();
            DisplayPersonne();
        }

        //creation d'une table dans la base de donnée
        private void button3_Click(object sender, EventArgs e)
        {
            string query1 = $" select * into {textBoxListe.Text} from Utilisateurs";

            using (SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection1))
            {
                sqlConnection1.Open();
 

                sqlCommand1.ExecuteScalar();

                sqlConnection1.Close();
            }
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    string query1 = $"CREATE TABLE {textBoxListe.Text}([id][int] PRIMARY KEY IDENTITY(1, 1) NOT NULL,[Prenom] [nvarchar](50) NULL,[Nom] [nvarchar](50) NULL,[Age] [int] NULL)";

        //    using (SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection1))
        //    {
        //        sqlConnection1.Open();


        //        sqlCommand1.ExecuteScalar();

        //        sqlConnection1.Close();
        //    }
        //}

        //insertion dans une table archive
        private void button2_Click(object sender, EventArgs e)
        {
            //string query1 = $"insert into Utilisateurs (Prenom,Nom,Age) select Prenom, Nom, Age from {textBoxListe.Text}";
            string query1 = $" select *  into {textBoxListe.Text} from Utilisateurs";
            

            using (SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection1))
                {
                    sqlConnection1.Open();
                    sqlCommand1.ExecuteScalar();
                    sqlConnection1.Close();
                }
                MessageBox.Show("Distribution terminé");
        }

         


    }
}
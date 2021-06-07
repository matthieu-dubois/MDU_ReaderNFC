using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        public Form1()
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["Matthieu"].ConnectionString;
            sqlConnection = new SqlConnection(connectionstring);

            InitializeComponent();

            //Execute la methode au demarrage du programme
            DisplayPersonne();
            TextBox();
            dG1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dG1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

             
        }

        private void TextBox()
        {
            // permet de récuperer le nom du poste

            string matthMachineName = Environment.MachineName;

            if (matthMachineName =="DESKTOP-PQ85T65")
            {
                textBox2.Text = "L17";
            }
            else
            {
                textBox2.Text = "Matt";
            }

            
        }

        private void DisplayPersonne()
        {
            try
            {
                string query = "SELECT * FROM [MiseApplicationLearning].[dbo].[Utilisateurs] ";

                SqlDataAdapter sqlData = new SqlDataAdapter(query, sqlConnection);

                using (sqlData)
                {

                    DataTable personnDataTable = new DataTable();

                    sqlData.Fill(personnDataTable);

                   //Afficher les données de la table dans le datagrid

                    dG1.DataSource = personnDataTable;
                    
                    dG1.AutoGenerateColumns = false;

                    dG1.DataSource = personnDataTable;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "insert into Utilisateurs_Archiv values (@Prenom , 'NA' , 0)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();

                sqlCommand.Parameters.AddWithValue("@Prenom", textBox1.Text);

                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                sqlConnection.Close();
                DisplayPersonne();
            }
        }

        private void dG1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = dG1.CurrentCell.RowIndex;
            int columnindex = dG1.CurrentCell.ColumnIndex;

            textBox1.Text = dG1.Rows[rowindex].Cells[columnindex].Value.ToString();

            string test = dG1.Rows[rowindex].Cells[columnindex].Value.ToString();

            try
            {
                string query = "insert into Utilisateurs_Archiv values (@Prenom , 'NA' , 0)";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();

                sqlCommand.Parameters.AddWithValue("@Prenom", test);

                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                sqlConnection.Close();
                DisplayPersonne();
            }
        }
    }
}

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Sydesoft.NfcDevice;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;
        private SqlConnection sqlConnection1;

        private static MyACR122U acr122u = new MyACR122U();

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


            acr122u.Init(false, 50, 4, 4, 200);
            acr122u.CardInserted += ReaderNfc;



        }

        private static void ReaderNfc(PCSC.ICardReader reader)
        {

            acr122u.ReadId = BitConverter.ToString(acr122u.GetUID(reader)).Replace("-", "");
        }

        private void DisplayPersonne()
        {
            try
            {
                string query = "SELECT * FROM  MDU";

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
             
            string query = "select RFID from mdu where RFID = @RFID";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
            {
                //sqlCommand.Parameters.Add(new SqlParameter("@Prenom", SqlDbType.NChar, 20));
                //sqlCommand.Parameters["@Prenom"].Value = textBox1.Text;

                sqlCommand.Parameters.AddWithValue("@RFID",textBox1.Text);

                sqlConnection.Open();

                SqlDataReader rdr = sqlCommand.ExecuteReader();

                if (rdr.HasRows)
                {
                    MessageBox.Show(" ok");
                    sqlConnection.Close();

                    //string query1 = $"insert into MDU (Recupere) values ('oui') where RFID = textBox1.Text";
                    string query1 = $"update mdu set Recupere = 'oui' where RFID = @RFID";


                    using (SqlCommand sqlCommand1 = new SqlCommand(query1, sqlConnection1))
                    {
                        sqlCommand1.Parameters.AddWithValue("@RFID", textBox1.Text);

                        sqlConnection1.Open();

                        sqlCommand1.ExecuteScalar();

                        sqlConnection1.Close();

                    }

                }
                else
                {
                    MessageBox.Show("l'tilisateur ne figure pas dans la liste");
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

        public class MyACR122U : ACR122U
        {
            private string readId;
            public string ReadId
            {
                get { return readId; }
                set { readId = value; }
            }

            public MyACR122U()
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = acr122u.ReadId;  
        }
    }
}
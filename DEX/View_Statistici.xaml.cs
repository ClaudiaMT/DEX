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
using System.Data;
using System.Data.SqlClient;

namespace DEX {
    public partial class View_Statistici : Window {
        public static string SQL_DataSource = "localhost\\DEX_DB";
        public static string SQL_UserID = "sa";
        public static string SQL_Password = "Qwert!123.";
        public static string SQL_InitialCatalog = "master";
        public static string db_name = "DEX_DB";
        public static int SQL_ConnectTimeout = 3; //asteapta x secunde pentru a verifica daca se poate conecta la SQLServer, apoi genereaza exceptia

        public SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public String sql;
        public StringBuilder sb = new StringBuilder();


        public View_Statistici() {
            InitializeComponent();
            Verifica_DEX_DB();
            View_lista_statistici_DEX_DB();
        }


        //******************* View_lista_statistici_SQL *****************
        public void View_lista_statistici_DEX_DB() {   // https://www.w3schools.com/sql/sql_join_full.asp
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "Cuvant";
            c1.Binding = new Binding("Cuvant");
            datatgrid_statistici.Columns.Add(c1);

            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "Numar accesari";
            c2.Binding = new Binding("Numar_accesari");
            datatgrid_statistici.Columns.Add(c2);

            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Cuvant,Total_accesari FROM Dictionar_accesari ");
                    sb.Append(" INNER JOIN Dictionar_cuvinte ON Dictionar_accesari.Id_cuvant=Dictionar_cuvinte.Id_cuvant");
                    //sb.Append(" FULL OUTER JOIN Dictionar_cuvinte ON Dictionar_accesari.Id_cuvant=Dictionar_cuvinte.Id_cuvant");
                    sql = sb.ToString();
                    DataSet dataSet = new DataSet();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                datatgrid_statistici.Items.Add(new { Cuvant = reader.GetString(0), Numar_accesari = reader.GetInt32(1).ToString() });
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* View_lista_statistici_SQL end *****************

        //******************* Afiseaza_mesaj_SQL *****************
        public void Afiseaza_mesaj_SQL(SqlException e) {
            StringBuilder errorMessages = new StringBuilder();
            for (int i = 0; i < e.Errors.Count; i++) {
                errorMessages.Append("*** Index #" + i + " ***\n" +
                    "Err code:\t" + e.Errors[i].Number + "\n" +
                    "Mesaj:\t" + e.Errors[i].Message + "\n\n");
            }
            MessageBox.Show(errorMessages.ToString(), "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            Console.WriteLine(e.ToString());
        }
        //******************* Afiseaza_mesaj_SQL end *****************

        //****************** Verifica_DEX_DB ******************
        public bool Verifica_DEX_DB() {
            try {
                builder.DataSource = SQL_DataSource;
                builder.UserID = SQL_UserID;
                builder.Password = SQL_Password;
                builder.InitialCatalog = SQL_InitialCatalog;
                builder.ConnectTimeout = SQL_ConnectTimeout; //asteapta x secunde pentru a verifica daca se poate conecta la SQLServer, apoi genereaza exceptia

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    connection.Close();
                    return true;
                }
            }

            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
                return false;
            }
        }
        //******************* Verifica_DEX_DB end *****************
    }
}

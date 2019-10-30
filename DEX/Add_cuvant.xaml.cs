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
    public partial class Add_cuvant : Window {
        public static string SQL_DataSource = "localhost\\DEX_DB";
        public static string SQL_UserID = "sa";
        public static string SQL_Password = "Qwert!123.";
        public static string SQL_InitialCatalog = "master";
        public static string db_name = "DEX_DB";
        public static int SQL_ConnectTimeout = 3; //asteapta x secunde pentru a verifica daca se poate conecta la SQLServer, apoi genereaza exceptia

        public SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public String sql;
        public StringBuilder sb = new StringBuilder();

        public Add_cuvant() {
            InitializeComponent();
            Verifica_DEX_DB();
            View_lista_categorii_DEX_DB(); //face refresh la lista categorii
        }

        private void btn_ok_add_word_Click(object sender, RoutedEventArgs e) {

            int Id_categorie_selectata=0;

            if ((txt_cuvant_nou.Text.Length != 0) && (txt_cuvant_nou_descriere.Text.Length != 0) && (listbox_selectie_categorie.SelectedItem != null)) {

                StringBuilder categorie_selectata = new StringBuilder();  // aduce cuvantul selectat din campul Categorie din fereastra ...
                foreach (DataRowView objDataRowView in listbox_selectie_categorie.SelectedItems) {
                    categorie_selectata.Append(objDataRowView[0].ToString());
                }

                try {                                        // si apoi cauta in baza de date cuvantul ca sa returneze Id_categorie, folosit mai jos pentru a completa corect tabelul Dictionar_cuvinte
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                        connection.Open();

                        sb.Clear();
                        sb.Append("USE " + db_name + "; ");
                        sb.Append("SELECT Id_categorie FROM Dictionar_categorii ");
                        sb.Append(" WHERE Categorie = @0");
                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@0", categorie_selectata.ToString());  // cauta Id_categorie in tabelul Dictionar_categorii
                            command.ExecuteNonQuery();

                            using (SqlDataReader reader = command.ExecuteReader()) {
                                    reader.Read();
                                    Id_categorie_selectata = reader.GetInt32(0);
                            }
                        }                     
                        connection.Close();
                    }
                }
                catch (SqlException ee) {
                    Afiseaza_mesaj_SQL(ee);
                }


                // completeaza tabelul Dictionar_cuvinte cu noul cuvant introdus
                try {
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                        connection.Open();

                        sb.Clear();
                        sb.Append("USE " + db_name + "; ");
                        sb.Append("INSERT INTO Dictionar_cuvinte (Cuvant, Descriere, Id_categorie) ");
                        sb.Append(" VALUES (@0,@1,@2)");
                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@0", txt_cuvant_nou.Text.ToString());  // forteaza sa scrie numai majuscule, vezi in xaml txt_cuvant_nou/CharacterCasing="Upper"
                            command.Parameters.AddWithValue("@1", txt_cuvant_nou_descriere.Text.ToString());
                            command.Parameters.AddWithValue("@2", Id_categorie_selectata);
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
                catch (SqlException ee) {
                    Afiseaza_mesaj_SQL(ee);
                }
            }
            else MessageBox.Show("Toate cele trei campuri\n\n   Cuvant\n   Categorie\n   Descriere\n\nsunt obligatorii!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); // afiseaza mesaj de eroare deoarece nu toate campurile sunt completate
            this.Close();
        }

        private void btn_cancel_add_word_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        //******************* View_lista_categorii_SQL *****************
        public void View_lista_categorii_DEX_DB() {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Categorie FROM Dictionar_categorii ");
                    sql = sb.ToString();
                    DataSet dataSet = new DataSet();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(dataSet);
                        listbox_selectie_categorie.DataContext = dataSet;
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* View_lista_categorii_SQL end *****************

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
        //******************* Verifica_DEX_DB end ****************
    }
}

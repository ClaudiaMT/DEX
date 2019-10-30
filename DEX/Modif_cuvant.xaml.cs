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
    public partial class Modif_cuvant : Window {
        public static string SQL_DataSource = "localhost\\DEX_DB";
        public static string SQL_UserID = "sa";
        public static string SQL_Password = "Qwert!123.";
        public static string SQL_InitialCatalog = "master";
        public static string db_name = "DEX_DB";
        public static int SQL_ConnectTimeout = 3; //asteapta x secunde pentru a verifica daca se poate conecta la SQLServer, apoi genereaza exceptia

        public SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public String sql;
        public StringBuilder sb = new StringBuilder();

        public Modif_cuvant(string cuvant_vechi) {
            InitializeComponent();
            Verifica_DEX_DB();

            txtblock_cuvant_de_modificat.Text = cuvant_vechi;
            citeste_Descriere_Categorie_cuvant(cuvant_vechi);
            View_lista_categorii_DEX_DB();
        }

        private void btn_ok_modif_word_Click(object sender, RoutedEventArgs e) {
            save_cuvant_modif();
            this.Close();
        }

        private void btn_cancel_modif_word_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        //******************* Descriere_Categorie_cuvant *****************
        public void citeste_Descriere_Categorie_cuvant(string cuvant_vechi) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Descriere, Categorie FROM Dictionar_cuvinte ");
                    sb.Append(" INNER JOIN Dictionar_Categorii ON Dictionar_cuvinte.Id_categorie=Dictionar_categorii.Id_categorie");
                    sb.Append(" WHERE Cuvant = @0");
                    sql = sb.ToString();
                    DataSet dataSet = new DataSet();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        command.Parameters.AddWithValue("@0", cuvant_vechi);
                        adapter.SelectCommand = command;
                        
                        using (SqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                txtbox_cuvant_vechi_descriere.Text = reader.GetString(0);
                                txtblock_categorie_veche.Text = reader.GetString(1);
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
        //******************* Descriere_Categorie_cuvant end *****************

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
                    listbox_selectie_categorie.UnselectAll();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* View_lista_categorii_SQL end *****************

        //******************* save_cuvant_modif *****************
        private void save_cuvant_modif() {

            int Id_categorie_selectata = 0;

            if ((txtbox_cuvant_vechi_descriere.Text.Length != 0) && (listbox_selectie_categorie.SelectedItem != null)) {

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


                // modifica tabelul Dictionar_cuvinte cu noul cuvant introdus
                try {
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                        connection.Open();

                        sb.Clear();
                        sb.Append("USE " + db_name + "; ");
                        sb.Append("UPDATE Dictionar_cuvinte SET Descriere = @1, Id_categorie = @2");
                        sb.Append(" WHERE Cuvant = @0");
                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@0", txtblock_cuvant_de_modificat.Text.ToString());  // forteaza sa scrie numai majuscule, vezi in xaml txt_cuvant_nou/CharacterCasing="Upper"
                            command.Parameters.AddWithValue("@1", txtbox_cuvant_vechi_descriere.Text.ToString());
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
        //******************* save_cuvant_modif end *****************

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

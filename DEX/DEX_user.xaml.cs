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
    public partial class DEX_user : Window {

        public static string SQL_DataSource = "localhost\\DEX_DB";
        public static string SQL_UserID = "sa";
        public static string SQL_Password = "Qwert!123.";
        public static string SQL_InitialCatalog = "master";
        public static string db_name = "DEX_DB";
        public static int SQL_ConnectTimeout = 3; //asteapta x secunde pentru a verifica daca se poate conecta la SQLServer, apoi genereaza exceptia

        public SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public String sql;
        public StringBuilder sb = new StringBuilder();

        public string user_name_local;
        public bool scroll_pagini_disponibile = true;

        public DEX_user(string user_name) {
            user_name_local = user_name;
            InitializeComponent();
            Verifica_DEX_DB();
            View_lista_categorii_DEX_DB(); //face refresh la lista categorii
            Indexeaza_cuvinte_prima_litera();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            MainWindow DEXwindow = new MainWindow();
            DEXwindow.Show();
            this.Close();
        }

        private void btn_change_password_Click(object sender, RoutedEventArgs e) {
            Change_Password ChangePasswordWindow = new Change_Password(user_name_local);
            ChangePasswordWindow.Show();
        }

        private void label_About_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            About aboutWindow = new About();
            aboutWindow.Show();
        }

        //******************* txtbox_cautare_GotFocus *****************
        private void txtbox_cautare_GotFocus(object sender, RoutedEventArgs e) {
            listbox_categorie.UnselectAll();   // listbox categorii devine inactiv deoarece nu mai folosim categoriile
            scroll_paginare.IsEnabled = false; // scrollbar de paginare devine inactiv deoarece nu mai folosim categoriile
            listbox_prima_litera.UnselectAll();// listbox prima litera devine inactiv deoarece nu mai folosim lista cu prima litera

            string cuvant_de_cautat = txtbox_cautare.Text.ToString();
            if (cuvant_de_cautat.Length == 0) {
                listbox_cuvinte.Visibility = System.Windows.Visibility.Hidden;
                txtbox_descriere.Text = "";
                txtbox_categorie.Text = "";
                groupbox_descriere.Header = "Descriere"; // numele GroupBoxului Descriere va fi Descriere
                groupbox_descriere.Foreground = Brushes.Black;

                txtblock_page_txt.Text = "";
                txtblock_page_from.Text = "cuvinte gasite";
                txtblock_page_to.Text = "0";
            }
        }
        //******************* txtbox_cautare_GotFocus end *****************

        //******************* listbox_prima_litera_GotFocus *****************
        private void listbox_prima_litera_GotFocus(object sender, RoutedEventArgs e) {
            listbox_categorie.UnselectAll();   // listbox categorii devine inactiv deoarece nu mai folosim categoriile
            txtbox_cautare.Clear();
            txtbox_descriere.Text = "";
            txtbox_categorie.Text = "";
            groupbox_descriere.Header = "Descriere"; // numele GroupBoxului Descriere va fi Descriere
            groupbox_descriere.Foreground = Brushes.Black;

            txtblock_page_txt.Text = "";
            txtblock_page_from.Text = "cuvinte gasite";
            txtblock_page_to.Text = "0";
            scroll_paginare.IsEnabled = false; // scrollbar de paginare devine inactiv deoarece nu mai folosim categoriile
        }
        //******************* listbox_prima_litera_GotFocus end *****************

        //******************* listbox_categorie_GotFocus *****************
        private void listbox_categorie_GotFocus(object sender, RoutedEventArgs e) {
            listbox_prima_litera.UnselectAll();// listbox prima litera devine inactiv deoarece nu mai folosim lista cu prima litera
            txtbox_cautare.Clear();
            txtbox_descriere.Text = "";
            txtbox_categorie.Text = "";
            groupbox_descriere.Header = "Descriere"; // numele GroupBoxului Descriere va fi Descriere
            groupbox_descriere.Foreground = Brushes.Black;
            txtblock_page_txt.Visibility = System.Windows.Visibility.Visible;
            txtblock_page_from.Visibility = System.Windows.Visibility.Visible;
            txtblock_page_to.Visibility = System.Windows.Visibility.Visible;

            txtblock_page_from.Text = "1";
            txtblock_page_txt.Text = "pana la";
            txtblock_page_to.Text = "10";
        }
        //******************* listbox_categorie_GotFocus end *****************

        //******************* txtbox_cautare_TextChanged *****************
        private void txtbox_cautare_TextChanged(object sender, TextChangedEventArgs e) {

            string cuvant_de_cautat = txtbox_cautare.Text.ToString();

            if (cuvant_de_cautat.Length == 0) {
                listbox_cuvinte.Visibility = System.Windows.Visibility.Hidden;
                txtbox_descriere.Text = "";
                txtbox_categorie.Text = "";
                groupbox_descriere.Header = "Descriere"; // numele GroupBoxului Descriere va fi Descriere
                groupbox_descriere.Foreground = Brushes.Black;
                listbox_prima_litera.UnselectAll(); //deselecteaza optiunile din lista de selectie prima_litera
                txtblock_page_to.Text = "0";
            }
            else {
                listbox_categorie.UnselectAll();//daca se face cautare cuvant atunci deselecteaza optiunile din ListBox_Categorii
                listbox_cuvinte.Visibility = System.Windows.Visibility.Visible;
                listbox_prima_litera.UnselectAll(); //deselecteaza optiunile din lista de selectie prima_litera

                try {
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                        connection.Open();

                        sb.Clear();
                        sb.Append("USE " + db_name + "; ");
                        sb.Append("SELECT Cuvant FROM  Dictionar_cuvinte ");
                        sb.Append("WHERE Cuvant LIKE '" + cuvant_de_cautat + "%';");
                        sql = sb.ToString();

                        DataSet dataSet = new DataSet();
                        using (SqlCommand command = new SqlCommand(sql, connection)) {
                            SqlDataAdapter adapter = new SqlDataAdapter();
                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet);
                            listbox_cuvinte.DataContext = dataSet;
                        }
                        connection.Close();
                        listbox_cuvinte.UnselectAll();
                        txtblock_page_to.Text = listbox_cuvinte.Items.Count.ToString();//afiseaza cate cuvinte s-au gasit in dictionar
                    }
                }
                catch (SqlException ee) {
                    Afiseaza_mesaj_SQL(ee);
                }
            }
        }
        //******************* txtbox_cautare_TextChanged end *****************

        //******************* listbox_cuvinte_SelectionChanged *****************
        private void listbox_cuvinte_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            StringBuilder cuvant_selectat = new StringBuilder();  // aduce cuvantul selectat din fereastra listbox_cuvinte ...
            foreach (DataRowView objDataRowView in listbox_cuvinte.SelectedItems) {
                cuvant_selectat.Append(objDataRowView[0].ToString());
            }

            groupbox_descriere.Header = cuvant_selectat.ToString(); // numele GroupBoxului Descriere va fi cuvantul selectat
            groupbox_descriere.Foreground = Brushes.Red;

            if (cuvant_selectat.ToString() == "") {
                txtbox_descriere.Text = "";
                txtbox_categorie.Text = "";
                groupbox_descriere.Header = "Descriere"; // numele GroupBoxului Descriere va fi Descriere
                groupbox_descriere.Foreground = Brushes.Black;
            }
            else {
                try {                                        // si apoi cauta in baza de date cuvantul ca sa returneze Descriere si Categorie, folosite mai jos
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                        connection.Open();

                        sb.Clear();
                        sb.Append("USE " + db_name + "; ");
                        sb.Append("SELECT Descriere, Categorie FROM Dictionar_cuvinte ");
                        sb.Append(" INNER JOIN Dictionar_categorii ON Dictionar_cuvinte.Id_categorie=Dictionar_categorii.Id_categorie");
                        sb.Append(" WHERE Dictionar_cuvinte.Cuvant = @0");
                        sql = sb.ToString();
                        using (SqlCommand command = new SqlCommand(sql, connection)) {
                            command.Parameters.AddWithValue("@0", cuvant_selectat.ToString());  // cauta cuvant_selectat in coloana Cuvant din tabelul Dictionar_cuvinte
                            command.ExecuteNonQuery();

                            using (SqlDataReader reader = command.ExecuteReader()) {
                                reader.Read();
                                txtbox_descriere.Text = reader.GetString(0);
                                txtbox_categorie.Text = reader.GetString(1);
                            }
                        }
                        connection.Close();
                    }
                    Actualizare_statistici_DEX_DB(cuvant_selectat.ToString());
                }
                catch (SqlException ee) {
                    Afiseaza_mesaj_SQL(ee);
                }
            }
        }
        //******************* listbox_cuvinte_SelectionChanged end *****************

        //******************* Modifica_categorie_DEX_DB *****************
        public void Actualizare_statistici_DEX_DB(string cuvant_selectectat) {
            int Id_cuvant = 0;
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Id_cuvant FROM Dictionar_cuvinte WHERE Cuvant = @0;");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", cuvant_selectectat);
                        command.ExecuteNonQuery();

                        using (SqlDataReader reader = command.ExecuteReader()) {
                                reader.Read();
                                Id_cuvant = reader.GetInt32(0); // variabila locala retine IDul cuvantului cautat in baza de date
                                }
                    }

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Id_cuvant FROM Dictionar_accesari WHERE Id_cuvant = @0");
                    sb.Append(" IF @@ROWCOUNT=0"); // daca linia cu Id_cuvant nu este gasita in tabel
                    sb.Append("  INSERT INTO Dictionar_accesari (Id_cuvant, Total_accesari) ");
                    sb.Append("  VALUES (@0, 1)");
                    sb.Append(" ELSE"); // atunci va trebuie adaugata cu valoarea 1
                    sb.Append("  UPDATE Dictionar_accesari SET Total_accesari = Total_accesari + 1 WHERE Id_cuvant = @0");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", Id_cuvant);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* Modifica_categorie_DEX_DB end *****************

        //******************* listbox_categorie_SelectionChanged *****************
        private void listbox_categorie_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            listbox_prima_litera.UnselectAll(); //deselecteaza optiunile din lista de selectie prima_litera
            scroll_paginare.Value = 0; // aduce la zero spre stanga bara pentru paginare
            scroll_pagini_disponibile = true;
            categorie_paginare(0); // afiseaza prima pagina incepand cu index 0
            txtblock_page_from.Text = "1";
            txtblock_page_to.Text = "10";
            scroll_paginare.Maximum = 10; // resetaza valoarea maxima a barei orizontale la maximum, aceasta se va mari dinamic
        }
        //******************* listbox_categorie_SelectionChanged end *****************

        //******************* categorie_paginare *****************
        private void categorie_paginare(int pagina_categorie) {
            int Id_categorie_selectata = 0;

            if (listbox_categorie.SelectedIndex != -1) {
                txtbox_descriere.Text = "";
                txtbox_categorie.Text = "";
                txtbox_cautare.Text = "";// daca s-a selectat categoriile atunci sterge textul din campul cautare cuvant
                scroll_paginare.IsEnabled = true; // scrollbar pentru paginare cate 10 cuvinte devine activ
                groupbox_descriere.Header = "Descriere"; // numele GroupBoxului Descriere va fi Descriere
                groupbox_descriere.Foreground = Brushes.Black;

                StringBuilder categorie_selectata = new StringBuilder();  // aduce cuvantul selectat din campul Categorie din fereastra ...
                foreach (DataRowView objDataRowView in listbox_categorie.SelectedItems) {
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

                try {
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                        connection.Open();

                        sb.Clear();
                        sb.Append("USE " + db_name + "; ");
                        sb.Append("SELECT Cuvant FROM  Dictionar_cuvinte ");
                        sb.Append(" INNER JOIN Dictionar_categorii ON Dictionar_cuvinte.Id_categorie=Dictionar_categorii.Id_categorie");
                        sb.Append(" WHERE Dictionar_categorii.Id_categorie = @0 ORDER BY Cuvant OFFSET @1 ROWS FETCH NEXT 10 ROWS ONLY;");//afiseaza primele 10 rezultate, celelalte se vor parcurge cu butoanele sageti
                        sql = sb.ToString();

                        DataSet dataSet = new DataSet();
                        using (SqlCommand command = new SqlCommand(sql, connection)) {
                            SqlDataAdapter adapter = new SqlDataAdapter();
                            command.Parameters.AddWithValue("@0", Id_categorie_selectata);
                            command.Parameters.AddWithValue("@1", pagina_categorie * 10); //clau
                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet);
                            listbox_cuvinte.DataContext = dataSet;
                        }
                        connection.Close();
                        listbox_cuvinte.UnselectAll();
                        listbox_cuvinte.Visibility = System.Windows.Visibility.Visible;

                        if ((listbox_cuvinte.Items.Count < 10) && scroll_pagini_disponibile) { // daca numarul de elemente afisate in paginare este mai mic de 10 inseamna ca s-a ajuns la sfarsit
                            scroll_paginare.Maximum = pagina_categorie;
                            scroll_pagini_disponibile = false;
                        }
                        if (scroll_pagini_disponibile) {
                            scroll_paginare.Maximum++;
                        }

                        txtblock_page_from.Text = (pagina_categorie*10+1).ToString(); // afiseaza cate cuvinte sunt paginate
                        txtblock_page_to.Text = (pagina_categorie * 10 + listbox_cuvinte.Items.Count).ToString();
                    }
                }
                catch (SqlException ee) {
                    Afiseaza_mesaj_SQL(ee);
                }
            }
            else {
                listbox_cuvinte.Visibility = System.Windows.Visibility.Hidden;
                txtbox_descriere.Text = "";
                txtbox_categorie.Text = "";
                groupbox_descriere.Header = "Descriere"; // numele GroupBoxului Descriere va fi Descriere
                groupbox_descriere.Foreground = Brushes.Black;
            }
        }
        //******************* categorie_paginare end *****************

        //******************* scroll_paginare_ValueChanged *****************
        private void scroll_paginare_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
              categorie_paginare(Convert.ToInt16(scroll_paginare.Value)); //modifica variabila pagina_categorie si afiseaza noua pagina cu 10 cuvinte filtrate cu Categoria selectata
        }
        //******************* scroll_paginare_ValueChanged end *****************

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
                        listbox_categorie.DataContext = dataSet;
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* View_lista_categorii_SQL end *****************

        //******************* Indexeaza_cuvinte_prima_litera *****************
        public void Indexeaza_cuvinte_prima_litera() {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT DISTINCT LEFT(Cuvant, 1) AS Cuvant FROM Dictionar_cuvinte ORDER BY Cuvant");
                    sql = sb.ToString();
                    DataSet dataSet = new DataSet();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(dataSet);
                        listbox_prima_litera.DataContext = dataSet;
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* Indexeaza_cuvinte_prima_litera end *****************

        //******************* listbox_prima_litera_SelectionChanged *****************
        private void listbox_prima_litera_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            listbox_cuvinte.Visibility = System.Windows.Visibility.Visible; // face vizibila lista de cuvinte daca aceasta a fost hidden
            listbox_categorie.UnselectAll(); // deselecteaza linia selectata din lista de categorii

            StringBuilder litera_selectata = new StringBuilder();  // aduce cuvantul selectat din fereastra listbox_cuvinte ...
            foreach (DataRowView objDataRowView in listbox_prima_litera.SelectedItems) {
                litera_selectata.Append(objDataRowView[0].ToString());
            }

                try {                                        // si apoi cauta in baza de date cuvantul ca sa returneze Descriere si Categorie, folosite mai jos
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                        connection.Open();

                        sb.Clear();
                        sb.Append("USE " + db_name + "; ");
                        sb.Append("USE " + db_name + "; ");
                        sb.Append("SELECT Cuvant FROM  Dictionar_cuvinte ");
                        sb.Append("WHERE Cuvant LIKE '" + litera_selectata + "%';");
                        sql = sb.ToString();

                        DataSet dataSet = new DataSet();
                        using (SqlCommand command = new SqlCommand(sql, connection)) {
                            SqlDataAdapter adapter = new SqlDataAdapter();
                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet);
                            listbox_cuvinte.DataContext = dataSet;
                        }
                        connection.Close();
                        listbox_cuvinte.UnselectAll();
                        txtblock_page_to.Text = listbox_cuvinte.Items.Count.ToString();//afiseaza cate cuvinte s-au gasit in dictionar
                    }
                }
                catch (SqlException ee) {
                    Afiseaza_mesaj_SQL(ee);
                }
        }
        //******************* listbox_prima_litera_SelectionChanged end *****************

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

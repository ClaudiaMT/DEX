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
    public partial class DEX_admin : Window {
        public static string SQL_DataSource = "localhost\\DEX_DB";
        public static string SQL_UserID = "sa";
        public static string SQL_Password = "Qwert!123.";
        public static string SQL_InitialCatalog = "master";
        public static string db_name = "DEX_DB";
        public static int SQL_ConnectTimeout = 3; //asteapta x secunde pentru a verifica daca se poate conecta la SQLServer, apoi genereaza exceptia

        public SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public String sql;
        public StringBuilder sb = new StringBuilder();

        public DEX_admin() {
            InitializeComponent();
            Verifica_DEX_DB();
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e) {
            MainWindow DEXwindow = new MainWindow();
            DEXwindow.Show();
            this.Close();
        }

        private void label_About_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            About aboutWindow = new About();
            aboutWindow.Show();
        }

        private void btn_ViewAutorizati_Click(object sender, RoutedEventArgs e) {
            btn_DelAutorizati.IsEnabled = true;
            btn_AutorizeazaCont.IsEnabled = false;
            txtblock_utilizatori.Text = "Lista utilizatori autorizati";
            View_lista_useri_DEX_DB("utilizator autentificat");
        }

        private void btn_ViewNeautorizati_Click(object sender, RoutedEventArgs e) {
            btn_AutorizeazaCont.IsEnabled = true;
            btn_DelAutorizati.IsEnabled = false;
            txtblock_utilizatori.Text = "Lista utilizatori neautorizati";
            View_lista_useri_DEX_DB("utilizator neautentificat");
        }

        private void btn_AutorizeazaCont_Click(object sender, RoutedEventArgs e) {
            if (Lista_utilizatori.SelectedItem == null) {
                MessageBox.Show("Selectati o optiune din lista.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                StringBuilder user_selectat = new StringBuilder();
                foreach (DataRowView objDataRowView in Lista_utilizatori.SelectedItems) {
                    user_selectat.Append(objDataRowView[0].ToString());
                    Autorizeaza_user_DEX_DB(user_selectat.ToString());
                    user_selectat.Clear();
                }
                View_lista_useri_DEX_DB("utilizator neautentificat");//face refresh la lista curenta
                Lista_utilizatori.UnselectAll();// deselecteaza orice optiune
            }
        }

         private void btn_DelAutorizati_Click(object sender, RoutedEventArgs e) {
             if (Lista_utilizatori.SelectedItem == null) {
                 MessageBox.Show("Selectati o optiune din lista.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
             }
             else {
                 StringBuilder user_selectat = new StringBuilder();
                 foreach (DataRowView objDataRowView in Lista_utilizatori.SelectedItems) {
                     user_selectat.Append(objDataRowView[0].ToString());
                     Delete_user_DEX_DB(user_selectat.ToString());
                     user_selectat.Clear();
                 }
                 View_lista_useri_DEX_DB("utilizator autentificat");//face refresh la lista curenta
                 Lista_utilizatori.UnselectAll();// deselecteaza orice optiune
             }
        }

        private void radio_statistici_Checked(object sender, RoutedEventArgs e) {
            btn_AddWordCategorie.IsEnabled = false;
            btn_ModifWordCategorie.IsEnabled = false;
            btn_DelWordCategorie.IsEnabled = false;
            txtblock_dictionar.Text = "Lista statistici";
            Refresh_Lista_dictionar(); //face refresh automat la Lista_dictionar
        }

        private void radio_cuvinte_Checked(object sender, RoutedEventArgs e) {
            btn_AddWordCategorie.IsEnabled = true;
            btn_ModifWordCategorie.IsEnabled = true;
            btn_DelWordCategorie.IsEnabled = true;
            txtblock_dictionar.Text = "Lista cuvinte";
            Refresh_Lista_dictionar(); //face refresh automat la Lista_dictionar
        }

        private void radio_categorie_Checked(object sender, RoutedEventArgs e) {
            btn_AddWordCategorie.IsEnabled = true;
            btn_ModifWordCategorie.IsEnabled = true;
            btn_DelWordCategorie.IsEnabled = true;
            txtblock_dictionar.Text = "Lista categorii";
            Refresh_Lista_dictionar(); //face refresh automat la Lista_dictionar
        }

        private void btn_AddWordCategorie_Click(object sender, RoutedEventArgs e) {
            if (radio_cuvinte.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Cuvant";
                Adauga_cuvant();
                View_lista_cuvinte_DEX_DB();//face refresh la lista curenta
            }
            else if (radio_categorie.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Categorie";
                Adauga_categorie();
                View_lista_categorii_DEX_DB();//face refresh la lista curenta
            }
            Lista_dictionar.UnselectAll();// deselecteaza orice linie selectata
        }

        private void btn_ModifWordCategorie_Click(object sender, RoutedEventArgs e) {
            if (radio_cuvinte.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Cuvant";
                Modifica_cuvant();
                View_lista_cuvinte_DEX_DB();//face refresh la lista curenta
            }
            else if (radio_categorie.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Categorie";
                Modifica_categorie();
                View_lista_categorii_DEX_DB();//face refresh la lista curenta
            }
            Lista_dictionar.UnselectAll();// deselecteaza orice linie selectata
        }

        private void btn_DelWordCategorie_Click(object sender, RoutedEventArgs e) {
            if (radio_cuvinte.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Cuvant";
                Delete_dictionar();
                View_lista_cuvinte_DEX_DB();//face refresh la lista curenta
            }
            else if (radio_categorie.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Categorie";
                Delete_categorie();
                View_lista_categorii_DEX_DB();//face refresh la lista curenta
            }
            Lista_dictionar.UnselectAll();// deselecteaza orice linie selectata
        }

        //******************* Refresh_Lista_dictionar *****************
        public void Refresh_Lista_dictionar() {
            if (radio_cuvinte.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Cuvant";
                Lista_dictionar.Items.Refresh();
                View_lista_cuvinte_DEX_DB();
            }
            else if (radio_categorie.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Categorie";
                Lista_dictionar.Items.Refresh();
                View_lista_categorii_DEX_DB();
            }
            else if (radio_statistici.IsChecked == true) {
                Lista_dictionar.DisplayMemberPath = "Cuvant Total_accesari";
                Lista_dictionar.Items.Refresh();
                View_lista_statistici_DEX_DB();

                View_Statistici DEXStatisticiWindow = new View_Statistici(); // deschide o noua fereastra cu statisticile fiecarui cuvant
                DEXStatisticiWindow.ShowDialog();
            }
        }
        //******************* Refresh_Lista_dictionar end *****************

        //******************* Adauga_categorie *****************
        public void Adauga_categorie() {

            Add_categorie Add_categorie_window = new Add_categorie(); // deschide fereastra Add_categorie
            Add_categorie_window.ShowDialog(); // ShowDialog deschide fereastra si asteapta pana aceasta se inchide ca sa treaca la urmatoarea instructiune View_lista_categorii_DEX_DB()

            View_lista_categorii_DEX_DB(); // refresh la Lista_dictionar
        }
        //******************* Adauga_categorie end *****************

        //******************* Adauga_cuvant *****************
        public void Adauga_cuvant() {

            Add_cuvant Add_cuvant_window = new Add_cuvant(); // deschide fereastra Add_cuvant
            Add_cuvant_window.ShowDialog(); // ShowDialog deschide fereastra si asteapta pana aceasta se inchide ca sa treaca la urmatoarea instructiune View_lista_cuvinte_DEX_DB()

            View_lista_cuvinte_DEX_DB(); // refresh la Lista_dictionar
        }
        //******************* Adauga_ccuvant end *****************

        //******************* Modifica_categorie *****************
        public void Modifica_categorie() {

            if (Lista_dictionar.SelectedItem == null) {
                MessageBox.Show("Selectati o optiune din lista.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                StringBuilder categorie_selectata = new StringBuilder();
                foreach (DataRowView objDataRowView in Lista_dictionar.SelectedItems) {
                    categorie_selectata.Append(objDataRowView[0].ToString());

                    Modif_categorie Modif_categorie_window = new Modif_categorie(categorie_selectata.ToString()); // deschide fereastra Modif_categorie
                    Modif_categorie_window.ShowDialog(); // ShowDialog deschide fereastra si asteapta pana aceasta se inchide ca sa treaca la urmatoarea instructiune View_lista_categorii_DEX_DB()

                    categorie_selectata.Clear(); // goleste variabila
                }
            }
            View_lista_categorii_DEX_DB(); // refresh la Lista_dictionar
        }
        //******************* Modifica_categorie end *****************

        //******************* Modifica_cuvant *****************
        public void Modifica_cuvant() {
            if (Lista_dictionar.SelectedItem == null) {
                MessageBox.Show("Selectati un cuvant din lista.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                StringBuilder cuvant_selectat= new StringBuilder();
                foreach (DataRowView objDataRowView in Lista_dictionar.SelectedItems) {
                    cuvant_selectat.Append(objDataRowView[0].ToString());

                    Modif_cuvant Modif_cuvant_window = new Modif_cuvant(cuvant_selectat.ToString()); // deschide fereastra Modif_cuvant
                    Modif_cuvant_window.ShowDialog(); // ShowDialog deschide fereastra si asteapta pana aceasta se inchide ca sa treaca la urmatoarea instructiune View_lista_cuvinte_DEX_DB()

                    cuvant_selectat.Clear(); // goleste variabila
                }
            }
            View_lista_cuvinte_DEX_DB(); // refresh la Lista_dictionar
        }
        //******************* Modifica_ccuvant end *****************

        //******************* Delete_dictionar *****************
        public void Delete_dictionar() {
            if (Lista_dictionar.SelectedItem == null) {
                MessageBox.Show("Selectati o optiune din lista.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                StringBuilder cuvant_selectat = new StringBuilder();
                foreach (DataRowView objDataRowView in Lista_dictionar.SelectedItems) {
                    cuvant_selectat.Append(objDataRowView[0].ToString());
                    Delete_cuvant_DEX_DB(cuvant_selectat.ToString());
                    cuvant_selectat.Clear();
                }
            }
        }
        //******************* Delete_dictionar end *****************

        //******************* Delete_categorie *****************
        public void Delete_categorie() {
            if (Lista_dictionar.SelectedItem == null) {
                MessageBox.Show("Selectati o optiune din lista.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else {
                StringBuilder categorie_selectata = new StringBuilder();
                foreach (DataRowView objDataRowView in Lista_dictionar.SelectedItems) {
                    categorie_selectata.Append(objDataRowView[0].ToString());
                    Delete_categorie_DEX_DB(categorie_selectata.ToString());
                    categorie_selectata.Clear();
                }
            }
        }
        //******************* Delete_categorie end *****************

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

        //******************* View_lista_useri_SQL *****************
        public void View_lista_useri_DEX_DB(string tip_utilizator_selected) {
            try {
                // https://stackoverflow.com/questions/3558945/populating-a-wpf-listbox-with-items-from-an-sql-sdf-database
                // https://www.c-sharpcorner.com/UploadFile/mahesh/data-binding-in-wpf/
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Nume FROM Utilizatori ");
                    sb.Append(" INNER JOIN Utilizatori_Tip ON Utilizatori.Id_tip_utilizator=Utilizatori_Tip.Id_tip_utilizator");
                    sb.Append(" WHERE Utilizatori_Tip.Tip_utilizator = @0");
                    sql = sb.ToString();
 
                    DataSet dataSet = new DataSet();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        command.Parameters.AddWithValue("@0", tip_utilizator_selected);
                        adapter.SelectCommand = command;
                        adapter.Fill(dataSet);
                        Lista_utilizatori.DataContext = dataSet;
                    }
                    connection.Close();
                    Lista_utilizatori.UnselectAll();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* View_lista_useri_SQL end *****************

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
                        Lista_dictionar.DataContext = dataSet;
                    }
                    connection.Close();
                    Lista_dictionar.UnselectAll();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* View_lista_categorii_SQL end *****************

        //******************* View_lista_statistici_SQL *****************
        public void View_lista_statistici_DEX_DB() {  //https://www.w3schools.com/sql/sql_join.asp
            try {
                // MessageBox.Show("statistici");
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Cuvant,Total_accesari FROM Dictionar_accesari ");
                    sb.Append(" INNER JOIN Dictionar_cuvinte ON Dictionar_accesari.Id_cuvant=Dictionar_cuvinte.Id_cuvant");
                    sql = sb.ToString();
                    DataSet dataSet = new DataSet();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(dataSet);
                        Lista_dictionar.DataContext = dataSet;
                    }
                    connection.Close();
                    Lista_dictionar.Items.Refresh();
                    Lista_dictionar.UnselectAll();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* View_lista_statistici_SQL end *****************

        //******************* Autorizeaza_user_SQL *****************
        public void Autorizeaza_user_DEX_DB(string utilizator_selectectat) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("UPDATE Utilizatori SET Id_tip_utilizator = 2");
                    sb.Append(" WHERE Nume = @0");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", utilizator_selectectat);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* Autorizeaza_user_SQL end *****************

        //******************* Delete_user_SQL *****************
        public void Delete_user_DEX_DB(string utilizator_selectectat) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("DELETE FROM Utilizatori");
                    sb.Append(" WHERE Nume = @0");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", utilizator_selectectat);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* Delete_user_SQL end *****************

        //******************* View_lista_cuvinte_SQL *****************
        public void View_lista_cuvinte_DEX_DB() {  //https://www.w3schools.com/sql/sql_join.asp
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Cuvant FROM Dictionar_cuvinte ");
                    sql = sb.ToString();
                    DataSet dataSet = new DataSet();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(dataSet);
                        Lista_dictionar.DataContext = dataSet;
                    }
                    connection.Close();
                    Lista_dictionar.UnselectAll();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* View_lista_cuvinte_SQL end *****************

        //******************* Delete_cuvant_SQL *****************
        public void Delete_cuvant_DEX_DB(string cuvant_selectectat) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("DELETE FROM Dictionar_cuvinte");
                    sb.Append(" WHERE Cuvant = @0");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", cuvant_selectectat);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* Delete_cuvant_SQL end *****************

        //******************* Delete_categorie_SQL *****************
        public void Delete_categorie_DEX_DB(string categorie_selectectata) {
            //schimba categoria tuturor cuvintelor ce au categorie_selectectata in categorie=NESPECIFICAT
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("UPDATE Dictionar_cuvinte SET Id_categorie = ( SELECT Id_categorie FROM Dictionar_categorii WHERE Categorie = @1) ");
                    sb.Append(" WHERE Id_categorie = ( SELECT Id_categorie FROM Dictionar_categorii WHERE Categorie = @0) ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", categorie_selectectata);
                        command.Parameters.AddWithValue("@1", "NESPECIFICAT");
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }

            // acum nici un cuvant nu mai are categoria categorie_selectectata si se poate sterge categoria din tabelul Dictionar_categorii
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("DELETE FROM Dictionar_categorii");
                    sb.Append(" WHERE Categorie = @0");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", categorie_selectectata);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
            }
        }
        //******************* Delete_categorie_SQL end *****************
    }
}

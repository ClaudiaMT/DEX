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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

// http://courses.cs.vt.edu/cs5614/lectures/module2.pdf
// http://home.hit.no/~hansha/documents/software/software_development/topics/resources/programming/exercises/Introduction%20to%20SQL%20Server%20and%20SQL.pdf


namespace DEX {

    public partial class MainWindow : Window {
        
        public static string SQL_DataSource = "localhost\\DEX_DB";
        public static string SQL_UserID = "sa";
        public static string SQL_Password = "Qwert!123.";
        //public static string SQL_InitialCatalog = "DEX_DB";
        public static string SQL_InitialCatalog = "master";
        public static string db_name = "DEX_DB";
        public static int    SQL_ConnectTimeout = 3; //asteapta x secunde pentru a verifica daca se poate conecta la SQLServer, apoi genereaza exceptia

        public SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        public String sql;
        public StringBuilder sb = new StringBuilder();

        public string userName = "";
        public string tip_utilizator = "";
       

        public MainWindow() {
            InitializeComponent();
            btn_Login.Visibility = System.Windows.Visibility.Hidden; //pana se conecteaza la baza de date butoanele nu vor fi vizibile
            btn_Signup.Visibility = System.Windows.Visibility.Hidden;
            btn_Cancel.Visibility = System.Windows.Visibility.Hidden;

            this.Show(); // se asigura ca afiseaza fereastra principala DEX_Login

            // verifica la inceput daca avem acces la SQLServer si daca exista baza de date DEX_DB
            if (!Verifica_DEX_DB()) {
                this.Close();
            }
            else { //daca avem acces la SQLServer atunci afiseaza butoanele si sterge continutul din cele 2 campuri
                btn_Login.Visibility = System.Windows.Visibility.Visible;
                btn_Signup.Visibility = System.Windows.Visibility.Visible;
                btn_Cancel.Visibility = System.Windows.Visibility.Visible;
                user_name.Text = "";
            }
        }

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

        //******************* buton Cancel *****************
        private void btn_Cancel_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
        //******************* buton Cancel end *****************

        //******************* buton Login *****************
        private void btn_Login_Click(object sender, RoutedEventArgs e) {
            userName = user_name.Text;

            if (userName == "reset") {
                Reset_Database();
            }
 
            //Verifica_User_DEX_DB(userName, PasswdBox.Password);
            if (Verifica_User_DEX_DB(userName, PasswdBox.Password)) { // daca autentificarea s-a facut cu succes atunci inchide fereastra curenta si deschide urmatoarea fereastra
                if (tip_utilizator == "utilizator autentificat") {
                    DEX_user DEXwindow = new DEX_user(userName);
                    DEXwindow.Show();
                }
                else if (tip_utilizator == "administrator") {
                    DEX_admin DEXwindow = new DEX_admin();
                    DEXwindow.Show();
                }
                this.Close();
            }
        }
        //******************* buton Login end *****************

        //******************* btn_Signup_Click *****************
        private void btn_Signup_Click(object sender, RoutedEventArgs e) {
            if (PasswdBox.Password.Length != 0) {
                if (Insert_user(user_name.Text, PasswdBox.Password)) {
                    MessageBox.Show("User  '" + user_name.Text + "'  de tip 'Utilizator neautentificat' s-a creat cu succes.\n\nDoar Administratorul va putea sa autorizeze noul cont de utilizator!", "User neautentificat - done", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else {
                MessageBox.Show("Va rugam sa introduceti o parola valida","Eroare parola", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            user_name.Text = "";// sterge campul user name
            PasswdBox.Clear(); // sterge campul password
        }
        //******************* btn_Signup_Click end *****************

        //****************** Insert_user ******************
        public bool Insert_user(string userNou, string userPassword) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("INSERT INTO Utilizatori (Nume, Parola, Id_tip_utilizator) VALUES (@0,@1,@2)");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", userNou);
                        command.Parameters.AddWithValue("@1", userPassword);
                        command.Parameters.AddWithValue("@2", 3);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                    return true;
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
                return false;
            }
        }
        //****************** Insert_user ******************

        //****************** Verifica_userName_DEX_DB ******************
        public bool Verifica_User_DEX_DB(string userSelected, string userPassword) {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();
                    int var_Id_tip_utilizator = 0;
                    string var_password = "";
                    string var_userName = "";

                    //https://www.w3schools.com/sql/sql_join.asp

                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("SELECT Nume, Parola, Tip_utilizator, Utilizatori.Id_tip_utilizator FROM Utilizatori ");
                    sb.Append(" INNER JOIN Utilizatori_Tip ON Utilizatori.Id_tip_utilizator=Utilizatori_Tip.Id_tip_utilizator");
                    sb.Append(" WHERE Utilizatori.Nume = @0");

                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) {
                        command.Parameters.AddWithValue("@0", userSelected);
                        command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                //Console.WriteLine("\ntest\n{0}\n{1}\n{2}\n{3}\n", reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                                var_userName = reader.GetString(0);
                                var_password = reader.GetString(1);
                                tip_utilizator = reader.GetString(2);
                                var_Id_tip_utilizator = reader.GetInt32(3);
                            }
                        }
                    }


                    if (var_Id_tip_utilizator != 0) {// daca se gaseste userul in baza de date
                        if (var_Id_tip_utilizator == 3) {// daca utilizatorul este de tip neautentificat
                            MessageBox.Show("User  '" + var_userName + "'  este de tip Utilizator neautentificat.\n\nDoar Administratorul va putea sa autorizeze contul de utilizator!", "User neautentificat", MessageBoxButton.OK, MessageBoxImage.Information);
                            user_name.Text = "";// sterge campul user name
                            PasswdBox.Clear(); // sterge campul password
                            connection.Close();
                            return false;
                        }
                        else {
                            if (userPassword != var_password) {// daca parola nu este cea corecta
                                MessageBox.Show("Parola introdusa este gresita!", "Parola gresita", MessageBoxButton.OK, MessageBoxImage.Information);
                                PasswdBox.Clear(); // sterge campul password
                                connection.Close();
                                return false;
                            }
                            else {// daca autentificarea s-a facut cu success...
                                user_name.Text = "";// sterge campul user name
                                PasswdBox.Clear(); // sterge campul password
                                connection.Close();
                                return true;
                            }
                        }
                    }
                    else { // daca userul nu se afla in baza de date
                        MessageBox.Show("User name  '" + userSelected + "'  nu este inregistrat.\n\nVa rugam sa alegeti un Nume si o Parola,\napoi selectati 'Sign up' pentru a crea un nou cont.\n\nAtentie, contul nou creat este de tip utilizator neautentificat.\nDoar Administratorul va putea sa autorizeze noul cont de utilizator!", "User not found", MessageBoxButton.OK, MessageBoxImage.Error);
                        user_name.Text = "";// sterge campul user name
                        PasswdBox.Clear(); // sterge campul password
                        connection.Close();
                        return false;
                    }
                }
            }
            catch (SqlException e) {
                Afiseaza_mesaj_SQL(e);
                return false;
            }
        }
        //******************* Verifica_userName_DEX_DB end*****************

        //******************* Reset_Database *****************
        private void Reset_Database() {
            try {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString)) {
                    connection.Open();

                    sql = "DROP DATABASE [" + db_name + "]; CREATE DATABASE [" + db_name + "]";
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    String table_name;

                    // https://www.w3schools.com/sql/sql_foreignkey.asp
                    // https://www.w3schools.com/sql/sql_injection.asp

                    /*
                    The UNIQUE constraint ensures that all values in a column are different.
                    Both the UNIQUE and PRIMARY KEY constraints provide a guarantee for uniqueness for a column or set of columns.
                    A PRIMARY KEY constraint automatically has a UNIQUE constraint.
                    */

                    table_name = "Utilizatori_Tip";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE TABLE " + table_name + " ( ");
                    sb.Append(" Id_tip_utilizator INT IDENTITY(1,1) NOT NULL, ");
                    sb.Append(" Tip_utilizator NVARCHAR(50) NOT NULL UNIQUE, ");
                    sb.Append(" PRIMARY KEY (Id_tip_utilizator) ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO " + table_name + " (Tip_utilizator) VALUES ");
                    sb.Append("(N'administrator'), ");
                    sb.Append("(N'utilizator autentificat'), ");
                    sb.Append("(N'utilizator neautentificat'); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    table_name = "Utilizatori";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE TABLE " + table_name + " ( ");
                    sb.Append(" Id_utilizator INT IDENTITY(1,1) NOT NULL, ");
                    sb.Append(" Nume NVARCHAR(50) NOT NULL UNIQUE, ");
                    sb.Append(" Parola NVARCHAR(50) NOT NULL, ");
                    sb.Append(" PRIMARY KEY (Id_utilizator), ");
                    sb.Append(" Id_tip_utilizator INT FOREIGN KEY REFERENCES Utilizatori_Tip(Id_tip_utilizator) NOT NULL ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO " + table_name + " (Nume, Id_tip_utilizator, Parola) VALUES ");
                    sb.Append("(N'admin', N'1', N'admin'), ");
                    sb.Append("(N'Clau', N'2', N'13'), ");
                    sb.Append("(N'Ada', N'3', N'11'), ");
                    sb.Append("(N'Alex', N'3', N'24'), ");
                    sb.Append("(N'Dan', N'2', N'03'); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    table_name = "Utilizatori";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE UNIQUE INDEX " + "index_" + table_name + " ON " + table_name + " (Nume);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    table_name = "Dictionar_categorii";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE TABLE " + table_name + " ( ");
                    sb.Append(" Id_categorie INT IDENTITY(1,1) NOT NULL, ");
                    sb.Append(" Categorie NVARCHAR(50) NOT NULL UNIQUE DEFAULT 'NEDEFINIT', "); // coloana Categorie are valoare default=NEDEFINIT
                    sb.Append(" PRIMARY KEY (Id_categorie) ");
                    sb.Append("); ");
                    sb.Append("INSERT INTO " + table_name + " (Categorie) VALUES ");
                    sb.Append("(N'adjectiv'), ");
                    sb.Append("(N'adverb'), ");                                 
                    sb.Append("(N'conjunctie'), ");                             
                    sb.Append("(N'interjectie'), ");                            
                    sb.Append("(N'numar cardinal'), ");                         
                    sb.Append("(N'numar ordinar'), ");                      
                    sb.Append("(N'prepozitie'), ");                             
                    sb.Append("(N'pronume demonstrativ'), ");       
                    sb.Append("(N'pronume interogativ'), ");
                    sb.Append("(N'pronume nehotarat'), ");                      
                    sb.Append("(N'pronume personal'), ");                       
                    sb.Append("(N'substantiv'), ");                             
                    sb.Append("(N'substantiv feminin'), ");                     
                    sb.Append("(N'substantiv masculin'), ");                    
                    sb.Append("(N'substantiv neutru'), ");                      
                    sb.Append("(N'verb'), ");                                   
                    sb.Append("(N'verb intranzitiv'), ");                       
                    sb.Append("(N'verb reflexiv'), ");                          
                    sb.Append("(N'verb tranzitiv'), ");
                    sb.Append("(N'NESPECIFICAT'); ");    
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    // indexeaza in tabelul "Dictionar_categorii" pentru a cauta mai rapid dupa campul Categorie
                    table_name = "Dictionar_categorii";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE UNIQUE INDEX " + "index_" + table_name + " ON " + table_name + " (Categorie);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    table_name = "Dictionar_cuvinte";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE TABLE " + table_name + " ( ");
                    sb.Append(" Id_cuvant INT IDENTITY(1,1) NOT NULL, ");
                    sb.Append(" Cuvant NVARCHAR(255) NOT NULL UNIQUE, ");
                    sb.Append(" Descriere NVARCHAR(255) NOT NULL, ");
                    sb.Append(" PRIMARY KEY (Id_cuvant), ");
                    sb.Append(" Id_categorie INT FOREIGN KEY REFERENCES Dictionar_categorii(Id_categorie) NOT NULL ");
                    sb.Append("); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    // indexeaza in tabelul "Dictionar_cuvinte" pentru a cauta mai rapid dupa campul Cuvant
                    table_name = "Dictionar_cuvinte";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE UNIQUE INDEX " + "index_" + table_name + " ON " + table_name + " (Cuvant);");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    table_name = "Dictionar_accesari";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE TABLE " + table_name + " ( ");
                    sb.Append(" Total_accesari INT, ");
                    sb.Append(" Id_cuvant INT FOREIGN KEY REFERENCES Dictionar_cuvinte(Id_cuvant) ON DELETE CASCADE ");
                    sb.Append("); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    table_name = "Utilizatori_cuvinte_cautate";
                    sb.Clear();
                    sb.Append("USE " + db_name + "; ");
                    sb.Append("CREATE TABLE " + table_name + " ( ");
                    sb.Append(" Numar_cautari INT NOT NULL, ");
                    sb.Append(" PRIMARY KEY (Numar_cautari), ");
                    sb.Append(" Id_cuvant INT FOREIGN KEY REFERENCES Dictionar_cuvinte(Id_cuvant) NOT NULL, ");
                    sb.Append(" Id_utilizator INT FOREIGN KEY REFERENCES Utilizatori(Id_utilizator) NOT NULL ");
                    sb.Append("); ");
                    sql = sb.ToString();
                    using (SqlCommand command = new SqlCommand(sql, connection)) { command.ExecuteNonQuery(); }

                    connection.Close();
                    user_name.Text = "";
                    MessageBox.Show("Baza de date " + db_name + " s-a initializat cu succes.", "Initializare", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException e1) {
                Afiseaza_mesaj_SQL(e1);
            }
        }
        //*******************Reset_Database end*****************
		
        private void label_About_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            About aboutWindow = new About();
            aboutWindow.Show();
        }


    }
}

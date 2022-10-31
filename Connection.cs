using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using authenticator.Configuration;


public class Connection
{
    public static SQLiteConnection? connectWithDatabase()
    {
        Config config = ConfigManager.Loader();
        SQLiteConnection? conn = null;
        try
        {
            conn = new SQLiteConnection(config.ConnectionString);
            conn.Open();
            Console.WriteLine("Conexão com o banco de dados feita com sucesso");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Houve problema ao tentar conectar com o banco de dados");
        }

        return conn;
    }

    public static bool buildDatabaseContent(SQLiteConnection? conn)
    {
        string queryString = File.ReadAllText("start_db.sql");

        try
        {
            using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
            {
                cmd.ExecuteReader();
            }
            return true;
        } 
        catch(Exception ex)
        {
            MessageBox.Show("Erro ao construir banco de dados!: " + ex.Message);
            return false;
        }
        
    }
}

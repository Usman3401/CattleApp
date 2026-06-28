using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BholaCattleApp.Services
{
    public class Connection
    {
        public static OracleConnection _connection;
        private static string connString;
        public static void OpenDatabaseConnection()
        {
            try
            {
                connString = ConfigManager.GetConnectionString("OracleDb");
                _connection = new OracleConnection(connString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed:\n{ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RecheckConnection()
        {
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Closed || _connection.State != ConnectionState.Broken)
                {
                    _connection = new OracleConnection(connString);
                    _connection.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Reconnection Failed:\n{ex.Message}","Database Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

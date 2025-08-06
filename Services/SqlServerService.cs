using System;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

namespace ContosoPizza.Services;

class SQLServerService
{

    public SqlConnection conn;
    public SQLServerService()
    {
        string connStr = "Server=WS-01-396;Database=testdb;User ID=sa;Password=z3us@123;TrustServerCertificate=True;";
        Console.WriteLine("Connecting to SQL Server...");
        
        this.conn = new SqlConnection(connStr);
        try
        {
            this.conn.Open();
            Console.WriteLine("Connected to SQL Server!");
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error connecting to SQL Server: " + ex.Message);
        }
    }


}

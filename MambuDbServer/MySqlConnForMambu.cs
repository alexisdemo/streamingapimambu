using System;
using MySql.Data.MySqlClient;
using MambuDbServer.entities;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace MambuDbServer
{
    public class MySqlConnForMambu
    {
        public MySqlConnForMambu()
        { }


        public void TestConnection()
        {
            //string connStr = "server=database-streaming.c8tzvgsv1enk.us-east-1.rds.amazonaws.com;user=mambu;port=3306;password=mambu123;";
            string x = GetConnectionString();
            using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
            {
                conn.Open();
            }
        }

        public List<Eventos> ReadDataBaseMambu()
        {
            string connStr = GetConnectionString();
            List<Eventos> lista = new List<Eventos>();
            using (
             MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    string sql = "SELECT * FROM streamingapi.Eventos;";
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();


                    while (rdr.Read())
                    {
                        Eventos eve = new Eventos();

                        eve.UUID = Convert.ToString(rdr["uuid"]);
                        eve.Mensaje = Convert.ToString(rdr["mensaje"]);
                        lista.Add(eve);

                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return lista;
        }

        public void InsertDataBaseMambu(string mensaje)
        {
            Guid myuuid = Guid.NewGuid();
            try
            {
                string connStr = GetConnectionString();

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"insert into streamingapi.Eventos (uuid, mensaje) values (@uuid, @mensaje);";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@uuid", myuuid.ToString());
                    cmd.Parameters.AddWithValue("@mensaje", mensaje);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ClearDataBaseMambu()
        {
            Guid myuuid = Guid.NewGuid();
            try
            {
                string connStr = GetConnectionString();

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string sql = @"truncate table streamingapi.Eventos;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetConnectionString()
        {
            string conn = string.Empty;

            try
            {

                IConfigurationBuilder builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddJsonFile("archivodos.json", optional: true, reloadOnChange: true);
                IConfiguration configuration = builder.Build();
                conn = configuration.GetConnectionString("DefaultConnection");
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return conn;
        }

    }
}

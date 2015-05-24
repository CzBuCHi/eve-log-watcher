using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using eve_log_watcher.model;

namespace eve_log_watcher
{
    public static class DbHelper
    {
        private static DataContext _DataContext;
        private static SQLiteConnection _Connection;
        public static DataContext DataContext => _DataContext ?? (_DataContext = new DataContext());
        public static bool HasDataContext => _DataContext != null;

        public static IEnumerable<string> EnumerateSolarsystemsInText(string text) {
            return from o in DataContext.SolarSystems.ToArray()
                   where Regex.Match(text, @"(^|\s)" + Regex.Escape(o.SolarsystemName) + @"([\*\s]|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase).Success
                   select o.SolarsystemName;
        }

        #region Import data from MySQL Dumps - see http://eve-marketdata.com/developers/mysql.php

        public static void EnsureDatatabaseExists() {
            ConnectionStringSettings c = ConfigurationManager.ConnectionStrings["red-watcher"];
            IEnumerable<string> q = from o in c.ConnectionString.Split(';')
                                    let p = o.Split('=')
                                    where p[0] == "Data Source"
                                    select p[1];

            string dbName = q.First();
#if DEBUG
            if (File.Exists(dbName)) {
                File.Delete(dbName);
            }
#endif
            if (File.Exists(dbName)) {
                return;
            }

            SQLiteConnection.CreateFile(dbName);

            using (_Connection = new SQLiteConnection(c.ConnectionString)) {
                _Connection.Open();
                CreateTable_Regions();
                CreateTable_SolarSystems();
                CreateTable_SolarSystemJumps();

                InsertData("mysql_eve_map_regions.sql");
                InsertData("mysql_eve_map_solarsystem_jumps.sql");
                InsertData("mysql_eve_map_solarsystems.sql");

                _Connection.Close();
            }
        }

        private static void CreateTable_Regions() {
            using (SQLiteCommand command = new SQLiteCommand(@"
CREATE TABLE [eve_map_regions] (
    [region_id] INTEGER NOT NULL PRIMARY KEY,
    [region_name] TEXT NOT NULL
)", _Connection)) {
                command.ExecuteNonQuery();
            }
        }

        private static void CreateTable_SolarSystems() {
            using (SQLiteCommand command = new SQLiteCommand(@"
CREATE TABLE [eve_map_solarsystems] (
    [solarsystem_id] INTEGER NOT NULL PRIMARY KEY,
    [region_id] INTEGER DEFAULT NULL,
    [region_name] TEXT NOT NULL DEFAULT '',
    [solarsystem_name] TEXT DEFAULT NULL,
    [security] REAL DEFAULT NULL, 
    [x] INTEGER NOT NULL DEFAULT 0,
    [y] INTEGER NOT NULL DEFAULT 0,
    [z] INTEGER NOT NULL DEFAULT 0,
    [flat_x] INTEGER NOT NULL DEFAULT 0,
    [flat_y] INTEGER NOT NULL DEFAULT 0,
    [dotlan_x] INTEGER NOT NULL DEFAULT 0,
    [dotlan_y] INTEGER NOT NULL DEFAULT 0,
    [has_station] INTEGER NOT NULL DEFAULT 0,

    FOREIGN KEY(region_id) REFERENCES eve_map_regions(region_id)
)", _Connection)) {
                command.ExecuteNonQuery();
            }
        }

        private static void CreateTable_SolarSystemJumps() {
            using (SQLiteCommand command = new SQLiteCommand(@"
CREATE TABLE [eve_map_solarsystem_jumps] (
    [from_solarsystem_id] INTEGER NOT NULL,
    [to_solarsystem_id] INTEGER NOT NULL,
    [from_region_id] INTEGER NOT NULL,
    [to_region_id] INTEGER NOT NULL,

    PRIMARY KEY (from_solarsystem_id, to_solarsystem_id),
    
    FOREIGN KEY(from_solarsystem_id) REFERENCES eve_map_solarsystems(solarsystem_id),
    FOREIGN KEY(to_solarsystem_id) REFERENCES eve_map_solarsystems(solarsystem_id),
    FOREIGN KEY(from_region_id) REFERENCES eve_map_regions(region_id),
    FOREIGN KEY(to_region_id) REFERENCES eve_map_regions(region_id)
)", _Connection)) {
                command.ExecuteNonQuery();
            }
        }

        private static void InsertData(string fileName) {
            fileName = AppDomain.CurrentDomain.BaseDirectory + @"\data\" + fileName;
            if (!File.Exists(fileName)) {
                throw new Exception("File " + fileName + " not found!");
            }

            foreach (string line in File.ReadLines(fileName)) {
                if (line.StartsWith("INSERT INTO")) {
                    using (SQLiteTransaction transaction = _Connection.BeginTransaction()) {
                        using (SQLiteCommand command = new SQLiteCommand(line, _Connection)) {
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    break;
                }
            }
        }

        #endregion
    }
}

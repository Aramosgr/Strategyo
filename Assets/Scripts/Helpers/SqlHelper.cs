using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;
using System.Collections.Generic;

public static class SqlHelper
{
    //Check C:/Users/USERNAME/AppData/LocalLow/DefaultCompany/Strategyo/
    private readonly static string connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";

    public static void CreateDDBB()
    {
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        IDbCommand dbcmd;
        IDataReader reader;

        dbcmd = dbcon.CreateCommand();
        string query =
          "CREATE TABLE IF NOT EXISTS " + "maps" + " (" +
          "id INTEGER PRIMARY KEY, " +
          "name VARCHAR, " +
          "X INTEGER, " +
          "Y INTEGER, " +
          "description VARCHAR )";

        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();

        dbcmd = dbcon.CreateCommand();
        query =
          "CREATE TABLE IF NOT EXISTS rows (" +
          "id INTEGER PRIMARY KEY, mapId INTEGER, description VARCHAR)";

        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();

        dbcmd = dbcon.CreateCommand();
        query =
          "CREATE TABLE IF NOT EXISTS cells (" +
          "id INTEGER PRIMARY KEY, rowId INTEGER, description VARCHAR, bridge BOOLEAN, X INTEGER, Y INTEGER, sprite INTEGER)";

        dbcmd.CommandText = query;
        reader = dbcmd.ExecuteReader();

        dbcon.Close();

        SeedData();

    }

    public static Map GetMap(mapType mapType)
    {
        var map = new Map();

        using (IDbConnection dbcon = new SqliteConnection(connection))
        {
            dbcon.Open();
            IDbCommand dbcmd;
            IDataReader reader;
            dbcmd = dbcon.CreateCommand();

            dbcmd.CommandText = "SELECT * FROM MAPS WHERE name='" + mapType + "'";
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                map.Name = reader["name"].ToString();
                map.Description = reader["description"].ToString();
                map.Id = int.Parse(reader["id"].ToString());
                map.X = int.Parse(reader["X"].ToString());
                map.Y = int.Parse(reader["Y"].ToString());
            }
        }
        map.Rows.AddRange(GetRows(map));
        return map;
    }

    private static List<Row> GetRows(Map map)
    {
        var rows = new List<Row>();

        using (IDbConnection dbcon = new SqliteConnection(connection))
        {
            dbcon.Open();
            IDbCommand dbcmd;
            IDataReader reader;
            dbcmd = dbcon.CreateCommand();

            dbcmd.CommandText = "SELECT * FROM ROWS WHERE mapId='" + map.Id + "'";
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                var row = new Row()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    Description = reader["description"].ToString(),
                    MapId = int.Parse(reader["mapId"].ToString())
                };
                rows.Add(row);
            }
        }
        foreach (Row row in rows)
        {
            row.Cells.AddRange(GetCells(row));
        }

        return rows;
    }

    private static List<Cell> GetCells(Row row)
    {
        var cells = new List<Cell>();

        using (IDbConnection dbcon = new SqliteConnection(connection))
        {
            dbcon.Open();
            IDbCommand dbcmd;
            IDataReader reader;
            dbcmd = dbcon.CreateCommand();

            dbcmd.CommandText = "SELECT * FROM CELLS WHERE rowId='" + row.Id + "'";
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                cells.Add(new Cell()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    RowId = int.Parse(reader["rowId"].ToString()),
                    Description = reader["description"].ToString(),
                    Bridge = bool.Parse(reader["bridge"].ToString()),
                    X = int.Parse(reader["X"].ToString()),
                    Y = int.Parse(reader["Y"].ToString()),
                    Sprite = int.Parse(reader["sprite"].ToString()),

                });
            }
        }
        return cells;
    }

    private static void SeedData()
    {
        DeleteData();
        SeedMaps();
        SeedRows();
        SeedCells();
    }

    private static void SeedMaps()
    {
        using (IDbConnection dbcon = new SqliteConnection(connection))
        {
            dbcon.Open();
            IDbCommand dbcmd;
            IDataReader reader;

            dbcmd = dbcon.CreateCommand();

            dbcmd.CommandText = "SELECT COUNT(*) FROM MAPS";

            int count = Convert.ToInt32(dbcmd.ExecuteScalar());

            if (count <= 0)
            {
                dbcmd = dbcon.CreateCommand();
                string query =
                    "INSERT INTO maps(id, name, X, Y, description) " +
                    "VALUES (1, 'BigRiver', 13, 8, 'A green map with a river crossing the middle'), " +
                    "(2, 'Islands', 13, 8, 'A group of islands in the ocean'), " +
                    "(3, 'Winter', 13, 8, 'A snow map with ice'), " +
                    "(4, 'Wasteland', 13, 8, 'A wasteland with no vegetation with rivers of lava');";
                dbcmd.CommandText = query;
                reader = dbcmd.ExecuteReader();

            }
        }
    }

    //check the board prefab, the list of blocksprites
    private static void SeedRows()
    {
        using (IDbConnection dbcon = new SqliteConnection(connection))
        {
            dbcon.Open();

            IDbCommand dbcmd;
            IDataReader reader;

            dbcmd = dbcon.CreateCommand();
            dbcmd.CommandText = "SELECT COUNT(*) FROM ROWS";

            int count = Convert.ToInt32(dbcmd.ExecuteScalar());

            if (count <= 0)
            {
                dbcmd = dbcon.CreateCommand();
                string query =
                    "INSERT INTO rows (id, MapId, description) " +
                    "VALUES " +
                    "(1,1,'row'), (2,1,'row'), (3,1,'row'), (4,1,'row'), " +
                    "(5,1,'row'), (6,1,'row'), (7,1,'row'), (8,1,'row'), " +
                    "(9,2,'row'), (10,2,'row'), (11,2,'row'), (12,2,'row'), " +
                    "(13,2,'row'), (14,2,'row'), (15,2,'row'), (16,2,'row'), " +
                    "(17,3,'row'), (18,3,'row'), (19,3,'row'), (20,3,'row'), " +
                    "(21,3,'row'), (22,3,'row'), (23,3,'row'), (24,3,'row'), " +
                    "(25,4,'row'), (26,4,'row'), (27,4,'row'), (28,4,'row'), " +
                    "(29,4,'row'), (30,4,'row'), (31,4,'row'), (32,4,'row');";
                dbcmd.CommandText = query;
                reader = dbcmd.ExecuteReader();
            }
        }
    }

    private static void SeedCells()
    {
        using (IDbConnection dbcon = new SqliteConnection(connection))
        {
            dbcon.Open();

            IDbCommand dbcmd;
            IDataReader reader;

            dbcmd = dbcon.CreateCommand();
            dbcmd.CommandText = "SELECT COUNT(*) FROM CELLS";

            int count = Convert.ToInt32(dbcmd.ExecuteScalar());

            if (count <= 0)
            {
                dbcmd = dbcon.CreateCommand();
                string query =
                    "INSERT INTO cells (rowId, description,bridge,X,Y,sprite) " +
                    "VALUES " +
                    "(1,'cell',0,0,0,4), (1,'cell',0,1,0,4), (1,'cell',0,2,0,4), (1,'cell',0,3,0,4), " +
                    "(1,'cell',0,4,0,4), (1,'cell',0,5,0,4), (1,'cell',0,6,0,32), (1,'cell',0,7,0,32), " +
                    "(1,'cell',0,8,0,4), (1,'cell',0,9,0,4), (1,'cell',0,10,0,4), (1,'cell',0,11,0,4), (1,'cell',0,12,0,4), " +
                    "(2,'cell',0,0,1,4), (2,'cell',0,1,1,4), (2,'cell',0,2,1,4), (2,'cell',0,3,1,4), " +
                    "(2,'cell',0,4,1,4), (2,'cell',0,5,1,4), (2,'cell',0,6,1,32), (2,'cell',0,7,1,32), " +
                    "(2,'cell',0,8,1,4), (2,'cell',0,9,1,4), (2,'cell',0,10,1,4), (2,'cell',0,11,1,4), (2,'cell',0,12,1,4), " +
                    "(3,'cell',0,0,2,4), (3,'cell',0,1,2,4), (3,'cell',0,2,2,4), (3,'cell',0,3,2,4), " +
                    "(3,'cell',0,4,2,4), (3,'cell',0,5,2,4), (3,'cell',1,6,2,32), (3,'cell',1,7,2,32), " +
                    "(3,'cell',0,8,2,4), (3,'cell',0,9,2,4), (3,'cell',0,10,2,4), (3,'cell',0,11,2,4), (3,'cell',0,12,2,4), " +
                    "(4,'cell',0,0,3,4), (4,'cell',0,1,3,4), (4,'cell',0,2,3,4), (4,'cell',0,3,3,4), " +
                    "(4,'cell',0,4,3,4), (4,'cell',0,5,3,4), (4,'cell',0,6,3,32), (4,'cell',0,7,3,32), " +
                    "(4,'cell',0,8,3,4), (4,'cell',0,9,3,4), (4,'cell',0,10,3,4), (4,'cell',0,11,3,4), (4,'cell',0,12,3,4), " +
                    "(5,'cell',0,0,4,4), (5,'cell',0,1,4,4), (5,'cell',0,2,4,4), (5,'cell',0,3,4,4), " +
                    "(5,'cell',0,4,4,4), (5,'cell',0,5,4,4), (5,'cell',0,6,4,32), (5,'cell',0,7,4,32), " +
                    "(5,'cell',0,8,4,4), (5,'cell',0,9,4,4), (5,'cell',0,10,4,4), (5,'cell',0,11,4,4), (5,'cell',0,12,4,4), " +
                    "(6,'cell',0,0,5,6), (6,'cell',0,1,5,6), (6,'cell',0,2,5,4), (6,'cell',0,3,5,4), " +
                    "(6,'cell',0,4,5,4), (6,'cell',0,5,5,4), (6,'cell',0,6,5,32), (6,'cell',0,7,5,32), " +
                    "(6,'cell',0,8,5,4), (6,'cell',0,9,5,4), (6,'cell',0,10,5,4), (6,'cell',0,11,5,4), (6,'cell',0,12,5,4), " +
                    "(7,'cell',0,0,6,6), (7,'cell',0,1,6,6), (7,'cell',0,2,6,6), (7,'cell',0,3,6,4), " +
                    "(7,'cell',0,4,6,4), (7,'cell',0,5,6,4), (7,'cell',0,6,6,32), (7,'cell',0,7,6,32), " +
                    "(7,'cell',0,8,6,4), (7,'cell',0,9,6,4), (7,'cell',0,10,6,4), (7,'cell',0,11,6,4), (7,'cell',0,12,6,4), " +
                    "(8,'cell',0,0,7,6), (8,'cell',0,1,7,6), (8,'cell',0,2,7,6), (8,'cell',0,3,7,4), " +
                    "(8,'cell',0,4,7,4), (8,'cell',0,5,7,4), (8,'cell',1,6,7,32), (8,'cell',1,7,7,32), " +
                    "(8,'cell',0,8,7,4), (8,'cell',0,9,7,4), (8,'cell',0,10,7,4), (8,'cell',0,11,7,4), (8,'cell',0,12,7,4) " +
                    ";";
                dbcmd.CommandText = query;
                reader = dbcmd.ExecuteReader();
            }
        }
    }

    private static void DeleteData()
    {
        using (IDbConnection dbcon = new SqliteConnection(connection))
        {
            dbcon.Open();
            IDbCommand dbcmd;
            IDataReader reader;

            dbcmd = dbcon.CreateCommand();
            string query = "DELETE FROM MAPS; DELETE FROM ROWS; DELETE FROM CELLS";
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
        }
    }
}
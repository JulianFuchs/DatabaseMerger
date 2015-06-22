using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseMerger
{
    class MySqlHandler
    {
        private MySqlConnection mySqlConnection;

        public Dictionary<string, List<int>> kickAssToTorrentId;
        public Dictionary<int, List<int>> IMDbToTorrentId;

        public Dictionary<string, int> kickAssAffiliationToIMDbId;
        public Dictionary<int, string> IMDbIdToKickAssAffiliation;

        public MySqlHandler()
        {
            String connectionString = "Server=localhost;Uid=root;Pwd=wasihrwolle;Database=bachelor";
            //String connectionString = "Server=localhost;Uid=root;Pwd=xSXLQHfSj9hUsmj4;Connection Timeout=10000;Database=bachelor";
            //string connectionString = "SERVER=localhost;" + "DATABASE=bachelor;" + "UID=root;" + "PASSWORD=xSXLQHfSj9hUsmj4;" + "Connection Timeout=10000";

            mySqlConnection = new MySqlConnection(connectionString);

            kickAssToTorrentId = new Dictionary<string, List<int>>();
            IMDbToTorrentId = new Dictionary<int, List<int>>();

            kickAssAffiliationToIMDbId = new Dictionary<string, int>();
            IMDbIdToKickAssAffiliation = new Dictionary<int, string>();
        }

        public Torrent[] getTorrents(int contributersThreshold, out int maxId, out int found)
        {
            Torrent[] torrents;

            mySqlConnection.Open();
            MySqlCommand maxIdQuery = mySqlConnection.CreateCommand();
            maxIdQuery.CommandText = "SELECT MAX(id) FROM `torrents` WHERE seeders + leechers >= ?count;";
            maxIdQuery.Parameters.Add("?count", MySqlDbType.Int32).Value = contributersThreshold;
            MySqlDataReader reader = maxIdQuery.ExecuteReader();

            reader.Read();

            maxId = reader.GetInt32("MAX(id)");

            reader.Close();
            mySqlConnection.Close();

            mySqlConnection.Open();
            MySqlCommand torrentsQuery = mySqlConnection.CreateCommand();
            torrentsQuery.CommandText = "SELECT `id`, `IMDBId`, `kickAssAffiliation` FROM `torrents` WHERE seeders + leechers >= ?count;";
            torrentsQuery.Parameters.Add("?count", MySqlDbType.Int32).Value = contributersThreshold;
            reader = torrentsQuery.ExecuteReader();

            found = 0;

            torrents = new Torrent[maxId + 1];

            while (reader.Read())
            {
                int id = reader.GetInt32("id");
                int IMDbId = reader.GetInt32("IMDbId");
                string kickAssAffiliation = reader.GetString("kickAssAffiliation");
                torrents[id] = new Torrent(id, IMDbId, kickAssAffiliation);
                found += 1;

                if (kickAssAffiliation != "")
                {
                    if (!kickAssToTorrentId.ContainsKey(kickAssAffiliation))
                    {
                        List<int> newList = new List<int>();
                        kickAssToTorrentId.Add(kickAssAffiliation, newList);
                    }
                    kickAssToTorrentId[kickAssAffiliation].Add(id);

                }
                if (IMDbId != 0)
                {
                    if (!IMDbToTorrentId.ContainsKey(IMDbId))
                    {
                        List<int> newList = new List<int>();
                        IMDbToTorrentId.Add(IMDbId, newList);
                    }
                    IMDbToTorrentId[IMDbId].Add(id);
                }

                if (IMDbId != 0 && kickAssAffiliation != "")
                {
                    if (!kickAssAffiliationToIMDbId.ContainsKey(kickAssAffiliation))
                    {
                        kickAssAffiliationToIMDbId.Add(kickAssAffiliation, IMDbId);
                    }
                    if (!IMDbIdToKickAssAffiliation.ContainsKey(IMDbId))
                    {
                        IMDbIdToKickAssAffiliation.Add(IMDbId, kickAssAffiliation);
                    }
                }
            }

            reader.Close();
            torrentsQuery.Dispose();
            maxIdQuery.Dispose();
            mySqlConnection.Close();

            return torrents;
        }

        public List<int>[] getAssociated(int maxId, out int maxPeerId)
        {
            mySqlConnection.Open();

            MySqlCommand maxPeerIdQuery = mySqlConnection.CreateCommand();
            maxPeerIdQuery.CommandText = "SELECT MAX(peerID) FROM `associated`;";
            MySqlDataReader reader = maxPeerIdQuery.ExecuteReader();

            reader.Read();

            maxPeerId = reader.GetInt32("MAX(peerID)");

            reader.Close();

            List<int>[] associated = new List<int>[maxId + 1];

            for (int i = 0; i <= maxId; i++)
            {
                associated[i] = new List<int>();
            }

            MySqlCommand associatedQuery = mySqlConnection.CreateCommand();
            associatedQuery.CommandText = "SELECT * FROM `associated` ORDER BY torrentID ASC;";
            reader = associatedQuery.ExecuteReader();

            while (reader.Read())
            {
                int torrentId = reader.GetInt32("torrentID");
                int peerId = reader.GetInt32("peerID");

                associated[torrentId].Add(peerId);
            }

            reader.Close();
            associatedQuery.Dispose();
            mySqlConnection.Close();

            return associated;
        }

        public void openConnection()
        {
            mySqlConnection.Open();
        }

        public void closeConnection()
        {
            mySqlConnection.Close();
        }

        public void insertIntoMergeTorrents(int mergeIdCounter, int torrentId)
        {
            MySqlCommand insertCommand = mySqlConnection.CreateCommand();
            insertCommand.CommandText = "INSERT INTO mergetorrents(mergeId,torrentId) " +
            "VALUES(?mergeId, ?torrentId);";
            insertCommand.Parameters.Add("?mergeId", MySqlDbType.Int32).Value = mergeIdCounter;
            insertCommand.Parameters.Add("?torrentId", MySqlDbType.VarChar).Value = torrentId;
            insertCommand.CommandTimeout = int.MaxValue;

            insertCommand.ExecuteNonQuery();

            insertCommand.Dispose();
        }

        public void insertIntoMergeAssociated(int mergeIdCounter, int peerId)
        {
            MySqlCommand insertCommand = mySqlConnection.CreateCommand();
            insertCommand.CommandText = "INSERT INTO mergeassociated(mergeId,peerId) " + "VALUES(?mergeId, ?peerId);";
            insertCommand.Parameters.Add("?mergeId", MySqlDbType.Int32).Value = mergeIdCounter;
            insertCommand.Parameters.Add("?peerId", MySqlDbType.Int32).Value = peerId;
            insertCommand.CommandTimeout = int.MaxValue;

            insertCommand.ExecuteNonQuery();
            /*
            try
            {
                insertCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (e.Number != 1062)
                    throw e;
            }
            */
            insertCommand.Dispose();
        }

        public void insertIntoMergeAssociatedBulk(string insertString)
        {
            MySqlCommand insertCommand = mySqlConnection.CreateCommand();
            insertCommand.CommandText = insertString;
            insertCommand.CommandTimeout = int.MaxValue;

            insertCommand.ExecuteNonQuery();
            insertCommand.Dispose();
        }

        public string findKickAssAffiliation(int IMDbId)
        {
            string kickAssAffiliation = "";

            mySqlConnection.Open();

            MySqlCommand findKickAssAffiliation = mySqlConnection.CreateCommand();
            findKickAssAffiliation.CommandText = "SELECT kickAssAffiliation FROM bachelor.torrents WHERE IMDbId = ?IMDbId && kickAssAffiliation != '' LIMIT 1;";
            findKickAssAffiliation.Parameters.Add("?IMDbId", MySqlDbType.Int32).Value = IMDbId;
            findKickAssAffiliation.CommandTimeout = int.MaxValue;
            MySqlDataReader reader = findKickAssAffiliation.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                kickAssAffiliation = reader.GetString("kickAssAffiliation");
            }

            findKickAssAffiliation.Dispose();
            reader.Close();
            mySqlConnection.Close();

            return kickAssAffiliation;
        }

        public int findIMDbId(string kickAssAffiliation)
        {
            int IMDbId = 0;

            mySqlConnection.Open();

            MySqlCommand findIMDbId = mySqlConnection.CreateCommand();
            findIMDbId.CommandText = "SELECT IMDbId FROM bachelor.torrents WHERE IMDbId != 0 && kickAssAffiliation = ?affili LIMIT 1;";
            findIMDbId.Parameters.Add("?affili", MySqlDbType.String).Value = kickAssAffiliation;
            findIMDbId.CommandTimeout = int.MaxValue;
            MySqlDataReader reader = findIMDbId.ExecuteReader(); //MySqlException: Timeout
            if (reader.HasRows)
            {
                reader.Read();
                IMDbId = reader.GetInt32("IMDbId");
            }

            findIMDbId.Dispose();
            reader.Close();
            mySqlConnection.Close();

            return IMDbId;
        }


    }
}

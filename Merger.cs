using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace DatabaseMerger
{

    class Merger
    {
        private MySqlHandler mySqlHandler;

        private int maxTorrentId;
        private int maxPeerId;

        private bool[] lookedAt;
        private Torrent[] torrents;
        private List<int>[] associated;

        private int contributersThreshold;
        private int mergeIdCounter;
        private int found;

        private Dictionary<string, List<int>> kickAssToTorrentId;
        private Dictionary<int, List<int>> IMDbToTorrentId;

        private DateTime startingTime;
        private int done;
        

        public Merger()
        {
            mySqlHandler = new MySqlHandler();

            contributersThreshold = 45;
            mergeIdCounter = 1;

            startingTime = DateTime.Now;
            done = 0;

            kickAssToTorrentId = new Dictionary<string, List<int>>();
            IMDbToTorrentId = new Dictionary<int, List<int>>();
        }

        public void start()
        {
            Program.debug("Starting to analyze torrents database...");

            torrents = mySqlHandler.getTorrents(contributersThreshold, out maxTorrentId, out found);
            lookedAt = new bool[maxTorrentId + 1];

            Program.debug("Finished reading all torrents which have more than " + contributersThreshold + " contributers...");
            Program.debug("We found " + found + " torrents");

            Program.foundTorrents = found;

            Program.debug("Now starting to read associated database...");
            associated = mySqlHandler.getAssociated(maxTorrentId, out maxPeerId);
            
            Program.debug("Finished analyzing the associated database");

            kickAssToTorrentId = mySqlHandler.kickAssToTorrentId;
            IMDbToTorrentId = mySqlHandler.IMDbToTorrentId;

            Program.debug("Starting the merging process...");

            merging();

            Program.debug("Finished");
        }

        public void merging()
        {
            for (int id = 1; id < maxTorrentId + 1; id++)
            {
                Torrent torrent = torrents[id];

                if (lookedAt[id] == false && torrent != null)
                {
                    bool[] peerDownloadsTorrent = new bool[maxPeerId+1];

                    mySqlHandler.openConnection();

                    int IMDbId = torrent.IMDbId;
                    string kickAssAffiliation = torrent.kickAssAffiliation;

                    lookedAt[id] = true;

                    //Program.debug("We're looking at torrent: " + id + "...");

                    if (IMDbId == 0 && kickAssAffiliation == "")
                    {
                        mySqlHandler.insertIntoMergeTorrents(mergeIdCounter, id);

                        for (int j = 0; j < associated[id].Count(); j++)
                        {
                            int peerId = associated[id][j];
                            mySqlHandler.insertIntoMergeAssociated(mergeIdCounter, peerId);
                        }

                    }

                    else
                    {
                        //mySqlHandler.insertIntoMergeTorrents(mergeIdCounter, id);

                        if (IMDbId != 0 && kickAssAffiliation == "")
                        {
                            //kickAssAffiliation = mySqlHandler.findKickAssAffiliation(IMDbId);
                            if (mySqlHandler.IMDbIdToKickAssAffiliation.ContainsKey(IMDbId))
                                kickAssAffiliation = mySqlHandler.IMDbIdToKickAssAffiliation[IMDbId];
                        }

                        else if (IMDbId == 0 && kickAssAffiliation != "")
                        {
                            //IMDbId = mySqlHandler.findIMDbId(kickAssAffiliation);
                            if (mySqlHandler.kickAssAffiliationToIMDbId.ContainsKey(kickAssAffiliation))
                                IMDbId = mySqlHandler.kickAssAffiliationToIMDbId[kickAssAffiliation];
                        }

                        else //IMDbId and kickAssAffiliation are known from the beginning
                        { }

                        //From this point on we know the IMDbId and the kickAssAffiliation if they exist (at least one of them has to exist)

                        string insertString = "INSERT INTO mergeassociated (mergeId,peerId) VALUES ";

                        if (kickAssToTorrentId.ContainsKey(kickAssAffiliation))
                        {
                            List<int> sameKickAssAffiliation = kickAssToTorrentId[kickAssAffiliation];

                            for (int j = 0; j < sameKickAssAffiliation.Count(); j++)
                            {
                                int torrent2Id = sameKickAssAffiliation[j];
                                Torrent torrent2 = torrents[torrent2Id];
                                if (torrent2 != null && !lookedAt[torrent2Id])
                                {
                                    mySqlHandler.insertIntoMergeTorrents(mergeIdCounter, torrent2Id);

                                    for (int k = 0; k < associated[torrent2Id].Count(); k++)
                                    {
                                        int peerId = associated[torrent2Id][k];
                                        if (peerDownloadsTorrent[peerId] == false)
                                        {
                                            //mySqlHandler.insertIntoMergeAssociated(mergeIdCounter, peerId);
                                            insertString += "(" + mergeIdCounter + "," + peerId + "), ";
                                            peerDownloadsTorrent[peerId] = true;
                                        }
                                    }
                                    lookedAt[torrent2Id] = true;
                                    done += 1;
                                }
                            }
                        }

                        if (IMDbToTorrentId.ContainsKey(IMDbId))
                        {
                            List<int> sameIMDbId = IMDbToTorrentId[IMDbId];

                            for (int j = 0; j < sameIMDbId.Count(); j++)
                            {
                                int torrent2Id = sameIMDbId[j];
                                Torrent torrent2 = torrents[torrent2Id];
                                if (torrent2 != null && !lookedAt[torrent2Id])
                                {
                                    mySqlHandler.insertIntoMergeTorrents(mergeIdCounter, torrent2Id);

                                    for (int k = 0; k < associated[torrent2Id].Count(); k++)
                                    {
                                        int peerId = associated[torrent2Id][k];
                                        if (peerDownloadsTorrent[peerId] == false)
                                        {
                                            //mySqlHandler.insertIntoMergeAssociated(mergeIdCounter, peerId);
                                            insertString += "(" + mergeIdCounter + "," + peerId + "), ";
                                            peerDownloadsTorrent[peerId] = true;
                                        }
                                    }
                                    lookedAt[torrent2Id] = true;
                                    done += 1;
                                }
                            }
                        }

                        if (insertString != "INSERT INTO mergeassociated (mergeId,peerId) VALUES ")
                        {
                            insertString = insertString.Remove(insertString.Length - 2);
                            insertString += ";";
                            mySqlHandler.insertIntoMergeAssociatedBulk(insertString);
                        }
                    }

                    mySqlHandler.closeConnection();

                    mergeIdCounter += 1;
                    done += 1;
                }

                else
                {
                    //already looked at this torrent or the torrent doesn't qualify since it has too few seeders/leechers
                }

                updateUI(id,done);
            }
        
        }

        private void updateUI(int id, int done)
        {
            TimeSpan timeRunning = DateTime.Now - startingTime;
            Program.updateTimeRunning(timeRunning);

            Program.setCurrentIdLabel(id);
            Program.setProgressLabel(done);
        }
    }
}

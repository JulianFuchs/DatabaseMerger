using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseMerger
{
    class Torrent
    {
        public int id;
        public int IMDbId;
        public string kickAssAffiliation;

        public Torrent(int id, int IMDbId, string kickAssAffiliation)
        {
            this.id = id;
            this.IMDbId = IMDbId;
            this.kickAssAffiliation = kickAssAffiliation;
        }

    }
}

using MessagePack;
using LiteDB;

namespace Generated.DAO
{

    [MessagePackObject(true)]
    [JsonMapping("Bgm.json")]
    public partial class BgmDao
    {
        [BsonId]
        public long Id { get; set; }
        public string Bgm { get; set; }
        public long Pitch { get; set; }
        public bool IsLoop { get; set; }
    }
}

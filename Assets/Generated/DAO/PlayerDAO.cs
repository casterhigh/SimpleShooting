using MessagePack;
using LiteDB;

namespace Generated.DAO
{

    [MessagePackObject(true)]
    [JsonMapping("Player.json")]
    public partial class PlayerDao
    {
        [BsonId]
        public long Id { get; set; }
        public long Hp { get; set; }
        public long Power { get; set; }
        public long Defense { get; set; }
        public long Speed { get; set; }
    }
}

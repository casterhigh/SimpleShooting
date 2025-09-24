using MessagePack;
using LiteDB;

namespace Generated.DAO
{

    [MessagePackObject(true)]
    [JsonMapping("Enemy.json")]
    public partial class EnemyDao
    {
        [BsonId]
        public long Id { get; set; }
        public long Hp { get; set; }
        public long Power { get; set; }
        public long Defense { get; set; }
        public long Radius { get; set; }
        public long Speed { get; set; }
        public bool IsBoss { get; set; }
    }
}

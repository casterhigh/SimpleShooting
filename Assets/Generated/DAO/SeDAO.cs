using MessagePack;
using LiteDB;

namespace Generated.DAO
{

    [MessagePackObject(true)]
    [JsonMapping("Se.json")]
    public partial class SeDao
    {
        [BsonId]
        public long Id { get; set; }
        public string Se { get; set; }
    }
}

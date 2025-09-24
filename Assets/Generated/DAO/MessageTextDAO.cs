using MessagePack;
using LiteDB;

namespace Generated.DAO
{

    [MessagePackObject(true)]
    [JsonMapping("MessageText.json")]
    public partial class MessageTextDao
    {
        [BsonId]
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}

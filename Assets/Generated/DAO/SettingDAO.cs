using MessagePack;
using LiteDB;

namespace Generated.DAO
{

    [MessagePackObject(true)]
    [JsonMapping("Setting.json")]
    public partial class SettingDao
    {
        [BsonId]
        public long Id { get; set; }
        public string Key { get; set; }
        public double Value { get; set; }
    }
}

namespace Infrastructure.Messaging.Sound.Request
{
    public record AudioSettingRequest : IMessage
    {
        public float BgmVolume { get; }

        public float SeVolume { get; }

        public AudioSettingRequest(float bgmVolume, float seVolume)
        {
            BgmVolume = bgmVolume;
            SeVolume = seVolume;
        }
    }
}

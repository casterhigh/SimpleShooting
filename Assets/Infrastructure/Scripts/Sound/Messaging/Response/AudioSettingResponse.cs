namespace Infrastructure.Messaging.Sound.Response
{
    public record AudioSettingResponse : IMessage
    {
        public float BgmVolume { get; }

        public float SeVolume { get; }

        public AudioSettingResponse(float bgmVolume, float seVolume)
        {
            BgmVolume = bgmVolume;
            SeVolume = seVolume;
        }
    }
}

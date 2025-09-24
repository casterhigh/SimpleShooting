using Infrastructure.Domain.Interface.Repository;

namespace Infrastructure.Sound.Domain.Interface
{
    public interface IAudioRepository : ILoadRepository
    {
        void CreateBgmDto();

        void CreateSeDto();

        void UpdateAudioSetting(float bgmVolume, float seVolume);
    }
}

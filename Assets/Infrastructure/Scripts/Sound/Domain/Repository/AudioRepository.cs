using MessagePipe;
using Infrastructure.Sound.Domain.DTO;
using Infrastructure.Sound.Domain.Interface;
using Infrastructure.Messaging;
using Infrastructure.DB.Interface;
using Infrastructure.Domain;
using Infrastructure.Messaging.Sound.Response;
using Generated.DAO;
using Generated;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Sound.Domain.Repository
{
    public class AudioRepository : IAudioRepository
    {
        readonly IPublisher<IMessage> publisher;

        readonly IDBConnector dBConnector;

        // Key:Id, Value:Bgm
        Dictionary<ID, BgmDao> bgmClipNames = new();

        // Key:Id, Value:Se
        Dictionary<ID, SeDao> seClipNames = new();

        SettingDao bgmVolume;

        SettingDao seVolume;

        public AudioRepository(IPublisher<IMessage> publisher, IDBConnector dBConnector)
        {
            this.publisher = publisher;
            this.dBConnector = dBConnector;
        }

        public void Load()
        {
            bgmClipNames = dBConnector.FindAll<BgmDao>().ToDictionary(dao => new ID(dao.Id), dao => dao);
            seClipNames = dBConnector.FindAll<SeDao>().ToDictionary(dao => new ID(dao.Id), dao => dao);
            bgmVolume = dBConnector.FindOne<SettingDao>(dao => dao.Key == SettingKeys.BgmVolume);
            seVolume = dBConnector.FindOne<SettingDao>(dao => dao.Key == SettingKeys.SeVolume);
            PublishAudioSetting();
        }

        public void CreateBgmDto()
        {
            var bgmDtoList = bgmClipNames.Select(kpv => new BgmDTO(kpv.Value)).ToList();
            publisher.Publish(new LoadBgmResponse(bgmDtoList));
        }

        public void CreateSeDto()
        {
            var seDtoList = seClipNames.Select(kpv => new SeDTO(kpv.Value)).ToList();
            publisher.Publish(new LoadSeResponse(seDtoList));
        }

        public void UpdateAudioSetting(float bgmVolume, float seVolume)
        {
            this.bgmVolume.Value = bgmVolume;
            this.seVolume.Value = seVolume;

            dBConnector.Upsert(this.bgmVolume);
            dBConnector.Upsert(this.seVolume);

            PublishAudioSetting();
        }

        void PublishAudioSetting()
        {
            publisher.Publish(new AudioSettingResponse((float)bgmVolume.Value, (float)seVolume.Value));
        }
    }
}
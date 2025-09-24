using Generated.DAO;
using Infrastructure.Domain;
using UnityEngine;

namespace Infrastructure.Sound.Domain.DTO
{
    public class BgmDTO
    {
        public ID Id { get; }

        public string ClipName { get; }

        public int Pitch { get; }

        public bool IsLoop { get; }

        public AudioClip Clip { get; private set; }

        public BgmDTO(BgmDao dao)
        {
            Id = new ID(dao.Id);
            ClipName = dao.Bgm;
            Pitch = (int)dao.Pitch;
            IsLoop = dao.IsLoop;
        }

        public void SetAudioClip(AudioClip clip)
        {
            Clip = clip;
        }
    }
}

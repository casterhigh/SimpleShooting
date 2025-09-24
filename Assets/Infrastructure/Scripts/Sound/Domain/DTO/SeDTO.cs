using Generated.DAO;
using Infrastructure.Domain;
using UnityEngine;

namespace Infrastructure.Sound.Domain.DTO
{
    public class SeDTO
    {
        public ID Id { get; }

        public string ClipName { get; }

        public AudioClip Clip { get; private set; }

        public SeDTO(SeDao dao)
        {
            Id = new ID(dao.Id);
            ClipName = dao.Se;
        }

        public void SetAudioClip(AudioClip clip)
        {
            Clip = clip;
        }
    }
}

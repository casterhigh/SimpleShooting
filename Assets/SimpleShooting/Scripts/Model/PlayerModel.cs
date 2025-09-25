using Infrastructure.Domain;
using SimpleShooting.Domain.DTO;
using SimpleShooting.Domain.Interface;
using SimpleShooting.Model.Interface;
using UnityEngine;

namespace SimpleShooting.Model
{
    public class PlayerModel : IPlayerModel
    {
        readonly IPlayerRepository repository;

        int gameEnemyId;

        public PlayerModel(IPlayerRepository repository)
        {
            this.repository = repository;
        }

        public PlayerDTO CrateDto(Vector3 position)
        {
            var id = new ID(1);
            return new PlayerDTO();
        }

        public PlayerDTO ReceiveDamage(PlayerDTO dto)
        {
            return dto;
        }
    }
}

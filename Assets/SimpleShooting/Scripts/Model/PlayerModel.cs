using Infrastructure.Domain;
using SimpleShooting.Domain.DTO;
using SimpleShooting.Domain.Interface;
using SimpleShooting.Model.Interface;
using UnityEngine;

namespace SimpleShooting.Model
{
    public class PlayerModel : IPlayerModel
    {
        readonly IPlayerRepository playerRepository;

        readonly IPlayerCalculator playerCalculator;

        public PlayerModel(IPlayerRepository playerRepository,
        IPlayerCalculator playerCalculator)
        {
            this.playerRepository = playerRepository;
            this.playerCalculator = playerCalculator;
        }

        public PlayerDTO CrateDto(Vector3 position)
        {
            var id = new ID(1);
            var dao = playerRepository.Get();
            return dao.CreateDTO(position);
        }

        public PlayerDTO ReceiveDamage(PlayerDTO player, EnemyDTO enemy)
        {
            var damage = playerCalculator.ReceiveDamage(player, enemy);
            player.UpdateHp(damage);
            return player;
        }
    }
}

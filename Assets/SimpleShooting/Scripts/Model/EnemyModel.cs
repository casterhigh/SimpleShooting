using Infrastructure.Domain;
using Infrastructure.Messaging;
using MessagePipe;
using SimpleShooting.Domain.DTO;
using SimpleShooting.Domain.Interface;
using SimpleShooting.Model.Interface;
using UnityEngine;

namespace SimpleShooting.Model
{
    public class EnemyModel : IEnemyModel
    {
        readonly IEnemyRepository repository;

        int gameEnemyId;

        public EnemyModel(IEnemyRepository repository)
        {
            this.repository = repository;
        }

        public EnemyDTO CrateDto(Vector3 position)
        {
            gameEnemyId++;
            var id = new ID(gameEnemyId);
            return new EnemyDTO();
        }

        public EnemyDTO ReceiveDamage(EnemyDTO dto)
        {
            return dto;
        }
    }
}

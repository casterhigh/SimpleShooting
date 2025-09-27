using Infrastructure.Domain;
using Infrastructure.Messaging;
using MessagePipe;
using SimpleShooting.Domain.DTO;
using SimpleShooting.Domain.Interface;
using SimpleShooting.Model.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Model
{
    public class EnemyModel : IEnemyModel, IDisposable
    {
        readonly IEnemyRepository enemyRepository;

        readonly Dictionary<ID, EnemyDTO> enemies;

        readonly IEnemyCalculator enemyCalculator;

        int gameEnemyId;

        public EnemyModel(IEnemyRepository enemyRepository,
        IEnemyCalculator enemyCalculator)
        {
            this.enemyRepository = enemyRepository;
            this.enemyCalculator = enemyCalculator;
            enemies = new();
        }

        public void Dispose()
        {
            enemies.Clear();
        }

        public EnemyDTO CrateDto(Vector3 position, bool isBoss)
        {
            gameEnemyId++;
            var id = new ID(gameEnemyId);
            var dao = enemyRepository.PickUpEnemy(isBoss);
            return dao.CreateDTO(id, position);
        }

        public EnemyDTO ReceiveDamage(EnemyDTO enemy, PlayerDTO player)
        {
            var damage = enemyCalculator.ReceiveDamage(enemy, player);
            enemy.UpdateHp(damage);
            return enemy;
        }
    }
}

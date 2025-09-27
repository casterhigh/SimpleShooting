using Generated.DAO;
using Infrastructure.DB.Interface;
using Infrastructure.Domain;
using SimpleShooting.Domain.DTO;
using SimpleShooting.Domain.Interface;
using SimpleShooting.Model.Interface;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleShooting.Domain.Repository
{
    public class EnemyRepository : IEnemyRepository
    {
        readonly IDBConnector dBConnector;

        Dictionary<ID, EnemyDao> enemies = new();

        public EnemyRepository(IDBConnector dBConnector)
        {
            this.dBConnector = dBConnector;
        }

        public void Load()
        {
            var enemies = dBConnector.FindAll<EnemyDao>();
            foreach (var enemy in enemies)
            {
                this.enemies.Add(new ID(enemy.Id), enemy);
            }
        }

        public EnemyDao PickUpEnemy(bool isBoss)
        {
            if (isBoss)
            {
                var bossEnemies = enemies.Values.Where(enemy => enemy.IsBoss).ToList();
                var index = Random.Range(0, bossEnemies.Count);
                return bossEnemies[index];
            }
            else
            {
                var normalEnemies = enemies.Values.Where(enemy => !enemy.IsBoss).ToList();
                var index = Random.Range(0, normalEnemies.Count);
                return normalEnemies[index];
            }
        }
    }
}

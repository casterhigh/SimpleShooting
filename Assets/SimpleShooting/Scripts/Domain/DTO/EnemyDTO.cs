using Generated.DAO;
using Infrastructure.Domain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Domain.DTO
{
    public class EnemyDTO
    {
        public ID GameEnemyId { get; }

        public int MaxHp { get; }

        public int CurrentHp { get; private set; }

        public int Power { get; }

        public int Defense { get; }

        public int Radius { get; }

        public int Speed { get; }

        public bool IsBoss { get; }

        public bool IsDead => CurrentHp <= 0;

        public Vector3 SpawnPosition { get; }

        public EnemyDTO(ID gameEnemyId, int hp, int power, int defense, int radius, int speed, bool isBoss, Vector3 spawnPosition)
        {
            GameEnemyId = gameEnemyId;
            MaxHp = hp;
            CurrentHp = hp;
            Power = power;
            Defense = defense;
            Radius = radius;
            Speed = speed;
            IsBoss = isBoss;
            SpawnPosition = spawnPosition;
        }

        public void UpdateHp(int damage)
        {
            CurrentHp -= damage;
        }
    }

    public static class EnemyDaoExtension
    {
        public static EnemyDTO CreateDTO(this EnemyDao dao, ID id, Vector3 spawnPosition)
        {
            return new EnemyDTO(id, (int)dao.Hp, (int)dao.Power, (int)dao.Defense, (int)dao.Radius, (int)dao.Speed, dao.IsBoss, spawnPosition);
        }
    }
}

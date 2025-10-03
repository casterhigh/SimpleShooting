using Generated.DAO;
using Infrastructure.Domain;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Domain.DTO
{
    public class PlayerDTO
    {
        public ID Id { get; }
        public int Hp { get; }
        public int CurrentHp { get; private set; }
        public int Power { get; }
        public int Defense { get; }
        public int Speed { get; }
        public bool IsDead => CurrentHp <= 0;
        public Vector3 SpawnPosition { get; }

        public PlayerDTO(ID id, int hp, int power, int defense, int speed, Vector3 spawnPosition)
        {
            Id = id;
            Hp = hp;
            CurrentHp = hp;
            Power = power;
            Defense = defense;
            Speed = speed;
            SpawnPosition = spawnPosition;
        }

        public void UpdateHp(int damage)
        {
            CurrentHp -= damage;
        }
    }

    public static class PlayerDaoExtension
    {
        public static PlayerDTO CreateDTO(this PlayerDao dao, ID id, Vector3 spawnPosition)
        {
            return new PlayerDTO(id, (int)dao.Hp, (int)dao.Power, (int)dao.Defense, (int)dao.Speed, spawnPosition);
        }
    }
}

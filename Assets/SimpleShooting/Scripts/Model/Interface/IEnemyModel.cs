using Infrastructure.Domain;
using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Model.Interface
{
    public interface IEnemyModel
    {
        EnemyDTO CrateDto(Vector3 position, bool isBoss);

        EnemyDTO ReceiveDamage(ID id, PlayerDTO player);

        EnemyDTO GetEnemyDTO(ID id);
    }
}

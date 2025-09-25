using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Model.Interface
{
    public interface IEnemyModel
    {
        EnemyDTO CrateDto(Vector3 position);

        EnemyDTO ReceiveDamage(EnemyDTO dto);
    }
}

using SimpleShooting.Domain.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleShooting.Model.Interface
{
    public interface IPlayerModel
    {
        PlayerDTO CrateDto(Vector3 position);

        PlayerDTO ReceiveDamage(PlayerDTO player, EnemyDTO enemy);
    }
}

using SimpleShooting.Domain.DTO;
using SimpleShooting.Model.Interface;

namespace SimpleShooting.Model.Calculator
{
    /// <summary>
    /// todo:ちゃんとする
    /// </summary>
    public class GameCalculator : IEnemyCalculator, IPlayerCalculator
    {
        public int ReceiveDamage(EnemyDTO enemy, PlayerDTO player)
        {
            return 10;
        }

        public int ReceiveDamage(PlayerDTO player, EnemyDTO enemy)
        {
            return 10;
        }
    }
}

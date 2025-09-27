using SimpleShooting.Domain.DTO;

namespace SimpleShooting.Model.Interface
{
    public interface IEnemyCalculator
    {
        int ReceiveDamage(EnemyDTO enemy, PlayerDTO player);
    }
}

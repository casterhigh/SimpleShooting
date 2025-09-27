using SimpleShooting.Domain.DTO;

namespace SimpleShooting.Model.Interface
{
    public interface IPlayerCalculator
    {
        int ReceiveDamage(PlayerDTO player, EnemyDTO enemy);
    }
}

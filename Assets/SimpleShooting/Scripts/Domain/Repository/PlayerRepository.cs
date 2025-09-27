using Generated.DAO;
using Infrastructure.DB.Interface;
using SimpleShooting.Domain.Interface;
using SimpleShooting.Model.Interface;
using System;
using System.Linq;

namespace SimpleShooting.Domain.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        readonly IDBConnector dBConnector;
        PlayerDao player;

        public void Load()
        {
            var tmp = dBConnector.FindAll<PlayerDao>().FirstOrDefault();

            if (tmp == null)
            {
                throw new InvalidOperationException("PlayerDaoがありません");
            }

            player = tmp;
        }

        public PlayerDao Get() => player;
    }
}

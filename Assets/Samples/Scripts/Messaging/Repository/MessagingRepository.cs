using Generated.DAO;
using Infrastructure.DB.Interface;
using Sample.Messaging.DTO;
using System;

namespace Sample.Messaging.Repository
{
    public class MessagingRepository : IMessagingRepository
    {
        IDBConnector dBConnecter;

        public MessagingRepository(IDBConnector dBConnecter)
        {
            this.dBConnecter = dBConnecter;
        }

        public SampleDTO CreateDTO(long id)
        {
            var dao = dBConnecter.FindById<BgmDao>(id);

            if (dao == null)
            {
                throw new InvalidOperationException($"{id}に対応するデータがありません。");
            }

            return new SampleDTO(dao.Bgm);
        }
    }
}

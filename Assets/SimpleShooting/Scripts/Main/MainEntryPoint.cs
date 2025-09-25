using UnityEngine;
using VContainer.Unity;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

namespace SimpleShooting.Main
{
    public class MainEntryPoint : IInitializable
    {
        readonly ILogger logger;

        public MainEntryPoint(ILogger logger)
        {
            this.logger = logger;
        }

        public void Initialize()
        {
            logger.Log("MainEntryPoint Initialize", Color.red);
        }
    }
}

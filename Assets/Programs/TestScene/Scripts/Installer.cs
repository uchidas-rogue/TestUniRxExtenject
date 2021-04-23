using UnityEngine;
using Zenject;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<IModel>()
            .To<Model>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
}
using UnityEngine;
using Zenject;

public class DangeonSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<IDangeonFieldModel>()
            .To<DangeonFieldModel>()
            .FromNew()
            .AsSingle()
            .NonLazy();
    }
}
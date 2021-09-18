using UnityEngine;
using Zenject;

public class DangeonSceneInstaller : MonoInstaller
{
    public override void InstallBindings ()
    {
        Container
            .Bind<IDangeonFieldModel> ()
            .To<DangeonFieldModel> ()
            .FromNew ()
            .AsSingle ()
            .NonLazy ();

        Container
            .Bind<IPlayerModel> ()
            .To<PlayerModel> ()
            .FromNew ()
            .AsSingle ()
            .NonLazy ();

        Container
            .Bind<IMiniMapModel> ()
            .To<MiniMapModel> ()
            .FromNew ()
            .AsSingle ()
            .NonLazy ();

        Container
            .Bind<IMiniMapStringService> ()
            .To<MiniMapStringService> ()
            .FromNew ()
            .AsCached ()
            .NonLazy ();

        Container
            .Bind<IMoveObjectServece> ()
            .To<MoveObjectServece> ()
            .FromNew ()
            .AsCached ()
            .NonLazy ();
    }
}
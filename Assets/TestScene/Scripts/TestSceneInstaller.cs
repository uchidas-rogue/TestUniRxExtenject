using UnityEngine;
using Zenject;

public class TestSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        
        Container
            .Bind<IInputable>() // IInputable型のインスタンスをinjectの対象にする
            .To<InputFromKeyboard>() // InputFromKeyboardのインスタンスを注入
            .AsSingle(); // インスタンスはsingleton
    }
}
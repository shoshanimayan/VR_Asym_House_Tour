using UnityEngine;
using Zenject;
using Core;
using Utility;
using Signals;
using Gameplay;
using Audio;
using General;
using Network;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        //binding
        Container.Bind<StateManager>().AsSingle();
        Container.Bind<GameSettings>().AsSingle();

        Container.BindMediatorView<MusicMediator, MusicView>();
        Container.BindMediatorView<SoundEffectMediator, SoundEffectView>();
        Container.BindMediatorView<HardwareDetermineMediator, HardwareDetermineView>();
        Container.BindMediatorView<BasicSpawnerMediator, BasicSpawnerView>();


        //signals
        Container.DeclareSignal<StateChangeSignal>();
        Container.DeclareSignal<StateChangedSignal>();
        Container.DeclareSignal<AudioBlipSignal>();
        Container.DeclareSignal<SetHardWareSignal>();


    }
}
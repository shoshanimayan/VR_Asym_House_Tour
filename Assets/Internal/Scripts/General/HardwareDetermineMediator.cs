using UnityEngine;
using Core;
using Zenject;
using UniRx;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Management;
using Signals;
using Gameplay;

namespace General
{
    public enum Hardware { VR, NotVR}

    public class HardwareDetermineMediator: MediatorBase<HardwareDetermineView>, IInitializable, IDisposable
	{

		///  INSPECTOR VARIABLES       ///

		///  PRIVATE VARIABLES         ///

		///  PRIVATE METHODS           ///

		///  LISTNER METHODS           ///

		///  PUBLIC API                ///
		///  IMPLEMENTATION            ///

		[Inject]

		private SignalBus _signalBus;

		[Inject] private GameSettings _gameSettings;

		readonly CompositeDisposable _disposables = new CompositeDisposable();

		public void Initialize()
		{
			if (XRGeneralSettings.Instance.Manager.activeLoader != null)
			{
                //	_view.VRView.SetActive(true);
                //	_view.NoneVRView.SetActive(false);
                _gameSettings.SetHardware(Hardware.VR);

                _signalBus.Fire( new SetHardWareSignal(){ Hardware = Hardware.VR});
			}
			else
			{
             //   _view.VRView.SetActive(false);
              //  _view.NoneVRView.SetActive(true);
                _gameSettings.SetHardware(Hardware.NotVR);

                _signalBus.Fire(new SetHardWareSignal() { Hardware = Hardware.NotVR});

            }
        }

		public void Dispose()
		{

			_disposables.Dispose();

		}

	}
}

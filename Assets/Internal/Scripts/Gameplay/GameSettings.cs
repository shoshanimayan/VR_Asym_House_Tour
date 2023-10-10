using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using System;
using UniRx;
using Signals;
using Core;
using General;

namespace Gameplay
{
	public class GameSettings:  IDisposable
	{

		///  INSPECTOR VARIABLES      ///

		///  PRIVATE VARIABLES         ///

		private Hardware _hardware;
        ///  PRIVATE METHODS          ///





        ///  LISTNER METHODS          ///



        ///  PUBLIC API               ///

        public void SetHardware(Hardware hardware)
        {
            _hardware = hardware;
        }

        public bool IsVR()
		{ 
			return _hardware==Hardware.VR;
		}



		///    Implementation        ///



		readonly SignalBus _signalBus;

		readonly CompositeDisposable _disposables = new CompositeDisposable();

		public GameSettings(SignalBus signalBus)
		{
			_signalBus = signalBus;
 


        }

		



        public void Dispose()
		{

			_disposables.Dispose();

		}
	}
}

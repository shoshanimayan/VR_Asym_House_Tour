using UnityEngine;
using Core;
using Zenject;
using UniRx;
using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;

namespace Network
{
	public class BasicSpawnerMediator: MediatorBase<BasicSpawnerView>, IInitializable, IDisposable
	{

		///  INSPECTOR VARIABLES       ///

		///  PRIVATE VARIABLES         ///

		///  PRIVATE METHODS           ///

		///  LISTNER METHODS           ///

		///  PUBLIC API                ///

		public bool IsVR() { return _settings.IsVR(); }

		///  IMPLEMENTATION            ///

		[Inject]

		private SignalBus _signalBus;
        [Inject] private GameSettings _settings;


        readonly CompositeDisposable _disposables = new CompositeDisposable();


		public void Initialize()
		{
			_view.Init(this);
		}

		public void Dispose()
		{

			_disposables.Dispose();

		}

	}
}

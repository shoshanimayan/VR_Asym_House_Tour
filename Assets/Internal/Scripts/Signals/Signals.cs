using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core;
using General;

namespace Signals
{


	public class StateChangeSignal
	{
		public State ToState;
	}

	public class StateChangedSignal
	{
		public State ToState;
	}


	public class LoadEnvironmentSignal
	{
		public int Env;
	}

	public class EnvironmentLoadedSignal
	{
		public int Env;
	}


    public class SetHardWareSignal
    {
        public Hardware Hardware;
    }


    public class AudioBlipSignal
	{
		public string clipName;
	}

	

	

}

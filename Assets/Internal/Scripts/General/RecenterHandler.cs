using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Management;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;
using System;

namespace General
{
	public class RecenterHandler : MonoBehaviour
	{
		private void Start()
		{
			Recenter();

		}

		private void Awake()
		{
			Recenter();
		}

		private void OnEnable()
		{
			Recenter();
		}

		private void OnBecameVisible()
		{
			Recenter();

		}

		

		private bool _recentered;


		[SerializeField] private Transform _target;
		[SerializeField] private Transform _head;
		[SerializeField] private Transform _origin;

		[SerializeField] private InputActionProperty _recenterButton;

		[SerializeField] private GameObject[] _spots;

		private void Recenter()
		{
			if (_spots.Length == 0)
			{
				var spot =GameObject.Find("Spots");
				List<GameObject> spots = new List<GameObject>();
				foreach (Transform s in spot.transform)
				{
					spots.Add(s.gameObject);
				}
				_spots = spots.ToArray();

			}
			if (_target == null)
			{
				_target = _spots[0].transform;
			}
			Debug.Log("Recentered");
			Vector3 offset = _head.position - _origin.position;
			offset.y = 0;
			_origin.position = _target.position - offset;
			_origin.parent.transform.position = _target.position - offset;
			Vector3 targetForward = _target.forward;
			targetForward.y = 0;
			Vector3 cameraForwrd = _head.forward;
			cameraForwrd.y = 0;

			float angle = Vector3.SignedAngle(cameraForwrd, targetForward, Vector3.up);
			_origin.RotateAround(_head.position, Vector3.up, angle);
		}

		public void RecenterToPostition(int index)
		{
            
            _target = _spots[index].transform;
            
            Debug.Log("teleported: "+_target.position.ToString());
            Vector3 offset = _head.position - _origin.position;
            offset.y = 0;
            _origin.parent.transform.position = _target.position ;
            _origin.localPosition= Vector3.zero;

            //_origin.parent.position=_target.position - offset;
            Vector3 targetForward = _target.forward;
            targetForward.y = 0;
            Vector3 cameraForwrd = _head.forward;
            cameraForwrd.y = 0;

            float angle = Vector3.SignedAngle(cameraForwrd, targetForward, Vector3.up);
            _origin.RotateAround(_head.position, Vector3.up, angle);
        }

		private void Update()
		{
            if (!_recentered)
            {
                _recentered = true;
                Recenter();
            }
            /*
			

			if (_recenterButton.action.WasPerformedThisFrame())
			{
				Recenter();
			}*/
        }

	}
}

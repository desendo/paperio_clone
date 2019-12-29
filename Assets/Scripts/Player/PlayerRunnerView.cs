using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;

namespace Game
{
    public class PlayerRunnerView : MonoBehaviour
    {    
        Renderer[] _renderers;
        Transform _transform;
        
        [Inject]
        PlayerFacade playerFacade;

        public void Awake()
        {
            _transform = transform;
            _renderers = _transform.GetComponentsInChildren<Renderer>();

        }
        public Vector3 LookDir
        {
            get { return transform.right; }
            set {
                transform.right = value; }
        }
        public float Rotation
        {
            
            get { return _transform.rotation.eulerAngles.z; }
            set {
                _transform.eulerAngles = new Vector3(_transform.rotation.eulerAngles.x, _transform.rotation.eulerAngles.y,value); }
        }

        public Vector3 Position
        {
            get { return _transform.position; }
            set { _transform.position = value; }
        }

    }
}
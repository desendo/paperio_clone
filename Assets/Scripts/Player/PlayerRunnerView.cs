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
        Transform _transform;
        
        [Inject]
        PlayerFacade playerFacade;
        [SerializeField] GameObject crown;
        public void Awake()
        {
            _transform = transform;
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
        public void SetCrown(bool isOn)
        {
            crown.SetActive(isOn);
        }
        public Vector3 Position
        {
            get { return _transform.position; }
            set { _transform.position = value; }
        }

    }
}
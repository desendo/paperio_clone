using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class Player : MonoBehaviour
    {

        Rigidbody2D _rigidBody;        
        Renderer[] _renderers;
        Transform _transform;
        private void Awake()
        {
            _transform = transform;
            _renderers = _transform.GetComponentsInChildren<Renderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        [Inject]
        public PlayerFacade Facade
        {
            get; set;
        }
        public Renderer[] Renderers
        {
            get { return _renderers; }
        }
        public Vector2 LookDir
        {
            get { return _rigidBody.transform.right; }
        }

        public float Rotation
        {
            get { return _transform.rotation.eulerAngles.z; }
            set { _transform.eulerAngles = new Vector3(_transform.rotation.eulerAngles.x, _transform.rotation.eulerAngles.y,value); }
        }
        public bool IsOutside
        {
            get; set;
        }
        public bool IsCutOf
        {
            get; set;
        }
        public bool IsDead
        {
            get; set;
        }
        public Vector2 Position
        {
             get { return _transform.position; }
             set { _transform.position = value; }
        }
    }
}
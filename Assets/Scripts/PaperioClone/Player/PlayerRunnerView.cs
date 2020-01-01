using UnityEngine;

namespace PaperIOClone.Player
{
    public class PlayerRunnerView : MonoBehaviour
    {
        private Transform _transform;
        [SerializeField] private GameObject crown;

        public Vector3 LookDir
        {
            get => transform.right;
            set => transform.right = value;
        }

        public float Rotation
        {
            get => _transform.rotation.eulerAngles.z;
            set => _transform.eulerAngles =
                new Vector3(_transform.rotation.eulerAngles.x, _transform.rotation.eulerAngles.y, value);
        }

        public Vector3 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        public void Awake()
        {
            _transform = transform;
        }

        public void SetCrown(bool isOn)
        {
            crown.SetActive(isOn);
        }
    }
}
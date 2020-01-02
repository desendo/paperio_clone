using UnityEngine;

namespace PaperIOClone.Player
{
    public class PlayerRunnerView : MonoBehaviour
    {
        private Transform _transform;
        [SerializeField] private GameObject playerRunnerViewContainer;

        public Vector3 LookDir
        {
            get => transform.right;
            set => transform.right = value;
        }

        public float Rotation
        {
            get => _transform.rotation.eulerAngles.z;
            set
            {
                Quaternion rotation;
                _transform.eulerAngles =
                    new Vector3((rotation = _transform.rotation).eulerAngles.x, rotation.eulerAngles.y, value);
            }
        }

        public Vector3 Position
        {
            get => playerRunnerViewContainer.transform.position;
            set => playerRunnerViewContainer.transform.position = value;
        }

        public void Awake()
        {
            _transform = playerRunnerViewContainer.transform;
        }
    }
}
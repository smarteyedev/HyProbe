using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Seville
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private HingeJoint _doorJoint;
        [SerializeField] private TransformJoint _doorPuller;
        [SerializeField] private float _handleOpenValue = 0.1f;

        [SerializeField]
        float handleCloseValue = 0.5f;

        [SerializeField]
        float hingeCloseAngle = 5.0f;

        JointLimits _openDoorLimits;
        JointLimits _closedDoorLimits;
        bool _closed = false;
        float _lastHandleValue = 1.0f;

        void Start()
        {
            _openDoorLimits = _doorJoint.limits;
            _closedDoorLimits = _openDoorLimits;
            _closedDoorLimits.min = 0.0f;
            _closedDoorLimits.max = 0.0f;
            _doorJoint.limits = _closedDoorLimits;
            _closed = true;
        }

        void Update()
        {
            if (!_closed)
            {
                if (_lastHandleValue < handleCloseValue)
                    return;

                if (Mathf.Abs(_doorJoint.angle) < hingeCloseAngle)
                {
                    _doorJoint.limits = _closedDoorLimits;
                    _closed = true;
                }
            }
        }

        public void BeginDoorPulling(SelectEnterEventArgs args)
        {
            _doorPuller.connectedBody = args.interactorObject.GetAttachTransform(args.interactableObject);
            _doorPuller.enabled = true;
        }

        public void EndDoorPulling()
        {
            _doorPuller.enabled = false;
            _doorPuller.connectedBody = null;
        }

        public void DoorHandleUpdate(float handleValue)
        {
            _lastHandleValue = handleValue;

            if (!_closed)
                return;

            if (handleValue < _handleOpenValue)
            {
                _doorJoint.limits = _openDoorLimits;
                _closed = false;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace Seville
{
    public class SESocketInteractor : XRSocketInteractor
    {
        [Space(5f)]
        [Header("Seville Settings")]
        [SerializeField] private Transform parentArea;
        [SerializeField] private bool isUsingSocketModel = false;

        [Tooltip("make sure you have set objName on XRGrabIntractableTwoAttach")]
        [SerializeField] private List<string> targetObjNames = new List<string>();
        private MeshRenderer m_mesh;

        [Header("UI Assets")]
        [SerializeField] private Sprite activeHoverSprite;
        [SerializeField] private Sprite nonactiveHoverSprite;
        [Header("Component Dependencies")]
        [SerializeField] private Image imageHover;

        protected override void Awake()
        {
            base.Awake();
            m_mesh = GetComponent<MeshRenderer>();
        }

        protected override void Start()
        {
            base.Start();

            if (!isUsingSocketModel) m_mesh.enabled = false;
            ToggleImageHover(false);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(OnObjectSelectEntered);
            selectExited.AddListener(OnObjectSelectExit);
            hoverEntered.AddListener(OnObjectHoverEnter);
            hoverExited.AddListener(OnObjectHoverExit);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            selectEntered.RemoveListener(OnObjectSelectEntered);
            selectExited.RemoveListener(OnObjectSelectExit);
            hoverEntered.RemoveListener(OnObjectHoverEnter);
            hoverExited.RemoveListener(OnObjectHoverExit);
        }

        private void OnObjectSelectEntered(SelectEnterEventArgs args)
        {
            XRBaseInteractable interactableObject = args.interactableObject as XRBaseInteractable;

            // Debug.Log($"the {interactableObject.gameObject.name} is entered");

            if (parentArea != null)
            {
                interactableObject.transform.SetParent(parentArea);
                var obj = interactableObject.GetComponent<XRGrabInteractableTwoAttach>();
                obj.retainTransformParent = false;
            }

            ToggleMesh(false);
            imageHover.enabled = false;
        }

        private void OnObjectSelectExit(SelectExitEventArgs args)
        {
            XRBaseInteractable interactableObject = args.interactableObject as XRBaseInteractable;

            // Debug.Log($"the {interactableObject.gameObject.name} is entered");

            ToggleMesh(true);

            if (parentArea != null)
            {
                var obj = interactableObject.GetComponent<XRGrabInteractableTwoAttach>();
                obj.retainTransformParent = true;
            }

            imageHover.enabled = true;
        }

        private void OnObjectHoverEnter(HoverEnterEventArgs args)
        {
            ToggleMesh(false);

            if (!hasSelection)
                ToggleImageHover(true);
        }

        private void OnObjectHoverExit(HoverExitEventArgs args)
        {
            if (!hasSelection)
            {
                ToggleMesh(true);
            }
            ToggleImageHover(false);
        }

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            return base.CanSelect(interactable) && MatchUsingTag(interactable as XRBaseInteractable);
        }

        public override bool CanHover(IXRHoverInteractable interactable)
        {
            return base.CanHover(interactable) && MatchUsingTag(interactable as XRBaseInteractable);
        }

        private bool MatchUsingTag(XRBaseInteractable interactable)
        {
            var obj = interactable.GetComponent<XRGrabInteractableTwoAttach>();

            return targetObjNames.Contains(obj.objName);
            // return interactable.CompareTag(targetTag);
        }

        private void ToggleMesh(bool _state)
        {
            if (!isUsingSocketModel) return;

            m_mesh.enabled = _state;
        }

        private void ToggleImageHover(bool _isActive)
        {
            if (imageHover.gameObject.activeSelf)
            {
                imageHover.sprite = _isActive ? activeHoverSprite : nonactiveHoverSprite;
            }
        }
    }
}
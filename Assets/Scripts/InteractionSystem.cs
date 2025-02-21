using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("References")]
    [SerializeField] private CrosshairController crosshair;

    private Camera _playerCamera;
    private IInteractable _currentInteractable;

    private void Awake() => _playerCamera = GetComponentInChildren<Camera>();

    private void Update()
    {
        CheckForInteractables();
        HandleInteractionInput();
    }

    private void CheckForInteractables()
    {
        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                _currentInteractable = interactable;
                crosshair.ShowInteractable(
                    interactable.GetInteractionType(),
                    interactable.GetObjectName()
                );
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }
    }

    private void HandleInteractionInput()
    {
        if (_currentInteractable != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            _currentInteractable.OnInteract();
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        _currentInteractable = null;
        crosshair.HideInteractable();
    }
}
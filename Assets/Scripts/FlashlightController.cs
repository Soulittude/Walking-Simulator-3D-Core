using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("Settings")]
    /*[SerializeField]*/ private Light flashlight;
    [SerializeField] private float toggleCooldown = 0.3f;

    private InputAction flashlightAction;
    private float lastToggleTime;

    private void Awake()
    {
        var playerInput = GetComponentInParent<PlayerInput>();
        flashlightAction = playerInput.actions["Flashlight"];

        flashlight = GetComponent<Light>();
    }

    private void Update()
    {
        if (flashlightAction.triggered && Time.time > lastToggleTime + toggleCooldown)
        {
            ToggleFlashlight();
            lastToggleTime = Time.time;
        }
    }

    private void ToggleFlashlight()
    {
        flashlight.enabled = !flashlight.enabled;
        // Add sound effect here later
    }
}
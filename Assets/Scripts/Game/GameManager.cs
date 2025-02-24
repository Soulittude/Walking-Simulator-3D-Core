using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool CanMove { get; private set; } = true;
    public bool CanLook { get; private set; } = true;
    public bool CanUseItems { get; private set; } = true;
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetControlState(bool canMove, bool canLook, bool canUseItems)
    {
        CanMove = canMove;
        CanLook = canLook;
        CanUseItems = canUseItems;
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0 : 1;
        Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = IsPaused;
    }
}
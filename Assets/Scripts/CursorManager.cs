using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [SerializeField] private Texture2D basicCursorTexture;
    [SerializeField] private Texture2D selectCursorTexture;
    [SerializeField] private Texture2D holdCursorTexture;


    private Vector2 cursorhotspot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
       cursorhotspot = new Vector2(basicCursorTexture.width / 2, basicCursorTexture.height / 2);
        SetBasicCursor();
    }

    public void SetBasicCursor() {
        Cursor.SetCursor(basicCursorTexture, cursorhotspot, CursorMode.Auto);
    }
    public void SetSelectCursor() {
        Cursor.SetCursor(selectCursorTexture, cursorhotspot, CursorMode.Auto);
    }
    public void SetHoldCursor() {
        Cursor.SetCursor(holdCursorTexture, cursorhotspot, CursorMode.Auto);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private texture2D cursorTexture;

    private vector2 cursorhotspot; 
    // Start is called before the first frame update
    void Start()
    {
       cursorhotspot = new vector2(cursorTexture.width / 2, cursorTexture.height / 2);
       Cursor.setCursor(cursorTexture, cursorhotspot)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

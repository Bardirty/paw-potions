using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    private void Start()
{
        camera1.gameObject.SetActive(true);
        camera1.gameObject.SetActive(false);
}
    private void Update()
    {  
        if (Input.GetKeyDown(KeyCode.S))
       { 
            camera1.gameObject.SetActive(!camera1.gameObject.activeSelf);
            camera2.gameObject.SetActive(!camera2.gameObject.activeSelf);
       }
    }
}
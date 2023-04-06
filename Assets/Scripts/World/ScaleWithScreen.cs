using UnityEngine;

// Scales a GameObject size with the screen size, used for the playable area, but can be used later on
public class ScaleWithScreen : MonoBehaviour
{
    [SerializeField] Camera cam;                                            // A reference to the main camera

    void Start()
    {
        Scale();
    }

    void Scale()
    {
        var height = cam.orthographicSize * 2.0;
        var width = height * Screen.width / Screen.height;
        transform.localScale = new Vector3((float)width, (float)height, 1);
    }
}

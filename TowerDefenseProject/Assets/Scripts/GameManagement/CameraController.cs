using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Control Variables")]
    [SerializeField]
    private float panSpeed;
    [SerializeField]
    private float panBorderThickness;
    [SerializeField]
    private float scrollSpeed;
    [SerializeField]
    private float minYZoom;
    [SerializeField]
    private float maxYZoom;


    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameIsOver)
        {
            enabled = false;
            return;
        }

        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        } 
        if(Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            transform.Translate(Vector3.back* panSpeed * Time.deltaTime, Space.World);
        } 
        if(Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            transform.Translate(Vector3.left* panSpeed * Time.deltaTime, Space.World);
        } 
        if(Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;
        pos.y -= scrollWheel * 1000 * scrollSpeed * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, minYZoom, maxYZoom);

        transform.position = pos;
    }
}

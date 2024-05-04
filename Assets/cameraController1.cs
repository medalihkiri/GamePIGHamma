using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform cameraTransform;

    public float normalSpeed;
    public float fastSpeedS;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public float zoomSpeed;

    public Vector3 zoomAmount;
    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;
    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPostion;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPostion;

    private bool isInteractingWithUI = false;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteractingWithUI)
        {
            HandelMovementInput();
            HandelMouseInput();
        }
    }

    //Mouse
    void HandelMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.down);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.down);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPostion = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPostion;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            rotateCurrentPostion = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPostion;

            rotateStartPosition = rotateCurrentPostion;
            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    //Keyboard
    void HandelMovementInput()
    {
        //movement 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeedS;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        //rotaion
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.LeftArrow))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.RightArrow))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        //zoom
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.UpArrow))
        {
            newZoom += zoomAmount * zoomSpeed;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.DownArrow))
        {
            newZoom -= zoomAmount * zoomSpeed;
        }

        //TimeDeltaTime
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    // Method to set UI interaction status
    public void SetInteractingWithUI(bool value)
    {
        isInteractingWithUI = value;
    }
}


using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform body;
    public Transform head;
    float rotationY = 0;
    float rotationX = -90;

    float angleYmin = -90f;
    float angleYmax = 90f;

    float smoothRotX = 0;
    float smoothRotY = 0;

    float smoothCoefX = 0.005f;
    float smoothCoefY = 0.005f;

    public bool TremorAtivo;

    public float TremoIntensity = 0.3f;
    Vector3 posOriginal;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        posOriginal = transform.localPosition;
    }
    private void LateUpdate()
    {
        Vector3 pos = head.position;
        if (TremorAtivo)
        {
            pos += UnityEngine.Random.insideUnitSphere * TremoIntensity * Time.deltaTime * 60f;
        }
        transform.position = pos;

    }
    // Update is called once per frame
    void Update()
    {
        float verticalDelta = Input.GetAxisRaw("Mouse Y");
        float HorizontalDelta = Input.GetAxisRaw("Mouse X");

        smoothRotX = Mathf.Lerp(smoothRotX, HorizontalDelta, smoothCoefX);
        smoothRotY = Mathf.Lerp(smoothRotY, HorizontalDelta, smoothCoefY);

        rotationX += HorizontalDelta;
        rotationY += verticalDelta;

        rotationY = Mathf.Clamp(rotationY, angleYmin, angleYmax);

        body.localEulerAngles = new Vector3(0, rotationX, 0);

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

    }

}


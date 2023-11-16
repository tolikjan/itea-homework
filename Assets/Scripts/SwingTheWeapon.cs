using System.Collections;
using UnityEngine;

public class SwingTheWeapon : MonoBehaviour
{
    // Rotation angles of weapon
    private static float _eulerAngleX;
    private static float _eulerAngleY;
    private static float _eulerAngleZ;

    public Vector3 defaultRotationAngle = new Vector3(_eulerAngleX, _eulerAngleY, _eulerAngleZ);

    // The angles to rotate by
    [SerializeField] public static float desiredRotationAngleX = -15.0f;
    [SerializeField] public static float desiredRotationAngleY = -80.0f;
    [SerializeField] public static float desiredRotationAngleZ = -40.0f;

    public Vector3 desiredRotationAngles =
        new Vector3(desiredRotationAngleX, desiredRotationAngleY, desiredRotationAngleZ);

    public float rotationSpeed = 0.1f; // Speed of the weapon rotation

    private bool isRotating;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(defaultRotationAngle);
    }

    void Start()
    {
        isRotating = false;
    }

    void Update()
    {
        _eulerAngleX = transform.localEulerAngles.x;
        _eulerAngleY = transform.localEulerAngles.y;
        _eulerAngleZ = transform.localEulerAngles.z;
        
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) == true && isRotating == false)
        {
            // Start the weapon rotation towards the desired angle
            StartCoroutine(RotateObject(desiredRotationAngles, rotationSpeed));
        }
        else if (transform.rotation != Quaternion.Euler(defaultRotationAngle) && isRotating == false)
        {
            // Gradually return to the default rotation
            StartCoroutine(RotateObject(defaultRotationAngle, rotationSpeed)); // Slower return to default
        }
    }

    IEnumerator RotateObject(Vector3 endAngle, float inTime)
    {
        isRotating = true;
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(endAngle);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }

        transform.rotation = toAngle;
        isRotating = false;
    }
}

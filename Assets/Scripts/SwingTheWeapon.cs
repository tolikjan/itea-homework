using System.Collections;
using UnityEngine;

public class SwingTheWeapon : MonoBehaviour
{
    public Vector3 defaultRotationAngle = new Vector3(12.45f, 229.74f, 0f);
    public Vector3 desiredRotationAngle = new Vector3(14.87f, 273.61f, -31.35f); // The angle to rotate by

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
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && !isRotating)
        {
            // Start the weapon rotation towards the desired angle
            StartCoroutine(RotateObject(desiredRotationAngle, rotationSpeed / 2));
        }
        else if (transform.rotation != Quaternion.Euler(defaultRotationAngle) && !isRotating)
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

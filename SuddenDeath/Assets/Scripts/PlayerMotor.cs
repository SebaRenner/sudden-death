using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private bool invertCamera = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;

    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void move(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    public void rotate(Vector3 rotation)
    {
        this.rotation = rotation;
    }

    public void rotateCamera(Vector3 cameraRotation)
    {
        this.cameraRotation = cameraRotation;
    }

    public void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (cam != null)
        {
            cam.transform.Rotate(!invertCamera ? -cameraRotation : cameraRotation);
        }
    }
}

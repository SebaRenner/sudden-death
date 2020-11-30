using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;

    public void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    public void Update()
    {
        float xMovement = Input.GetAxisRaw("Horizontal"); // -1 and 1
        float zMovement = Input.GetAxisRaw("Vertical"); // -1 and 1

        Vector3 moveHorizontal = transform.right * xMovement;
        Vector3 moveVertical = transform.forward * zMovement;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.move(velocity);

        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRotation, 0f) * lookSensitivity;

        motor.rotate(rotation);

        float xRotation = Input.GetAxisRaw("Mouse Y");
        Vector3 cameraRotation = new Vector3(xRotation, 0, 0) * lookSensitivity;

        motor.rotateCamera(cameraRotation);
    }
}

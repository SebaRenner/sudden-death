using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;
    private Animator animator;

    public void Start()
    {
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {

        if (PauseMenu.isOn)
        {
            if(Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            motor.move(Vector3.zero);
            motor.rotate(Vector3.zero);
            motor.rotateCamera(Vector3.zero);

            return;
        }
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        float xMovement = Input.GetAxis("Horizontal"); // -1 and 1
        float zMovement = Input.GetAxis("Vertical"); // -1 and 1

        Vector3 moveHorizontal = transform.right * xMovement;
        Vector3 moveVertical = transform.forward * zMovement;

        Vector3 velocity = (moveHorizontal + moveVertical) * speed;

        animator.SetFloat("ForwardVelocity", zMovement);
        animator.SetFloat("SidestepVelocity", xMovement);

        motor.move(velocity);

        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0f, yRotation, 0f) * lookSensitivity;

        motor.rotate(rotation);

        float xRotation = Input.GetAxisRaw("Mouse Y");
        Vector3 cameraRotation = new Vector3(xRotation, 0, 0) * lookSensitivity;

        motor.rotateCamera(cameraRotation);
    }
}

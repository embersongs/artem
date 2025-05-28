using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
   // public float crouchSpeed = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Camera")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    public float maxVerticalAngle = 80f;

    private CharacterController characterController;
    private Vector3 velocity;
    private float verticalRotation = 0f;
    private bool isGrounded;

    public float crouchHeight = 1f;
    private float originalHeight;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Блокировка курсора
        originalHeight = characterController.height;

   
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 1.2f);
    }

    void Update()
    {
        // Движение
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //Приседание
        if (Input.GetKeyDown(KeyCode.C))
        {
            characterController.height = crouchHeight;

        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            characterController.height = originalHeight;
        }

        // Проверка на земле ли игрок
        //isGrounded = characterController.isGrounded;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.5f, LayerMask.GetMask("Ground"));

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Прижимаем к земле
        }

        // Вращение камеры
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

       

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * speed * Time.deltaTime);

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Гравитация
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);




    }
}
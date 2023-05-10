using UnityEditor;
using UnityEngine;

//src: BloxyDev
public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera; // The camera attached to the player object
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f; // Time for mouse smoothing
    [SerializeField] bool cursorLock = true; // Whether to lock the cursor or not
    [SerializeField] float mouseSensitivity = 3.5f; // Mouse sensitivity
    [SerializeField] float speed = 6.0f; // Player movement speed
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f; // Time for movement smoothing
    [SerializeField] float gravity = -30f; // Gravity for player movement
    [SerializeField] Transform groundCheck; // A transform representing the position to check for the ground
    [SerializeField] LayerMask ground; // A layer mask representing the ground

    public float jumpHeight = 6f; // Jump height for the player

    float velocityY; // Vertical velocity of the player
    bool isGrounded; // Whether the player is on the ground or not

    float cameraCap; // The camera's vertical tilt
    Vector2 currentMouseDelta; // The current mouse delta
    Vector2 currentMouseDeltaVelocity; // The current mouse delta velocity
    public GameObject[] tools;
    CharacterController controller;
    Vector2 currentDir; // The current movement direction
    Vector2 currentDirVelocity; // The current movement direction velocity
    public Vector3 velocity; // The current movement velocity
    int currentToolIndex;

    // Access the ToolIndex variable of the ToolContainer component


    // Use the currentToolIndex variable in your Movement script as needed

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Get the ToolContainer component attached to the same game object
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the screen
            Cursor.visible = false; // Hide the cursor
        }
    }

    void Update()
    {
        UpdateMove();
        UpdateMouse();
    }


    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;

        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = new Vector3(cameraCap, 0, 0); // Rotate the camera vertically
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity); // Rotate the player horizontally


    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground); // Check if the player is on the ground

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime); // Smooth the movement direction

        velocityY -= gravity * -2.0f * Time.deltaTime; // Apply gravity

        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocityY; // Calculate movement velocity

        controller.Move(velocity * Time.deltaTime); // Move the player

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded! && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }
    }
}
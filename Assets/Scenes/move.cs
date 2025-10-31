using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6f;            // Speed of movement
    public float rotationSpeed = 3f;       // Speed of rotation
    public float gravity = -9.81f;          // Gravity force
    public float jumpHeight = 1.5f;         // Height of the jump
    public Transform cameraTransform;       // Reference to the camera's transform

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset the downward velocity when grounded
        }

        // Get input from WASD keys
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate movement direction relative to the camera
        Vector3 direction = cameraTransform.right * moveX + cameraTransform.forward * moveZ;
        direction.y = 0f; // Keep movement on the horizontal plane

        // Normalize the direction vector to prevent faster diagonal movement
        if (direction.magnitude >= 0.1f)
        {
            // Move the player
            controller.Move(direction.normalized * moveSpeed * Time.deltaTime);

            // Smoothly rotate the player towards the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Handle gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Jumping (optional)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Move2 : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float speedRun = 15f;

    [Header("Salto y gravedad")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = -15f;

    [Header("Instanciar objeto (opcional)")]
    //[SerializeField] private GameObject objectToSpawn;

    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference sprintAction;
    public InputActionReference jumpAction;

    private CharacterController controller;
    private float currentSpeed;
    private Vector2 moveInput;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        sprintAction.action.Enable();
        jumpAction.action.Enable();

        jumpAction.action.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJumpPerformed;

        moveAction.action.Disable();
        sprintAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Start()
    {
        currentSpeed = normalSpeed;
    }

    private void Update()
    {
        ReadInput();
        HandleMovement();
    }

    private void ReadInput()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
        currentSpeed = sprintAction.action.IsPressed() ? speedRun : normalSpeed;
    }

    private void HandleMovement()
    {
        // Dirección relativa a hacia donde mira el personaje
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        move = move.normalized * currentSpeed;

        // Si está tocando el piso y va cayendo, resetea la velocidad vertical
        // (evita que la gravedad se acumule infinitamente estando en el suelo)
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

    // Instancia un objeto delante del personaje. Llamala desde otro InputAction si la necesitás
    // (por ejemplo agregando una acción "Interact" y suscribiéndola igual que hicimos con Jump).
   /* public void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position + transform.forward * 2f, Quaternion.identity);
        }
    }*/
}
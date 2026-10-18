using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
[RequireComponent(typeof(Rigidbody))]
public class MovePlayer : MonoBehaviour
{
    [Header("Velocidad")]
        [SerializeField] private float normalSpeed = 5f;
        [SerializeField] private float speedRun = 15f;

        
        //[SerializeField] private float rotationSpeed;


    [Header("Salto y gravedad")]
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = -15f;


    [Header("Instanciar objeto (opcional)")]
        /*[SerializeField] GameObject GameObjectPrefab;*/

    [Header("Input Actions")]
            public InputActionReference moveAction;
            public InputActionReference sprintAction;
            public InputActionReference jumpAction;
            private CharacterController controller;
    [SerializeField] private float currentSpeed;
            private float speed;
    private Vector2 moveInput;
            //private float rotate;
            //private Vector2 look;

    private float verticalVelocity;
            //private float pitch;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
                //sprintAction.action.Enable();
                //jumpAction.action.Enable();

                //jumpAction.action.performed += OnJumpPerformed;
    }

    void Start()
    {
        currentSpeed = normalSpeed;
                //rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {

                //Vector2 movement = Movete.action.ReadValue<Vector2>();

        ReadInput();
        HandleMovement();
                //HandleRotation();
                //HandleLook();
                //HandleInstanceObject(GameObjectPrefab);


             
    }

    private void ReadInput()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
        //currentSpeed = sprintAction.action.IsPressed() ? speedRun : normalSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
       {
           currentSpeed = speedRun;
       }
       else
       {
           currentSpeed = speed;
       }
    }

    /*private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(_move.x * speed, _move.y * speed);
    }*/

    private void HandleMovement()
    {
        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        move = move.normalized * currentSpeed;

                if (controller.isGrounded && verticalVelocity < 0)
                {
                    verticalVelocity = -2f;
                }

        verticalVelocity += gravity * Time.deltaTime;

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

           


    }


           /* private void HandleRotation()
            {
                float rotation = rotate * rotationSpeed * Time.deltaTime;
                transform.Rotate(0, rotation, 0);
            }

            private void HandleLook() 
            {
                float mouseX = look.x * rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up * mouseX);
            }*/

    private void HandleInstanceObject(GameObject GameObjectPrefab) 
    {
        if (GameObjectPrefab != null && Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(GameObjectPrefab, transform.position + transform.forward * 2f, Quaternion.identity);
        }
    }


    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

                    /*public void OnLook(InputAction.CallbackContext context)
                    {
                        look = context.ReadValue<Vector2>();
                    }*/

                    /*public void OnRotate(InputAction.CallbackContext context)
                    {
                        rotate = context.ReadValue<float>();
                    }*/

    /*public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
             speed = speedRun;

        if (context.canceled)
            speed = normalSpeed;
    }*/


    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
            verticalVelocity = jumpForce;
        }
    }

   /* public void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Instantiate(objectToSpawn, transform.position + transform.forward * 2f, Quaternion.identity);
        }
    }*/
}
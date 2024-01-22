using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movingSpeed, gravityModifier, jumpPower;
    public CharacterController characterController;
    public Transform cameraTransform, bulletPoint;
    public float mouseSensitivity;
    public FixedJoystick joystick;
    private Vector3 movingAxis;
    public FixedTouchField fixedTouchField;
    private float verticalRotation = 0.0f;
    public Canvas mobileControls;
    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    private Animator animator;
    public GameObject bullet;
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        if (IsOnMobile())
        {
            mobileControls.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOnDesktop())
        {
            HandleDesktopInput();
            //ApplyGravity();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ApplyJumping();
            }
            MovePlayer();
            if (Input.GetMouseButtonDown(0))
            {
                fireBullet();
            }
            RotatePlayerAndCamera(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
        else if (IsOnMobile())
        {
            HandleMobileInput();
            MovePlayer();
            RotatePlayerAndCamera(fixedTouchField.TouchDist.x, fixedTouchField.TouchDist.y);
        }
    }

    bool IsOnDesktop()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor;
    }

    bool IsOnMobile()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    void HandleDesktopInput()
    {
        float yStore = movingAxis.y;
        Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");
        Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
        movingAxis = horizontalMove + verticalMove;
        movingAxis.Normalize();
        movingAxis *= movingSpeed;
        movingAxis.y = yStore;
    }

    void HandleMobileInput()
    {
        float yStore = movingAxis.y;
        Vector3 joyStickMove = transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
        movingAxis = joyStickMove;
        movingAxis.Normalize();
        movingAxis *= movingSpeed;
        movingAxis.y = yStore;
    }

    void ApplyGravity()
    {

        if (characterController.isGrounded)
        {
            movingAxis.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }
        else
        {
            movingAxis.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        }
    }
    public void ApplyJumping()
    {
        canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;
        if (canJump)
        {
            Debug.Log("called");
            movingAxis.y = jumpPower;
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            Debug.Log("double jump called");
            movingAxis.y = jumpPower;
            canDoubleJump = false;
        }
    }

    void MovePlayer()
    {
        characterController.Move(movingAxis * Time.deltaTime);
        animator.SetFloat("moveSpeed", movingAxis.magnitude);
        animator.SetBool("onGround", canJump);
    }

    void RotatePlayerAndCamera(float mouseX, float mouseY)
    {
        // Horizontal rotation of player and camera
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + mouseX, transform.eulerAngles.z);
        // Vertical rotation of camera
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -45f, 45f);
        cameraTransform.rotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    public void fireBullet()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 50f))
        {
            if (Vector3.Distance(cameraTransform.position, hit.point) > 2f)
            {
                bulletPoint.LookAt(hit.point);
            }
        }
        else
        {
            bulletPoint.LookAt(cameraTransform.position + (cameraTransform.forward * 30f));
        }
        Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
    }
    private void FixedUpdate()
    {
        ApplyGravity();
    }
}

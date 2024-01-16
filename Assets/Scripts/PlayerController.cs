using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movingSpeed;
    public CharacterController characterController;
    public Transform cameraTransform;
    public float mouseSensitivity;
    public FixedJoystick joystick;
    Vector3 movingAxis;
    public FixedTouchField fixedTouchField;
    private float verticalRotation = 0.0f;
    public Canvas mobileControls;
    public float gravityModifier;
    // Start is called before the first frame update
    void Start()
    {
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    Debug.Log("mobile");
        //    mobileControls.gameObject.SetActive(true);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            //movingAxis.x = Input.GetAxis("Horizontal") * movingSpeed * Time.deltaTime;
            //movingAxis.z = Input.GetAxis("Vertical") * movingSpeed * Time.deltaTime;
            //Moving according to the input axis in the directin of camera
            Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");
            Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
            movingAxis = horizontalMove + verticalMove;
            movingAxis.Normalize();
            ApplyGravity();
            characterController.Move(movingAxis * movingSpeed * Time.deltaTime);
            //Controlling Camera Rotation
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
            ////horizontal rotation of player and camera
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + mouseInput.x, transform.eulerAngles.z);
            ////vertical rotation of camera
            verticalRotation -= mouseInput.y;
            verticalRotation = Mathf.Clamp(verticalRotation, -45f, 45f);
            cameraTransform.rotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, transform.eulerAngles.z);

        }
        else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //Moving according to the input axis in the directin of camera
            Vector3 joyStickMove = transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
            movingAxis = joyStickMove;
            movingAxis.Normalize();
            ApplyGravity();
            characterController.Move(movingAxis * movingSpeed * Time.deltaTime);
            //Controlling Camera Rotation
            Vector2 touchInput = fixedTouchField.TouchDist * mouseSensitivity * Time.deltaTime;
            //horizontal rotation of player and camera
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + touchInput.x, transform.eulerAngles.z);
            //vertical rotation of camera
            verticalRotation -= touchInput.y;
            verticalRotation = Mathf.Clamp(verticalRotation, -45f, 45f);
            cameraTransform.rotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            movingAxis.y = 0f;
        }
        else
        {
            movingAxis.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }
    }
}

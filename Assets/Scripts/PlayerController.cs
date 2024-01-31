using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float movingSpeed, gravityModifier, jumpPower, adsSpeed;
    public CharacterController characterController;
    public Transform cameraTransform, bulletPoint, adsPoint, gunHolder;
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
    public static PlayerController instance;
    public Gun gun;
    public List<Gun> gunList = new List<Gun>();
    public List<Gun> lockedGuns = new List<Gun>();
    public int currentGun;
    private Vector3 gunStartPosition;
    bool isZoomed;
    public bool isFireBtnClicked;
    public GameObject switchWeaponIcon;
    public GameObject muzzleFlash;
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
        StartCoroutine(enableCharacterController());
        gun = gunList[currentGun];
        gun.gameObject.SetActive(true);
        gunStartPosition = gunHolder.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //if (IsOnDesktop())
        //{
        //    HandleDesktopInput();
        //    //ApplyGravity();
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        ApplyJumping();
        //    }
        //    MovePlayer();
        //    if (Input.GetMouseButtonDown(0) && gun.fireCounter <= 0)
        //    {
        //        fireBullet();
        //    }
        //    if (Input.GetMouseButton(0) && gun.canAutoFire && gun.fireCounter <= 0)
        //    {
        //        fireBullet();
        //    }
        //    if (Input.GetKeyDown(KeyCode.Tab))
        //    {
        //        switchGun();
        //    }
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        CameraController.instance.ZoomIn(gun.zoomAmount);

        //    }
        //    if (Input.GetMouseButton(1))
        //    {
        //        gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
        //    }
        //    else
        //    {
        //        gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPosition, adsSpeed * Time.deltaTime);
        //    }
        //    if (Input.GetMouseButtonUp(1))
        //    {
        //        CameraController.instance.ZoomOut();
        //    }
        //    RotatePlayerAndCamera(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        //}
        //else if (IsOnMobile())
        //{
        HandleMobileInput();
        MovePlayer();
        RotatePlayerAndCamera(fixedTouchField.TouchDist.x, fixedTouchField.TouchDist.y);
        if (isFireBtnClicked && gun.canAutoFire)
        {
            fireBullet();
        }
        if (isZoomed == true)
        {
            Debug.Log(isZoomed);
            gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
        }
        else
        {
            gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPosition, adsSpeed * Time.deltaTime);
        }
        //}
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

    //void RotatePlayerAndCamera(float mouseX, float mouseY)
    //{
    //    // Horizontal rotation of player and camera
    //    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + mouseX * mouseSensitivity, transform.eulerAngles.z);
    //    // Vertical rotation of camera
    //    verticalRotation -= mouseY * mouseSensitivity;
    //    verticalRotation = Mathf.Clamp(verticalRotation, -45f, 45f);
    //    cameraTransform.rotation = Quaternion.Euler(verticalRotation, transform.eulerAngles.y, transform.eulerAngles.z);
    //}
    void RotatePlayerAndCamera(float mouseX, float mouseY)
    {
        // Horizontal rotation of player and camera
        float mouseXSmooth = mouseX * mouseSensitivity;
        float mouseYSmooth = mouseY * mouseSensitivity;

        transform.rotation *= Quaternion.Euler(0, mouseXSmooth, 0);

        // Vertical rotation of camera
        verticalRotation -= mouseYSmooth;
        verticalRotation = Mathf.Clamp(verticalRotation, -45f, 45f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
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

        //FireShot();
        if (gun.ammoAmount > 0 && gun.fireCounter <= 0)
        {
            gun.ammoAmount--;
            UiController.instance.ammoTxt.text = "Ammo: " + gun.ammoAmount;
            bulletPoint = gun.firePoint;
            Instantiate(gun.bullet, bulletPoint.position, bulletPoint.rotation);
            gun.fireCounter = gun.fireRate;
        }
        muzzleFlash.SetActive(true);
        muzzleFlash.transform.position = gun.transform.GetChild(1).transform.position;
        StartCoroutine(MuzzleFlashOff());
    }
    private void FixedUpdate()
    {
        ApplyGravity();
    }
    IEnumerator enableCharacterController()
    {
        yield return new WaitForFixedUpdate();
        gameObject.GetComponent<CharacterController>().enabled = true;

    }
    //public void FireShot()
    //{

    //}
    public void getAmmo(int amount)
    {
        gun.ammoAmount += amount;
        UiController.instance.ammoTxt.text = "Ammo: " + gun.ammoAmount;
    }
    public void switchGun()
    {
        gun.gameObject.SetActive(false);
        if (currentGun >= gunList.Count - 1)
        {
            currentGun = 0;
        }
        else
        {
            currentGun++;
        }
        if (isZoomed)
        {
            CameraController.instance.ZoomOut();
            isZoomed = false;
        }
        gun = gunList[currentGun];
        gun.gameObject.SetActive(true);
        UiController.instance.ammoTxt.text = "Ammo: " + gun.ammoAmount;

    }
    public void getWeapon(string name)
    {
        for (int i = 0; i < lockedGuns.Count; i++)
        {
            if (lockedGuns[i].gameObject.name == name)
            {
                switchWeaponIcon.gameObject.SetActive(true);
                gunList.Add(lockedGuns[i]);
                Debug.Log(gunList.Count);
                lockedGuns.RemoveAt(i);
                return;
            }
        }
    }
    public void zoom()
    {
        if (isZoomed)
        {
            CameraController.instance.ZoomOut();
            isZoomed = false;
        }
        else
        {
            CameraController.instance.ZoomIn(gun.zoomAmount);

            isZoomed = true;
        }
    }
    public void makeFireBtnTrue()
    {
        isFireBtnClicked = true;
    }
    public void makeFireBtnFalse()
    {
        isFireBtnClicked = false;
    }
    IEnumerator MuzzleFlashOff()
    {
        yield return new WaitForEndOfFrame();
        muzzleFlash.SetActive(false);
    }
}

using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public float movementSpeed = 5000f; // �������� �������� ����
    public float liftForce = 12000f; // ���� ������� ����
    public float rotationSpeed = 2f; // �������� �������� ����
    public AudioSource audioSource; // ������ �� ���� ������ ����
    public float verticalTilt;
    public float forceUpWhenFall = 0.4f;
    public float groundRaycastDistance = 0.2f;
    public bool isGrounded = false;
    public Joystick joystick;
    public float maxSpeed = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        LimitSpeed();

        // ��������������� �����, ���� ������ �� �������� �����
        if (!isGrounded && !audioSource.isPlaying)
            audioSource.Play();
        if (isGrounded && audioSource.isPlaying)
            audioSource.Stop();

        // ������������� ���� ������� ������������ ������������ ��� (��� X)
        verticalTilt = Mathf.Abs(transform.rotation.eulerAngles.x);

        // ��������� �������� ������ � ����� (W � S)
        float verticalInputKey = Input.GetAxis("Vertical");
        float verticalInputJoy = joystick.Vertical;
        float verticalInput = verticalInputKey + verticalInputJoy;
        Vector3 forwardMovement = transform.forward * verticalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(forwardMovement);

        // ��������� �������� ����� � ������ (A � D)
        float horizontalInputKey = Input.GetAxis("Horizontal");
        float horizontalInputJoy = joystick.Horizontal;
        float horizontalInput = horizontalInputKey + horizontalInputJoy;
        Vector3 sideMovement = transform.right * horizontalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(sideMovement);

        // ��������� ������� � ���� ��� ������ �������� � �����
        if (Input.anyKey && isGrounded)
        {
            Vector3 lift = Vector3.up * liftForce * Time.deltaTime;
            rb.AddForce(lift);
        }


        // ���� ���� ������� ������ 45 ��������, �������� ��� ����������� �����
        if (verticalTilt < 350f && verticalTilt > 10f)
        {
            Vector3 pushUp = Vector3.up * liftForce * Time.deltaTime * forceUpWhenFall;
            rb.AddForce(pushUp);
        }



        // ������������ ���� �� ���������
        Quaternion targetRotationY = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * rotationSpeed);

        // ������������ �������� ���� ����� ������������ ���
        Quaternion targetRotationZ = Quaternion.Euler(0f, transform.rotation.eulerAngles.z, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationZ, Time.deltaTime * rotationSpeed);


    }

    // ���������� ������������ � ������� ������������
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Button"))
            isGrounded = true;
    }

    // ���������� ��������� ������������ � ������� ������������
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Button"))
            isGrounded = false;
    }

    // ����������� �������� �������
    private void LimitSpeed()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f; // ���������� ������������ ���������� ��������
        float currentSpeed = horizontalVelocity.magnitude;

        if (currentSpeed > maxSpeed)
        {
            float clampedSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
            rb.velocity = horizontalVelocity.normalized * clampedSpeed;
        }
    }
}

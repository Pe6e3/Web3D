using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public float movementSpeed = 5000f; // �������� �������� ����
    public float liftForce = 3000f; // ���� ������� ����
    public float rotationSpeed = 2f; // �������� �������� ����
    public float verticalTilt;
    public float forceUpWhenFall = 0.2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ������������� ���� ������� ������������ ������������ ��� (��� X)
        verticalTilt = Mathf.Abs(transform.rotation.eulerAngles.x);

        // ��������� �������� ������ � ����� (W � S)
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 forwardMovement = transform.forward * verticalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(forwardMovement);

        // ��������� �������� ����� � ������ (A � D)
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 sideMovement = transform.right * horizontalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(sideMovement);

        // ��������� ������� � ��������� ���� (Q � E)
        if (Input.GetKey(KeyCode.E))
        {
            Vector3 lift = Vector3.up * liftForce * Time.deltaTime;
            rb.AddForce(lift);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Vector3 descend = Vector3.down * liftForce * Time.deltaTime;
            rb.AddForce(descend);
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

        // ������������ ���� �� ���������
        Quaternion targetRotationZ = Quaternion.Euler(0f, transform.rotation.eulerAngles.z, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationZ, Time.deltaTime * rotationSpeed);
    }
}

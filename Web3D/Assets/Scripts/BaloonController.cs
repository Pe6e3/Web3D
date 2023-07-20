using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public float movementSpeed = 5000f; // Скорость движения шара
    public float liftForce = 12000f; // Сила подъема шара
    public float rotationSpeed = 2f; // Скорость поворота шара
    public float verticalTilt;
    public float forceUpWhenFall = 0.4f;
    public float groundRaycastDistance = 0.2f;
    public bool isGrounded = false;
    public Joystick joystick;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Относительный угол наклона относительно вертикальной оси (ось X)
        verticalTilt = Mathf.Abs(transform.rotation.eulerAngles.x);

        // Обработка движения вперед и назад (W и S)
        //float verticalInput = Input.GetAxis("Vertical");
        float verticalInput = joystick.Vertical;
        Vector3 forwardMovement = transform.forward * verticalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(forwardMovement);

        // Обработка движения влево и вправо (A и D)
        //float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalInput = joystick.Horizontal;
        Vector3 sideMovement = transform.right * horizontalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(sideMovement);

        // Обработка подъема и шара При начале движения с земли
        if (Input.anyKey && isGrounded)
        {
            Vector3 lift = Vector3.up * liftForce * Time.deltaTime;
            rb.AddForce(lift);
        }


        // Если угол наклона больше 45 градусов, толкнуть шар вертикально вверх
        if (verticalTilt < 350f && verticalTilt > 10f)
        {
            Vector3 pushUp = Vector3.up * liftForce * Time.deltaTime * forceUpWhenFall;
            rb.AddForce(pushUp);
        }



        // Выравнивание шара по вертикали
        Quaternion targetRotationY = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * rotationSpeed);

        // Выравнивание вращение шара вдоль вертикальной оси
        Quaternion targetRotationZ = Quaternion.Euler(0f, transform.rotation.eulerAngles.z, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationZ, Time.deltaTime * rotationSpeed);


    }

    // Обработчик столкновения с другими коллайдерами
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Button"))
            isGrounded = true;
    }

    // Обработчик окончания столкновения с другими коллайдерами
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Button"))
            isGrounded = false;
    }
}

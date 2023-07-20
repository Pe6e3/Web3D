using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public float movementSpeed = 5000f; // Скорость движения шара
    public float liftForce = 12000f; // Сила подъема шара
    public float rotationSpeed = 2f; // Скорость поворота шара
    public AudioSource audioSource; // Ссылка на звук полета шара
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

        // Воспроизведение звука, если объект не касается земли
        if (!isGrounded && !audioSource.isPlaying)
            audioSource.Play();
        if (isGrounded && audioSource.isPlaying)
            audioSource.Stop();

        // Относительный угол наклона относительно вертикальной оси (ось X)
        verticalTilt = Mathf.Abs(transform.rotation.eulerAngles.x);

        // Обработка движения вперед и назад (W и S)
        float verticalInputKey = Input.GetAxis("Vertical");
        float verticalInputJoy = joystick.Vertical;
        float verticalInput = verticalInputKey + verticalInputJoy;
        Vector3 forwardMovement = transform.forward * verticalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(forwardMovement);

        // Обработка движения влево и вправо (A и D)
        float horizontalInputKey = Input.GetAxis("Horizontal");
        float horizontalInputJoy = joystick.Horizontal;
        float horizontalInput = horizontalInputKey + horizontalInputJoy;
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

    // Ограничение скорости объекта
    private void LimitSpeed()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f; // Игнорируем вертикальную компоненту скорости
        float currentSpeed = horizontalVelocity.magnitude;

        if (currentSpeed > maxSpeed)
        {
            float clampedSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
            rb.velocity = horizontalVelocity.normalized * clampedSpeed;
        }
    }
}

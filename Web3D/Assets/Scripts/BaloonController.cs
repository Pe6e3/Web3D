using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public float movementSpeed = 5000f; // Скорость движения шара
    public float liftForce = 3000f; // Сила подъема шара
    public float rotationSpeed = 2f; // Скорость поворота шара
    public float verticalTilt;
    public float forceUpWhenFall = 0.2f;

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
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 forwardMovement = transform.forward * verticalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(forwardMovement);

        // Обработка движения влево и вправо (A и D)
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 sideMovement = transform.right * horizontalInput * movementSpeed * Time.deltaTime;
        rb.AddForce(sideMovement);

        // Обработка подъема и опускания шара (Q и E)
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

        // Если угол наклона больше 45 градусов, толкнуть шар вертикально вверх
        if (verticalTilt < 350f && verticalTilt > 10f)
        {
            Vector3 pushUp = Vector3.up * liftForce * Time.deltaTime * forceUpWhenFall;
            rb.AddForce(pushUp);
        }

       

        // Выравнивание шара по вертикали
        Quaternion targetRotationY = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationY, Time.deltaTime * rotationSpeed);

        // Выравнивание шара по вертикали
        Quaternion targetRotationZ = Quaternion.Euler(0f, transform.rotation.eulerAngles.z, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationZ, Time.deltaTime * rotationSpeed);
    }
}

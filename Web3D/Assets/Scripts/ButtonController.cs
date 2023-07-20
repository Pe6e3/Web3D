using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public float buttonPressDistance = 0.3f; // Расстояние, на которое кнопка опускается при нажатии
    public float returnSpeed = 5f; // Скорость возвращения кнопки

    public bool isPressed = false;
    public bool isCollided = false;
    public Vector3 originalPosition;

    public string url = "http://185.246.67.169:36013/category/economics";


    public AudioSource audioSource; // Компонент для воспроизведения звука

    void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPressed && !isCollided) ButtonBack();
    }

    void OnCollisionEnter(Collision collision)
    {
        isCollided = true;

        // Проверяем столкновение с объектом, у которого есть тэг "Player" (шар)
        if (collision.gameObject.CompareTag("Player") && !isPressed)
        {
            // Уменьшаем позицию кнопки по оси Y на заданное расстояние
            Vector3 newPosition = originalPosition - new Vector3(0f, buttonPressDistance, 0f);
            transform.position = newPosition;

            isPressed = true;

            // Воспроизводим звук при нажатии на кнопку
            if (audioSource != null && audioSource.clip != null)
                audioSource.Play();

            Application.OpenURL(url);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isCollided = false;
    }

    void ButtonBack()
    {
        // Плавно возвращаем кнопку на исходное место
        if (Vector3.Distance(transform.position, originalPosition) > 0.01f)
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * returnSpeed);
        else
        {
            transform.position = originalPosition;
            isPressed = false;
        }
    }
}

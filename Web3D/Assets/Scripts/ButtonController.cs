using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public float buttonPressDistance = 0.3f; // ����������, �� ������� ������ ���������� ��� �������
    public float returnSpeed = 5f; // �������� ����������� ������

    public bool isPressed = false;
    public bool isCollided = false;
    public Vector3 originalPosition;

    public string url = "http://185.246.67.169:36013/category/economics";


    public AudioSource audioSource; // ��������� ��� ��������������� �����

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

        // ��������� ������������ � ��������, � �������� ���� ��� "Player" (���)
        if (collision.gameObject.CompareTag("Player") && !isPressed)
        {
            // ��������� ������� ������ �� ��� Y �� �������� ����������
            Vector3 newPosition = originalPosition - new Vector3(0f, buttonPressDistance, 0f);
            transform.position = newPosition;

            isPressed = true;

            // ������������� ���� ��� ������� �� ������
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
        // ������ ���������� ������ �� �������� �����
        if (Vector3.Distance(transform.position, originalPosition) > 0.01f)
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * returnSpeed);
        else
        {
            transform.position = originalPosition;
            isPressed = false;
        }
    }
}

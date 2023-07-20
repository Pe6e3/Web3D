using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSoundController : MonoBehaviour
{
    public float heightFromGround; // Высота шара от земли
    public AudioSource audioSource; // Ссылка на звук полета шара
    public float minHeigh = 0.5f;
    public float maxHeigh = 5f;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {    // Воспроизведение звука, если объект не касается земли
        if (!BalloonController.isGrounded && !audioSource.isPlaying)
            audioSource.Play();
        if (BalloonController.isGrounded && audioSource.isPlaying)
            audioSource.Stop();

        // Вычисление текущей высоты шара от земли
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f))
            heightFromGround = hit.distance;


        // Установка громкости аудио исходя из высоты шара
        float normalizedHeight = Mathf.InverseLerp(minHeigh, maxHeigh, heightFromGround);
        audioSource.volume = normalizedHeight;
    }
}

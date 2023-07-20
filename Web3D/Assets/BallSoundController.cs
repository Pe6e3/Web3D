using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSoundController : MonoBehaviour
{
    public float heightFromGround; // ������ ���� �� �����
    public AudioSource audioSource; // ������ �� ���� ������ ����
    public float minHeigh = 0.5f;
    public float maxHeigh = 5f;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {    // ��������������� �����, ���� ������ �� �������� �����
        if (!BalloonController.isGrounded && !audioSource.isPlaying)
            audioSource.Play();
        if (BalloonController.isGrounded && audioSource.isPlaying)
            audioSource.Stop();

        // ���������� ������� ������ ���� �� �����
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f))
            heightFromGround = hit.distance;


        // ��������� ��������� ����� ������ �� ������ ����
        float normalizedHeight = Mathf.InverseLerp(minHeigh, maxHeigh, heightFromGround);
        audioSource.volume = normalizedHeight;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHitSound : MonoBehaviour
{
    public AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }
}

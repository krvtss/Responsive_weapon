using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Presets;
using UnityEditor.Rendering;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb => GetComponent<Rigidbody>();

    [SerializeField] float speed;
    [SerializeField] GameObject[] hitEffect;
    [SerializeField] int damage;
    [SerializeField] float extra_Kickback;

    [SerializeField] bool kill_absolutely;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip metalHitClip;
    [SerializeField] AudioClip bodyHitClip;

    [SerializeField] AudioClip ricoClip;

    [SerializeField] bool Dont_Destroy_On_Collision;

    private void Start()
    {
        rb.AddForce(transform.forward * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<Bullet>()) { return; }

        //audioSource.pitch = Random.Range(0.8f, 1.2f);
        //audioSource.clip = metalHitClip;
        //Instantiate(audioSource.gameObject, transform.position, Quaternion.identity);

        //if (Random.Range(0, 10) < 2)
        //{
        //    audioSource.pitch = Random.Range(0.4f, 1.6f);
        //    audioSource.clip = ricoClip;
        //    Instantiate(audioSource.gameObject, transform.position, Quaternion.identity);
        //}

        if (collision.gameObject.GetComponent<Health>())
        {
            if (kill_absolutely) {

            }
            collision.gameObject.GetComponent<Health>().Damage(damage, rb.linearVelocity);
        }
        if (extra_Kickback != 0 && collision.gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.Normalize(collision.transform.position - transform.position) * extra_Kickback);
        }

        foreach (var item in hitEffect)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
        if (!Dont_Destroy_On_Collision)
        {
            Destroy(gameObject);
        }
    }
}

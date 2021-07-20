using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMove : MonoBehaviour
{
    [SerializeField] ParticleSystem mainEngineEffect;
    [SerializeField] ParticleSystem leftBoostEffect;
    [SerializeField] ParticleSystem rightBoostEffect;
    [SerializeField] Vector3 force;

    private AudioSource audioSource;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody>();
        mainEngineEffect.Play();
        leftBoostEffect.Play();
        rightBoostEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddRelativeForce(force*Time.deltaTime);
    }
}

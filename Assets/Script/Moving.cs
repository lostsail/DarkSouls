using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] float pushForce;
    [SerializeField] float rotateSpeed;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainEngineEffect;
    [SerializeField] ParticleSystem leftBoostEffect;
    [SerializeField] ParticleSystem rightBoostEffect;

    private Rigidbody rb;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }

    private void FixedUpdate()
    {
        ProcessBoost();
        ProcessRotate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ProcessBoost()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrust();
        }
        else
        {
            StopThrust();
        }
    }

    private void StartThrust()
    {
        if (!mainEngineEffect.isPlaying)
            mainEngineEffect.Play();

        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);

        rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * pushForce);
    }

    private void StopThrust()
    {
        mainEngineEffect.Stop();
        audioSource.Stop();
    }

    private void ProcessRotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.angularVelocity = Vector3.zero;
            StartRotateRight();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.angularVelocity = Vector3.zero;
            StartRotateLeft();
        }
        else
        {
            StopRotate();
        }
    }

    private void StartRotateRight()
    {
        leftBoostEffect.Stop();
        if (!rightBoostEffect.isPlaying)
            rightBoostEffect.Play();

        RotateForward(rotateSpeed);
    }

    private void StartRotateLeft()
    {
        rightBoostEffect.Stop();
        if (!leftBoostEffect.isPlaying)
            leftBoostEffect.Play();

        RotateForward(-rotateSpeed);
    }

    private void StopRotate()
    {
        leftBoostEffect.Stop();
        rightBoostEffect.Stop();
    }

    private void RotateForward(float rotateSpeed)
    {
        transform.Rotate(Vector3.forward * Time.fixedDeltaTime * rotateSpeed);
    }

    public void Stop()
    {
        StopThrust();
        StopRotate();
    }
}

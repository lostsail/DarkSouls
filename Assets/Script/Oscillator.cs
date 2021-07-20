using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startPos;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovementProcess();
    }

    private void MovementProcess()
    {
        float circle = (Time.time % period) / period;
        const float tau = 2 * Mathf.PI;
        float movementFactor = (Mathf.Sin(tau * circle) + 1f) / 2f;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startPos + offset;
    }
}

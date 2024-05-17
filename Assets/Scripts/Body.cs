using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Vector3 velocity;
    public float a;
    public Transform target;

    void Update()
    {
        var force = target.position - transform.position;

        velocity += force.normalized * 1f / force.magnitude * Time.deltaTime * a;

        var delta = velocity * Time.deltaTime;
        transform.position += delta;
    }
}

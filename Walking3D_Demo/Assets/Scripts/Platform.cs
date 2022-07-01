using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float _speed;
    void Update()
    {
        transform.Translate(Vector3.back * Time.deltaTime * _speed);
    }
}

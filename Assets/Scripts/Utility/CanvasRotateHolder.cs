using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotateHolder : MonoBehaviour
{
    private Quaternion rotation;

    private void Start()
    {
        rotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = rotation;
    }

}

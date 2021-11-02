using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}

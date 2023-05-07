using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    
    private void Update() {
        transform.Rotate(Vector3.forward, 0.5f);
    }
}

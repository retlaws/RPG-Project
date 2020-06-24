using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] GameObject target;
    public void Destroy()
    {
        Destroy(target);
    }
}

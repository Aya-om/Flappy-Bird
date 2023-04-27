using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFlowers : MonoBehaviour
{
    private float speed=20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position+=new Vector3(-1,0,0) * speed * Time.deltaTime;
    }
}

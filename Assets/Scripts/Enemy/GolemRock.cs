using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemRock : MonoBehaviour
{
    public float force;
    public GameObject target;

    public Rigidbody rb;
    private Vector3 direction;
    void Awake() {
        
    }
    void Start()
    {
        // rb = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void FlyToTarget(){
        if(target == null){
            Debug.Log("FlyToTarget--->target == null");
            target = FindObjectOfType<Player>().gameObject;
        }
        Vector3 up = new Vector3(0, 2,0);
        direction = target.transform.position - transform.position + up;
        Debug.Log("direction--->0:" + direction);
        direction .Normalize();
        Debug.Log("direction--->1:" + direction);
        // rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(direction * force, ForceMode.Impulse);

    }
}

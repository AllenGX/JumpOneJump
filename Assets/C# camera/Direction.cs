using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour {
    
    public Transform cube;
    // Use this for initialization
    float direction_X;
    float direction_Y;
    private void Awake()
    {
        direction_X = cube.position.x-this.transform.position.x;
        direction_Y = cube.position.x - this.transform.position.y;
    
    }
    void Start () {
        // Debug.Log(direction_X);
        

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<Rigidbody>().AddForce(-direction_X, -direction_Y, 100f);
        }
    }
}

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public Vector3 targetPosition;
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Mouse0))
		   {
			targetPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
		}
        
        if (transform.position != targetPosition)
		    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
	}
}



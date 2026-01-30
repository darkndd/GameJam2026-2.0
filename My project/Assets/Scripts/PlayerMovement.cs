using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public Vector2 targetPosition;
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Mouse0))
		   {
			targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
        
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
	}
}



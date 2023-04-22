using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePayload : MonoBehaviour
{
    public int speed = 3;
  bool move = false;
    void Update()
    {
        if(move){
            Debug.Log("yes");
            speed = 3;
         transform.Translate(Vector3.left * speed * Time.deltaTime);
         //move = false;
        }
    }


    private void OnTriggerEnter(Collider collider){
        
        if(collider.gameObject.tag == "Red"){

            move = true;
            //transform.position = new Vector3(0, 1, -5);
        }
    }

    private void OnTriggerExit(Collider collider){
            

         if(collider.gameObject.tag == "Red"){    
            Debug.Log("no");

            move = false;
            speed = 0;
        }
            
    }

}

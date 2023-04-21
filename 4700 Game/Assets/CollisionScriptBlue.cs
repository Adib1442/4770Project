using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScriptBlue : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision){
        
        if(collision.gameObject.tag == "BulletBlue"){
        Destroy(this.gameObject);
        }
    }
}

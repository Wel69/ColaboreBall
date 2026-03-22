using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Unity.VisualScripting;
//using System;

public class NotColliderDetection2 : MonoBehaviour
{
    public Main main;

    private void OnCollisionExit2D(Collision2D other)
    {
        /*PhysicsMaterial2D bouncyMaterial4 = new PhysicsMaterial2D();
        bouncyMaterial4.bounciness = 0.0f;
        bouncyMaterial4.friction = 0.0f;
        Collider2D collider4 = other.gameObject.GetComponentInChildren<Collider2D>();
        collider4.sharedMaterial = bouncyMaterial4;*/
        
        main.NotCollisionDetected2();
    }
}

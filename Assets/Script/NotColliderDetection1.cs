using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Unity.VisualScripting;
//using System;

public class NotColliderDetection1 : MonoBehaviour
{
    public Main main;

    private void OnCollisionExit2D(Collision2D other)
    {
        /*PhysicsMaterial2D bouncyMaterial3 = new PhysicsMaterial2D();
        bouncyMaterial3.bounciness = 0.0f;
        bouncyMaterial3.friction = 0.0f;
        Collider2D collider3 = other.gameObject.GetComponentInChildren<Collider2D>();
        collider3.sharedMaterial = bouncyMaterial3;*/

        main.NotCollisionDetected1();
    }
}


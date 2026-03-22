using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetection3 : MonoBehaviour
{
    public Main main;

    private void OnCollisionEnter2D(Collision2D other)
    {
        /*PhysicsMaterial2D bouncyMaterial = new PhysicsMaterial2D();
        bouncyMaterial.bounciness = 0.0f;
        bouncyMaterial.friction = 0.0f;
        Collider2D collider = other.gameObject.GetComponentInChildren<Collider2D>();
        collider.sharedMaterial = bouncyMaterial;*/

        if (other.gameObject.GetComponentInChildren<Renderer>().material.color == Color.red)
        {
            if (other.gameObject.tag == "Droite")
            {
                main.CollisionDetectedRedDroite();
            }
            else
            {
                main.CollisionDetectedRedGauche();
            }
        }
        else
        {
            if (other.gameObject.tag == "Droite")
            {
                main.hasBallFallen2 = true;
                main.WrongPlace2();
            }
            else
            {
                main.hasBallFallen1 = true;
                main.WrongPlace1();
            }

        }
    }
}

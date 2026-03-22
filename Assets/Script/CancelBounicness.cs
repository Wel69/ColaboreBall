using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelBounicness : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {

        Debug.Log("collision detectee ");
        PhysicsMaterial2D bouncyMaterial = new PhysicsMaterial2D();
        bouncyMaterial.bounciness = 0.0f;
        bouncyMaterial.friction = 0.0f;
        Collider2D collider = other.gameObject.GetComponentInChildren<Collider2D>();
        collider.sharedMaterial = bouncyMaterial;

    }
}

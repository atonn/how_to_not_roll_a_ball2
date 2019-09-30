using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnStroopWall : MonoBehaviour
{

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("obstacle")) {
            Debug.Log("StroopWall was destroyed/despawned.");
            Destroy(other.collider.gameObject);
        }
    }
}

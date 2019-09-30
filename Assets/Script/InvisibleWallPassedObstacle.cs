using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallPassedObstacle : MonoBehaviour
{

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("obstacle")) {
            Debug.Log("You passed a wall!");
            DataCollector.DC.requestDatapointLogging("wallPass", other.gameObject);
        }
    }
}

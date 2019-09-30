using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StroopWallGenerator : MonoBehaviour
{
    public GameObject stroopWallPrefab;

    public float spawnRateInSeconds; //spawn rate in seconds
    private float instantiationTimer;
    public float chanceOfCompatibility;
 
    private List<string> viableOrientationOptions = new List<string>(new string[] { "up", "left", "right" });

    private void Start() {
        //Random.seed = 42;
        instantiationTimer = spawnRateInSeconds;
    }

    private void Update() {
        instantiationTimer -= Time.deltaTime;
        if (instantiationTimer <= 0) {
            spawnWall();
            instantiationTimer = spawnRateInSeconds; //XXX refactor
        }
    }

    void spawnWall() {
        //roll a wall orientation first:
        string _chosenWallOrientation = viableOrientationOptions[ Mathf.FloorToInt(Random.Range(0, viableOrientationOptions.Count)) ];


        //create a (matching? depending on chanceOfCompatibility roll) arrow orientation:
        string _chosenArrowOrientation = "";
        if (Random.Range(0, 100) < chanceOfCompatibility) {
            _chosenArrowOrientation = _chosenWallOrientation;
        }
            else
            {
                do
                {
                    _chosenArrowOrientation = viableOrientationOptions[ Mathf.FloorToInt(Random.Range(0, viableOrientationOptions.Count)) ];
                } while (_chosenWallOrientation == _chosenArrowOrientation); //XXX optimize in the future (while loop inefficient)
            }
    
        
        GameObject instantiated = Instantiate(stroopWallPrefab, transform.position, Quaternion.identity);
        instantiated.GetComponent<StroopWall>().arrowOrientation = _chosenArrowOrientation;
        instantiated.GetComponent<StroopWall>().requiredDirectionToPass = _chosenWallOrientation;
    }

    private void FixedUpdate() {
        
    }
}

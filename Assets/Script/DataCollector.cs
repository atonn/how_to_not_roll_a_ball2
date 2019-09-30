using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataCollector : MonoBehaviour
{
    public static DataCollector DC; 
    private List<List<string>> _data;
    private List<string> _csvHeaders;
    // Start is called before the first frame update

    //from: https://answers.unity.com/questions/323195/how-can-i-have-a-static-class-i-can-access-from-an.html
    void Awake()
     {
         if(DC != null)
             GameObject.Destroy(DC);
         else
             DC = this;
         
         DontDestroyOnLoad(this);
     }

    void Start()
    {
        _data = new List<List<string>>();
        _csvHeaders = new List<string>();

        _csvHeaders.Add("eventType");
        _csvHeaders.Add("timestamp");

        _csvHeaders.Add("ballXPosition");
        _csvHeaders.Add("ballYPosition");
        _csvHeaders.Add("ballXVelocity");
        _csvHeaders.Add("ballYVelocity");

        _csvHeaders.Add("wallRequiredDirectionToPass");
        _csvHeaders.Add("wallArrowOrientation");
        _csvHeaders.Add("isCompatible");
        //if Event Type == WallCollision... get additional data for collision...
        _csvHeaders.Add("wallCollisionX");
        _csvHeaders.Add("wallCollisionY");
    }

    //allows any object like
    //the instantiated walls on spawning,
    //the ball (or wall) when both collide,
    //or the invisible wall that collides with walls that the ball "passed" and did not collide with
    //to request to log everything to a csv row, which is handled here centrally
    public void requestDatapointLogging(string eventType, GameObject theWallInQuestion, Collision collision = null) {
        Debug.Log("Requested Logging!");

        List<string> eventData = new List<string>();
        eventData.Add(eventType);
        eventData.Add(Time.realtimeSinceStartup.ToString().Replace( ",", "." ));

        GameObject ball = GameObject.Find("Ball");
        // .Replace( ",", "." ) - because float and csv comma separation dislike each other (depending on the OS)
        eventData.Add(ball.GetComponent<MoveBall>().GetXPosition().ToString().Replace( ",", "." ));
        eventData.Add(ball.GetComponent<MoveBall>().GetYPosition().ToString().Replace( ",", "." ));
        eventData.Add(ball.GetComponent<MoveBall>().GetXVelocity().ToString().Replace( ",", "." ));
        eventData.Add(ball.GetComponent<MoveBall>().GetYVelocity().ToString().Replace( ",", "." ));

        eventData.Add(theWallInQuestion.GetComponent<StroopWall>().requiredDirectionToPass);
        eventData.Add(theWallInQuestion.GetComponent<StroopWall>().arrowOrientation);
        eventData.Add(theWallInQuestion.GetComponent<StroopWall>().IsComplatible().ToString());

        if (collision != null) {
            //First contact points
            eventData.Add(collision.contacts[0].point[0].ToString().Replace( ",", "." )); //x
            eventData.Add(collision.contacts[0].point[1].ToString().Replace( ",", "." )); //y
        }
        
        _data.Add(eventData);
    }

    // Update is called once per frame
    // WRITING LOG TO CSV FILE AND QUITTING THE GAME VIA ESCAPE BUTTON
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("End of experiment, you are a russian spy!");

            List<string> csvLines = CSVTools.GenerateCSV(_data, _csvHeaders);
            foreach (string line in csvLines)
            {
                Debug.Log(line);
            }

            CSVTools.SaveCSV(csvLines, Application.dataPath + "/Data/" + GUID.Generate());
            Application.Quit();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBall : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float forceMultiplier;
    public float jumpStrength;

    public AudioClip explosionSound;
    AudioSource audioSource;

    private GameObject _sanikBall;


    //exploding walls on collision
    public GameObject explosionOne;
    public GameObject explosionTwo;
    public GameObject explosionThree;

    private Vector3 fingerStart;
    private Vector3 fingerEnd;
    private float swipeUpTolerance = 160; //swipe Threshold (android)

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _sanikBall = GameObject.Find("Ball");
        audioSource = GetComponent<AudioSource>();

        GameObject slonky = GameObject.Find("DebugText");
        Text txt = slonky.GetComponent<Text>();

        if (_rigidbody == null)
        {
            Debug.LogError("No rigidbody found");
        }
    }

    private void FixedUpdate()
    {

        /*if (Input.GetKey(KeyCode.W))
        {
            _rigidbody.AddForce(new Vector3(0,0,1) * forceMultiplier,ForceMode.Acceleration);        
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.AddForce(new Vector3(0,0,-1) * forceMultiplier,ForceMode.Acceleration);        
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody.AddForce(new Vector3(1,0,0) * forceMultiplier,ForceMode.Acceleration);        
        }
        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody.AddForce(new Vector3(-1,0,0) * forceMultiplier,ForceMode.Acceleration);        
        }*/

        float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");
        Debug.Log(x);
        _rigidbody.AddForce(new Vector3(x * forceMultiplier, 0, 0), ForceMode.Acceleration);

        //     //android support swipe-like (was wonky)
        //     if (Input.touchCount > 0 && 
        //    Input.GetTouch(0).phase == TouchPhase.Moved) {
        //      // Get movement of the finger since last frame
        //     Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //      // Move object across XY plane
        //      _rigidbody.AddForce(new Vector3(Mathf.Sign(touchDeltaPosition.x) * forceMultiplier , 0 , 0),ForceMode.Acceleration);
        //  }  

        //ANDROID SUPPORT
        //GetMouseButtonDown(0) instead of TouchPhase.Began
        if (Input.GetMouseButtonDown(0))
        {
            fingerStart = Input.mousePosition;
            fingerEnd = Input.mousePosition;
        }

        //GetMouseButton instead of TouchPhase.Moved
        //This returns true if the LMB is held down in standalone OR
        //there is a single finger touch on a mobile device
        if (Input.GetMouseButton(0))
        {
            fingerEnd = Input.mousePosition;

            //left right movement depending on whether the finger is to the left or right of the ball
            if (fingerEnd.x < Camera.main.WorldToScreenPoint(transform.position).x)
            {
                _rigidbody.AddForce(new Vector3(-forceMultiplier, 0, 0), ForceMode.Acceleration);

            }
            else
            {
                _rigidbody.AddForce(new Vector3(+forceMultiplier, 0, 0), ForceMode.Acceleration);
            }

            //There was some movement! The tolerance variable is to detect some useful movement
            //i.e. an actual swipe rather than some jitter. This is the same as the value of 80
            //you used in your original code.
            if (Mathf.Abs(fingerEnd.y - fingerStart.y) > swipeUpTolerance)
            {
                //Upward Swipe: Jump
                if ((fingerEnd.y - fingerStart.y) > 0)
                    Jump();

                //After the checks are performed, set the fingerStart & fingerEnd to be the same
                fingerStart = fingerEnd;
            }
        }

        //GetMouseButtonUp(0) instead of TouchPhase.Ended
        if (Input.GetMouseButtonUp(0))
        {
            fingerStart = Vector2.zero;
            fingerEnd = Vector2.zero;
        }

    }


    private void Update()
    {
        _sanikBall.transform.Rotate(eulers: new Vector3(1000, y: 0, z: 0) * Time.deltaTime); //illusion of rolling forward

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Jump();
        }
    }

    private void Jump()
    {
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.01)
        { //can not jump if not on floor
            _rigidbody.AddForce(new Vector3(0, 1, 0) * jumpStrength, ForceMode.Impulse);
        }
        else
        {
            Debug.Log(_rigidbody.velocity.y);
        }
    }

    //public getter functions for the DataCollector object (data logging)
    public float GetXPosition()
    {
        return transform.position.x;
    }
    public float GetYPosition()
    {
        return transform.position.y;
    }
    public float GetXVelocity()
    {
        return _rigidbody.velocity.x;
    }
    public float GetYVelocity()
    {
        return _rigidbody.velocity.y;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("obstacle"))
        {
            Debug.Log("You hit a wall!");
            DataCollector.DC.requestDatapointLogging("wallCollide", other.gameObject, other);

            Instantiate(explosionOne, transform.position, Quaternion.identity);
            Instantiate(explosionTwo, transform.position, Quaternion.identity);
            Instantiate(explosionThree, transform.position, Quaternion.identity);
            Destroy(other.collider.gameObject);
            audioSource.PlayOneShot(explosionSound, 0.7F);
        }
    }
}

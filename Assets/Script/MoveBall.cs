using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _sanikBall = GameObject.Find("Ball");
        audioSource = GetComponent<AudioSource>();

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
        _rigidbody.AddForce(new Vector3(x * forceMultiplier , 0 , 0),ForceMode.Acceleration);
    }

    private void Update()
    {
        _sanikBall.transform.Rotate( eulers: new Vector3(1000, y: 0, z:0 ) * Time.deltaTime); //illusion of rolling forward

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (Mathf.Abs(_rigidbody.velocity.y) < 0.01) { //can not jump if not on floor
                _rigidbody.AddForce(new Vector3(0,1,0) * jumpStrength ,ForceMode.Impulse);
            } else {
                Debug.Log(_rigidbody.velocity.y);
            }       
        }
    }

    //public getter functions for the DataCollector object (data logging)
    public float GetXPosition() {
        return transform.position.x;
    }
    public float GetYPosition() {
        return transform.position.y;
    }
    public float GetXVelocity() {
        return _rigidbody.velocity.x;
    }
    public float GetYVelocity() {
        return _rigidbody.velocity.y;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.collider.CompareTag("obstacle")) {
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

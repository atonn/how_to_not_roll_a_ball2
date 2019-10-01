using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScrolling : MonoBehaviour
{
    public float scrollSpeed = .5f;
    public Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float vOffset = Time.time * scrollSpeed;
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(0, -vOffset));
    }
}

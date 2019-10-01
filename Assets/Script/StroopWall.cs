using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StroopWall : MonoBehaviour
{
    public string arrowOrientation; //up, left, right
    public string requiredDirectionToPass; //up, left, right
    //Position
    public float xOffset = 0;

    //Scrolling
    public float movementTime;
    float lerpValue = 0;
    public GameObject start;
    public GameObject end;


    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    //
    // Start is called before the first frame update
    void Start()
    {
        //assert if not up left right
        Debug.Assert(arrowOrientation == "up" || arrowOrientation == "left" || arrowOrientation == "right");
        Debug.Assert(requiredDirectionToPass == "up" || requiredDirectionToPass == "left" || requiredDirectionToPass == "right");

        Debug.Log(string.Format("Wall spawned with correct answer: {0} and arrow direction: {1}", requiredDirectionToPass, arrowOrientation));
        DataCollector.DC.requestDatapointLogging("wallSpawn", this.gameObject);

        //get texture
        Renderer m_Renderer = GetComponent<Renderer>();
        Texture2D currentTexture = m_Renderer.material.GetTexture("_MainTex") as Texture2D;

        if (arrowOrientation == "up")
        {
            //no rotation of texture is necessary, but due to the bug above XXX we need to rotate twice (for 180°)
            Texture2D rotatedTexture = rotateTexture(currentTexture, true);
            rotatedTexture = rotateTexture(rotatedTexture, true);
            m_Renderer.material.SetTexture("_MainTex", rotatedTexture);

        }
        else if (arrowOrientation == "right")
        {
            Texture2D rotatedTexture = rotateTexture(currentTexture, false);
            m_Renderer.material.SetTexture("_MainTex", rotatedTexture);

        }
        else if (arrowOrientation == "left")
        {
            Texture2D rotatedTexture = rotateTexture(currentTexture, true);
            m_Renderer.material.SetTexture("_MainTex", rotatedTexture);
        }

        //bool randomBoolean  = (Random.value > 0.5f); //old coin throw code

        //adjust size and texture tiling according to wall orientation (up=wide; rest=not so wide)
        if (requiredDirectionToPass == "up")
        {
            transform.localScale = new Vector3(x: 21, y: 7, z: 2); //wide wall (needs to be jumped over)
            xOffset = 0;
            m_Renderer.material.mainTextureScale = new Vector2(x: 3, y: 1); //change tiling so that arrow shape stays uniform

        }
        else if (requiredDirectionToPass == "right")
        {
            transform.localScale = new Vector3(x: 14, y: 21, z: 2); //not so wide wall (can be walked around)
            xOffset = -3;
            m_Renderer.material.mainTextureScale = new Vector2(x: 2, y: 3);

        }
        else if (requiredDirectionToPass == "left")
        {
            transform.localScale = new Vector3(x: 14, y: 21, z: 2); //not so wide wall (can be walked around)
            xOffset = 5;
            m_Renderer.material.mainTextureScale = new Vector2(x: 2, y: 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //lerps from spawn point to finish line
        lerpValue += Time.deltaTime / movementTime;
        transform.position = Vector3.Lerp(start.transform.position, end.transform.position, lerpValue) + new Vector3(x: xOffset, y: 0, z: 0); ;
    }

    public bool IsComplatible()
    {
        return (requiredDirectionToPass == arrowOrientation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    [SerializeField]
    private Renderer renderer;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float scale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
        renderer.material.mainTextureScale = new Vector2(scale, scale);
    }
}

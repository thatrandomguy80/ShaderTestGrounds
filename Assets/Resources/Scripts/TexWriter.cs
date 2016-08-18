using UnityEngine;
using System.Collections;
using System.IO;

public class TexWriter : MonoBehaviour {
    public Color col;
    private float dist;
    private WWW w;
    private Texture2D tex;

    public float distin;

    void Awake() {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 45;
    }
    // Use this for initialization
    //void Start() {
    //    Application.targetFrameRate = 60;
    //}

    // Update is called once per frame
    void Update() {
        if (Input.GetKey("up")) {
            write(true);
            StartCoroutine("writeTex");
        }
        else if (Input.GetKey("down")){
            write(false);
            StartCoroutine("writeTex");
        }
        //if (distin - dist > 1f || distin - dist < 1f)
        //   {
        //rewrite
        //      writeTex();
        // }
    }

    void write(bool a) {
        if (a) {
            // Create a texture
            int width = 800;
            int height = 600;
            tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            //set colors
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (x > 400)
                        tex.SetPixel(x, y, Color.green);
                    else
                        tex.SetPixel(x, y, col);
                }
            }
            tex.Apply();
        } else {
            // Create a texture
            int width = 800;
            int height = 600;
            tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            //set colors
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (y > 300) 
                        tex.SetPixel(x, y, Color.white);
                    else
                        tex.SetPixel(x, y, col);
                }
            }
            tex.Apply();
        }
    }

    IEnumerator writeTex() {
      

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        //gen filename
        string fileName = Application.dataPath + "/Resources/dot.png";

        // Write to file
        File.WriteAllBytes(fileName, bytes);
        //www file import for runtime
        w = new WWW ("file://" + fileName);
        yield return w;//wait for DL to finish
        //change texture of shader
        if (w.isDone) {
            this.gameObject.GetComponent<Renderer>().material.SetTexture("_posTex", w.texture);
        }
        //assuming writing is done call import
        Object.Destroy(tex);
    }

    public void updateMat() {
        //this.gameObject.GetComponent<Renderer>().material.
    }
}

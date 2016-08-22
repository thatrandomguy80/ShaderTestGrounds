using UnityEngine;
using System.Collections;

public class Warptar : MonoBehaviour {

    public Transform target;
    public float mul = 0.1f;
    public float bounds = 3f;
    public float offset = 0.5f;

    private float off = 0f;
    private Renderer r;
    private Shader s;
    private Material m;

    // Use this for initialization
    void Start() {
        r = this.GetComponent<Renderer>();
        m = r.material;
        s = m.shader;
    }

    // Update is called once per frame
    void Update() {
        Vector3 dist = this.gameObject.transform.position - target.position;
        if (dist.y < bounds && dist.y > -bounds) {
            if (dist.y < bounds / 2 && dist.y > -bounds / 2) {//if closer allow portal to reach object
                Vector3 temp = new Vector3(Mathf.Lerp(off, dist.y * -mul, Time.deltaTime), 0, 0);
                off = temp.x;
            } else {
                Vector3 temp = new Vector3(Mathf.Lerp(off, (dist.y - offset) * -mul, Time.deltaTime), 0, 0);//only allow portal to get -offset close to object
                off = temp.x;
            }
        } else {
            //reset pos
            Vector3 temp = new Vector3(Mathf.Lerp(off, 0f, Time.deltaTime), 0, 0);
            off = temp.x;
        }
        m.SetFloat("_Poffx", off);
        float x = target.localPosition.x, y = target.localPosition.z;
        //t = (y - range)/ range;
        //-1,1
        x += 1;
        y += 1;
        x *= 2;
        y *= 2;
        //0-2
        Debug.Log("X " + x + " Y " + y);
        m.SetFloat("_sampleoffx", x);
        m.SetFloat("_sampleoffy", -y);
    }
}

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
        if (dist.x < bounds && dist.x > -bounds) {
            if (dist.x < bounds / 2 && dist.x > -bounds / 2) {//if closer allow portal to reach object
                Vector3 temp = new Vector3(Mathf.Lerp(off, dist.x * -mul, Time.deltaTime), 0, 0);
                off = temp.x;
            } else {
                Vector3 temp = new Vector3(Mathf.Lerp(off, (dist.x - offset) * -mul, Time.deltaTime), 0, 0);//only allow portal to get -offset close to object
                off = temp.x;
            }
        } else {
            //reset pos
            Vector3 temp = new Vector3(Mathf.Lerp(off, 0f, Time.deltaTime), 0, 0);
            off = temp.x;
        }
        m.SetFloat("_Poffx", off);

        float range = 10f;
        float t = 0.1f, b = 0.1f; ;
        float x = target.position.z, y = target.position.y;
        if (y > range) {
            y += (range - y) * 2;
        } else {
            y += (range - y) * 2;
        }
        t = (y - range)/ range;
        m.SetFloat("_sampleoffx", (y / range) - t);//world coord differ from tex map
        m.SetFloat("_sampleoffy", (x / range) - b);
    }
}

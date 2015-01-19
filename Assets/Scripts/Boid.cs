using UnityEngine;
using System.Collections;

public class Boid {

    public GameObject go;
    public Vector3 velocity;

    public Boid(GameObject go) {
        this.go = go;
        velocity = Vector3.zero;
    }
}

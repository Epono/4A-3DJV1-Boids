using UnityEngine;
using System.Collections;

public class BoidsScriptV2 : MonoBehaviour {

    [SerializeField]
    int _boidsCount;

    [SerializeField]
    GameObject _boidPrefab;

    [SerializeField]
    GameObject _newBoidPrefab;

    [SerializeField]
    GameObject[] _boids;

    [SerializeField]
    GameObject[] _newBoids;

    private Boid[] boids;

    private Boid[] newBoids;

    void Start() {
        InitializePositions();
    }

    // Update is called once per frame
    void Update() {
        MoveAllBoidsToNewPositions();
    }

    void InitializePositions() {
        /*
        boids = new Boid[_boids.Length];
        newBoids = new Boid[_newBoids.Length];

        for(var i = 0; i < _boids.Length; ++i) {
            _boids[i].transform.position = new Vector3((Random.value - 0.5f) * 10, Random.value * 5, 5 + ((Random.value - 0.5f) * 10));

            Boid b = new Boid(_boids[i]);
            boids[i] = b;
        }

        for(var i = 0; i < _newBoids.Length; ++i) {
            _newBoids[i].transform.position = new Vector3((Random.value - 0.5f) * 10, Random.value * 5, 5 + ((Random.value - 0.5f) * 10));

            Boid b = new Boid(_newBoids[i]);
            newBoids[i] = b;
        }*/

        boids = new Boid[_boidsCount];
        newBoids = new Boid[_boidsCount];

        for(var i = 0; i < _boidsCount; ++i) {
            GameObject go = (GameObject)GameObject.Instantiate(_boidPrefab);
            go.transform.position = new Vector3((Random.value - 0.5f) * 10, Random.value * 5, 5 + ((Random.value - 0.5f) * 10));

            Boid b = new Boid(go);
            boids[i] = b;
        }

        for(var i = 0; i < _boidsCount; ++i) {
            GameObject go = (GameObject)GameObject.Instantiate(_newBoidPrefab);
            go.transform.position = new Vector3((Random.value - 0.5f) * 10, Random.value * 5, 5 + ((Random.value - 0.5f) * 10));

            Boid b = new Boid(go);
            newBoids[i] = b;
        }
    }

    void MoveAllBoidsToNewPositions() {
        Vector3 v1 = Vector3.zero;
        Vector3 v2 = Vector3.zero;
        Vector3 v3 = Vector3.zero;
        Vector3 v4 = Vector3.zero;
        Vector3 v5 = Vector3.zero;

        foreach(Boid b in boids) {
            Vector3 initPos = b.go.transform.position;

            v1 = GatheringRule(b);
            v2 = MinimumVitalSpaceRule(b);
            v3 = BoidsTryToKeepUpRule(b);
            //v4 = GoThereRule(b);
            v5 = StayHereRule(b);

            LimitVelocity(b);

            b.velocity = b.velocity + v1 + v2 + v3 + v4 + v5;

            Vector3 lol = b.go.transform.position + b.velocity;
            b.go.transform.LookAt(lol);

            b.go.transform.position = b.go.transform.position + b.velocity;

            Vector3 endPos = b.go.transform.position;
            Vector3 temp = endPos - initPos;
            Vector3 direction = endPos.normalized;
            //Debug.Log(direction);

            // b.go.rigidbody.MoveRotation(Quaternion.LookRotation(direction));
            // b.go.transform.LookAt(endPos);
        }

        foreach(Boid b in newBoids) {
            Vector3 initPos = b.go.transform.position;

            v1 = GatheringRule(b);
            v2 = MinimumVitalSpaceRule(b);
            v3 = BoidsTryToKeepUpRule(b);
            //v4 = GoThereRule(b);
            v5 = StayHereRule(b);

            LimitVelocity(b);

            b.velocity = b.velocity + v1 + v2 + v3 + v4 + v5;

            Vector3 lol = b.go.transform.position + b.velocity;
            b.go.transform.LookAt(lol);

            b.go.transform.position = b.go.transform.position + b.velocity;

            Vector3 endPos = b.go.transform.position;
            Vector3 temp = endPos - initPos;
            Vector3 direction = endPos.normalized;
            //Debug.Log(direction);

            // b.go.rigidbody.MoveRotation(Quaternion.LookRotation(direction));
            // b.go.transform.LookAt(endPos);
        }
    }

    Vector3 GatheringRule(Boid bj) {
        Vector3 pcj = new Vector3(0, 0, 0);

        foreach(Boid b in boids) {
            if(bj != b) {
                pcj += b.go.transform.position;
            }
        }

        pcj /= (_boids.Length - 1);

        return (pcj - bj.go.transform.position) / 400;
    }

    Vector3 MinimumVitalSpaceRule(Boid bj) {
        Vector3 c = new Vector3(0, 0, 0);

        foreach(Boid b in boids) {
            if(bj != b) {
                if(Vector3.Distance(b.go.transform.position, bj.go.transform.position) < 3) {
                    c = c - (b.go.transform.position - bj.go.transform.position);
                }
            }
        }

        return c;
    }

    Vector3 BoidsTryToKeepUpRule(Boid bj) {
        Vector3 pvj = new Vector3(0, 0, 0);

        foreach(Boid b in boids) {
            if(bj != b) {
                pvj += b.velocity;
            }
        }

        pvj /= (_boids.Length - 1);

        return (pvj - bj.velocity) / 100;
    }

    Vector3 GoThereRule(Boid bj) {
        Vector3 place = new Vector3(0, 0, 0);
        return (place - bj.go.transform.position) / 100;
    }

    void LimitVelocity(Boid b) {
        float vlim = 1f;

        if(Mathf.Abs(b.velocity.magnitude) > vlim) {
            b.velocity = (b.velocity / Mathf.Abs(b.velocity.magnitude)) * vlim;
        }
    }

    Vector3 StayHereRule(Boid b) {
        var xMin = -50;
        var xMax = 50;
        var yMin = 0;
        var yMax = 50;
        var zMin = -50;
        var zMax = 50;

        Vector3 v = Vector3.zero;

        if(b.go.transform.position.x < xMin)
            v.x = 10;
        else if(b.go.transform.position.x > xMax)
            v.x = -10;

        if(b.go.transform.position.y < yMin)
            v.y = 10;
        else if(b.go.transform.position.y > yMax)
            v.y = -10;

        if(b.go.transform.position.z < zMin)
            v.z = 10;
        else if(b.go.transform.position.z > zMax)
            v.z = -10;

        return v;
    }
}

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

    //[SerializeField]
    //GameObject[] _newBoids;

    private Boid[] boids;

    //private Boid[] newBoids;

    void Start() {
        InitializePositions();
    }

    // Update is called once per frame
    void Update() {
        MoveAllBoidsToNewPositions();
    }

    void InitializePositions() {
        boids = new Boid[_boidsCount];
        //newBoids = new Boid[_boidsCount];

        for(var i = 0; i < _boidsCount; ++i) {
            GameObject go = (GameObject)GameObject.Instantiate(_boidPrefab);
            go.transform.position = new Vector3((Random.value - 0.5f) * 10, Random.value * 5, 5 + ((Random.value - 0.5f) * 10));

            Boid b = new Boid(go);
            boids[i] = b;
        }
        /*
                for(var i = 0; i < _boidsCount; ++i) {
                    GameObject go = (GameObject)GameObject.Instantiate(_newBoidPrefab);
                    go.transform.position = new Vector3((Random.value - 0.5f) * 10, Random.value * 5, 5 + ((Random.value - 0.5f) * 10));

                    Boid b = new Boid(go);
                    newBoids[i] = b;
                }
         */
    }

    void MoveAllBoidsToNewPositions() {
        Vector3 v1 = Vector3.zero;
        Vector3 v2 = Vector3.zero;
        Vector3 v3 = Vector3.zero;
        Vector3 v4 = Vector3.zero;
        Vector3 v5 = Vector3.zero;

        foreach(Boid b in boids) {
            v1 = GatheringRule(b);
            v2 = MinimumVitalSpaceRule(b);
            v3 = BoidsTryToKeepUpRule(b);
            //v4 = GoThereRule(b);
            v5 = StayHereRule(b);

            LimitVelocity(b);

            b.velocity = b.velocity + v1 + v2 + v3 + v4 + v5;

            Vector3 endPos = b.go.transform.position + b.velocity;
            b.go.transform.LookAt(endPos);
            b.go.transform.position = endPos;
        }

        /*
        foreach(Boid b in newBoids) {
            v1 = GatheringRule(b);
            v2 = MinimumVitalSpaceRule(b);
            v3 = BoidsTryToKeepUpRule(b);
            //v4 = GoThereRule(b);
            v5 = StayHereRule(b);

            LimitVelocity(b);

            b.velocity = b.velocity + v1 + v2 + v3 + v4 + v5;

            Vector3 endPos = b.go.transform.position + b.velocity;
            b.go.transform.LookAt(endPos);
            b.go.transform.position = endPos;
        }*/
    }

    Vector3 GatheringRule(Boid bj) {
        Vector3 pcj = new Vector3(0, 0, 0);

        foreach(Boid b in boids) {
            if(bj != b) {
                pcj += b.go.transform.position;
            }
        }
        pcj /= (_boidsCount - 1);
        return (pcj - bj.go.transform.position) / 2000;
    }

    Vector3 MinimumVitalSpaceRule(Boid bj) {
        Vector3 c = Vector3.zero;
        float distance = 3;

        foreach(Boid b in boids) {
            if(bj != b) {
                if(Vector3.Distance(b.go.transform.position, bj.go.transform.position) < distance) {
                    c = c - ((b.go.transform.position - bj.go.transform.position) / distance);
                }
            }
        }
        return c;
    }

    Vector3 BoidsTryToKeepUpRule(Boid bj) {
        Vector3 pvj = Vector3.zero;

        foreach(Boid b in boids) {
            if(bj != b) {
                pvj += b.velocity;
            }
        }
        pvj = pvj / (_boidsCount - 1);
        return (pvj - bj.velocity) / 1000;
    }

    Vector3 GoThereRule(Boid bj) {
        Vector3 place = Vector3.zero;
        return (place - bj.go.transform.position) / 100;
    }

    void LimitVelocity(Boid b) {
        float vlim = 0.5f;

        if(Mathf.Abs(b.velocity.magnitude) > vlim) {
            b.velocity = (b.velocity / Mathf.Abs(b.velocity.magnitude)) * vlim;
        }
    }

    Vector3 StayHereRule(Boid b) {
        int xMin = -50;
        int xMax = 50;
        int yMin = 0;
        int yMax = 50;
        int zMin = -50;
        int zMax = 50;
        int getOutOrHere  = 10;

        Vector3 v = Vector3.zero;

        if(b.go.transform.position.x < xMin)
            v.x = getOutOrHere;
        else if(b.go.transform.position.x > xMax)
            v.x = -getOutOrHere;

        if(b.go.transform.position.y < yMin)
            v.y = getOutOrHere;
        else if(b.go.transform.position.y > yMax)
            v.y = -getOutOrHere;

        if(b.go.transform.position.z < zMin)
            v.z = getOutOrHere;
        else if(b.go.transform.position.z > zMax)
            v.z = -getOutOrHere;

        return v;
    }
}
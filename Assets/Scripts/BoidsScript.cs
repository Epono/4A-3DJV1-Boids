using UnityEngine;
using System.Collections;

public class BoidsScript : MonoBehaviour {

    [SerializeField]
    GameObject[] _boids;

    void Start() {
        InitializePositions();
    }

    // Update is called once per frame
    void Update() {
        MoveAllBoidsToNewPositions();
    }

    void InitializePositions() {
        for(var i = 0; i < _boids.Length; ++i) {
            _boids[i].transform.position = new Vector3((Random.value - 0.5f) * 10, Random.value * 5, 5 + ((Random.value - 0.5f) * 10));
        }
    }

    void MoveAllBoidsToNewPositions() {
        Vector3 v1 = Vector3.zero;
        Vector3 v2 = Vector3.zero;
        Vector3 v3 = Vector3.zero;
        Vector3 v4 = Vector3.zero;
        Vector3 v5 = Vector3.zero;

        foreach(GameObject b in _boids) {
            v1 = GatheringRule(b);
            v2 = MinimumVitalSpaceRule(b);
            v3 = BoidsTryToKeepUpRule(b);
            //v4 = GoThereRule(b);
            v5 = StayHereRule(b);

            LimitVelocity(b);

            b.rigidbody.velocity = b.rigidbody.velocity + v1 + v2 + v3 + v4 + v5;
            b.transform.position = b.transform.position + b.rigidbody.velocity;
            b.rigidbody.MoveRotation(Quaternion.Euler(b.rigidbody.velocity));
        }
    }

    Vector3 GatheringRule(GameObject bj) {
        Vector3 pcj = new Vector3(0,0,0);

        foreach(GameObject b in _boids) {
            if(bj != b) {
                pcj += b.transform.position;
            }
        }

        pcj /= (_boids.Length -1);

        return (pcj - bj.transform.position) / 1;
    }

    Vector3 MinimumVitalSpaceRule(GameObject bj) {
        Vector3 c = new Vector3(0, 0, 0);

        foreach(GameObject b in _boids) {
            if(bj != b) {
                if(Vector3.Distance(b.transform.position, bj.transform.position) < 4) {
                    c = c- (b.transform.position - bj.transform.position);
                }
            }
        }

        return c;
    }

    Vector3 BoidsTryToKeepUpRule(GameObject bj) {
        Vector3 pvj = new Vector3(0, 0, 0);

        foreach(GameObject b in _boids) {
            if(bj != b) {
                pvj += b.rigidbody.velocity;
            }
        }

        pvj /= (_boids.Length - 1);

        return (pvj - bj.rigidbody.velocity) / 5;
    }

    /*
    Vector3 GoThereRule(GameObject bj) {
        Vector3 place = new Vector3(0, 0, 0);
        return (place - bj.transform.position) / 100;
    }
    */

    void LimitVelocity(GameObject b) {
        float vlim = 0.001f;

        if(Mathf.Abs(b.rigidbody.velocity.magnitude) > vlim) {
            b.rigidbody.velocity = (b.rigidbody.velocity / Mathf.Abs(b.rigidbody.velocity.magnitude)) * vlim;
        }
    }

    Vector3 StayHereRule(GameObject b) {
        var xMin = -50;
        var xMax = 50;
        var yMin = 0;
        var yMax = 50;
        var zMin = -50;
        var zMax = 50;

        Vector3 v = new Vector3(0, 0, 0);

        if(b.transform.position.x < xMin)
			v.x = 1;
		else if (b.transform.position.x > xMax)
			v.x = -1;

        if (b.transform.position.y < yMin)
			v.y = 1;
		else if (b.transform.position.y > yMax)
			v.y = -1;

		if (b.transform.position.z < zMin)
			v.z = 1;
		else if (b.transform.position.z > zMax)
			v.z = -1;
		
		return v;
    }
}

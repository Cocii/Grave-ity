using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakgravityLevitation : MonoBehaviour
{
    PlayerManager pManager;
    GravityManager gManager;
    public Rigidbody2D body;
    public Vector2 force;

    [Header("Levitation settings")]
    public float gravityMult;
    public float cosSpeed;
    [Header("Direction and rotation")]
    public float rangeToInfluenceLevitation = 4f;
    public float rotationSpeed;
    private float time;
    private float cosTime;


    //public float minHeight;
    //public float maxHeight;

    void Start()
    {
        pManager = PlayerManager.instance;
        gManager = GravityManager.instance;

        if (body == null)
            body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (gManager.gravityRatio >= 1f) {
            time = 0;
            return;
        }

        Vector2 gravity = gManager.physicsGravity;
        Vector2 forceDirection = -gravity.normalized;

        if (Vector3.Distance(transform.position, pManager.transform.position) <= rangeToInfluenceLevitation) {
            //bool playerOnTheRight = transform.position.x < pManager.transform.position.x;
            Vector2 dirToSum = new Vector2(Mathf.Sign(transform.position.x - pManager.transform.position.x), 0f);
            //print(dirToSum);
            forceDirection = (forceDirection * 3f + dirToSum).normalized;

        }

        cosTime = (Mathf.Cos(time * cosSpeed) + 1) * 0.5f;
        time += Time.fixedDeltaTime;

        float forceMagnitude = gManager.physicsGravity.magnitude * cosTime * gravityMult;
        force = forceDirection * (forceMagnitude * forceDirection.y);
        //Debug.DrawRay(transform.position, force * 0.3f, Color.blue);
        //print(force.magnitude);

        
    }

    private void FixedUpdate() {
        if (gManager.gravityRatio >= 1f) {
            return;
        }

        body.AddForce(force);

        float speed = rotationSpeed * Time.smoothDeltaTime * -Mathf.Sign(transform.position.x - pManager.transform.position.x);
        transform.Rotate(0f, 0f, speed);
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeToInfluenceLevitation);

        Gizmos.color = Color.white;
    }
}

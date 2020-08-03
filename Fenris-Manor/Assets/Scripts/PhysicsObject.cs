using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {
    protected const float minMoveDistance = 1f / 16f;
    
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;

    protected Rigidbody2D rb2d;
    protected BoxCollider2D boxCollider2D;
    protected Vector2 velocity;
    [SerializeField]
    protected LayerMask layerMask;

    protected bool isGrounded;

    protected virtual void Awake() {
        Debug.Log("Awake is run");
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update() {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
        AnimateActor();
    }

    protected virtual void ComputeVelocity() {

    }

    protected virtual void AnimateActor( ) {

    }

    void FixedUpdate(){
        isGrounded = ObjectIsGrounded();

        // Apply gravity to the object
        rb2d.velocity += (gravityModifier * Physics2D.gravity * Time.deltaTime);

        velocity.x = targetVelocity.x;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 move = Vector2.right * deltaPosition.x;

        Movement(move);
    }

    void Movement(Vector2 move) {
        float distance = move.magnitude;
        if (distance > minMoveDistance) {
            rb2d.position += move.normalized * distance;
        }        
    }

    protected bool ObjectIsGrounded() {
        float shellDistance = 0.1f;
        if (rb2d.velocity.y == 0) {
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, shellDistance, layerMask);
            return raycastHit;
        }
        return false;
    }
}
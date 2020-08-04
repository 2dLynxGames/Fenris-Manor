using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    * Each object to be affected by collision requires the following:
    *      Rigidbody2D to apply physics changes to the actor
    *      Rigidbody2D gravity to be a non 0 value (if the unit should fall due to gravity)
    *      BoxCollider2D for grounded calculations
    *          should be a child with the foot layer, to avoid collisions with other actors
    *          should be small and only ground adjacent
    *          must be attached to script in inspector
    *          must be ground adjacent
    * Insure all objects to collide with are on the Terrain layer
*/

public class PhysicsObject : MonoBehaviour {
    protected const float minMoveDistance = 1f / 16f;
    
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;

    protected Rigidbody2D rb2d;
    public BoxCollider2D actorFeetCollider;
    protected Vector2 velocity;
    [SerializeField]
    protected LayerMask layerMask;

    protected bool isGrounded;


    protected virtual void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
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
        Color rayColor;
        RaycastHit2D raycastHit = Physics2D.BoxCast(actorFeetCollider.bounds.center, actorFeetCollider.bounds.size, 0f, Vector2.down, shellDistance, layerMask);
        if (raycastHit && raycastHit.collider.transform.position.y > transform.position.y) {
            Debug.Log("Hit Head");
            return false;
        }
        return raycastHit;
/*          if (raycastHit) {
            rayColor = Color.magenta;
        } else {
            rayColor = Color.red;
        }
        Debug.DrawRay((actorFeetCollider.bounds.center - new Vector3(actorFeetCollider.bounds.extents.x, 0)), (Vector2.down * (actorFeetCollider.bounds.extents.y + shellDistance)), rayColor);
        Debug.DrawRay((actorFeetCollider.bounds.center + new Vector3(actorFeetCollider.bounds.extents.x, 0)), (Vector2.down * (actorFeetCollider.bounds.extents.y + shellDistance)), rayColor);
        Debug.DrawRay((actorFeetCollider.bounds.center - new Vector3(actorFeetCollider.bounds.extents.x, actorFeetCollider.bounds.extents.y + shellDistance)), (Vector2.right * (actorFeetCollider.bounds.size.x)), rayColor);
        Debug.DrawRay((actorFeetCollider.bounds.center + new Vector3(-actorFeetCollider.bounds.extents.x, 0)), (Vector2.right * (actorFeetCollider.bounds.size.x)), rayColor); */
    }
}
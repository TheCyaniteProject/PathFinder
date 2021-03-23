using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NpcController : MonoBehaviour
{
    public PathFinder pathFinder;
    public float speed = 20;
    [Space]
    public float maxDistance = 10f;
    public float minDistance = 2f;
    [Space]
    public float maxHealth = 10;
    public float health = 10;
    public float maxMana = 10;
    public float mana = 10;
    Rigidbody2D body;
    Vector2 target;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        target = transform.position;
    }

    private void FixedUpdate()
    {
        if (pathFinder.path == null || pathFinder.path.Count < 2)
            return;

        target = pathFinder.path[1].worldPosition;
        if (Vector2.Distance(transform.position, pathFinder.target.position) > maxDistance || Vector2.Distance(transform.position, pathFinder.target.position) < minDistance)
            return;

        Vector2 velocity = Vector2.zero;
        if (target.x - transform.position.x > 0.1f) velocity.x = 1; else if (target.x - transform.position.x  < -0.1f) velocity.x = -1; else velocity.x = 0;
        if (target.y - transform.position.y  > 0.1f) velocity.y = 1; else if (target.y - transform.position.y < -0.1f) velocity.y = -1; else velocity.y = 0;
        body.AddForce(new Vector2(velocity.x * 10 * speed, velocity.y * 10 * speed));
    }
}

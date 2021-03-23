using UnityEngine;


public class DemoPlayer : MonoBehaviour
{
    private Rigidbody2D body;
    public float speed = 20;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        body.AddForce(new Vector2(Input.GetAxis("Horizontal") * 10 * speed, Input.GetAxis("Vertical") * 10 * speed));
    }
}
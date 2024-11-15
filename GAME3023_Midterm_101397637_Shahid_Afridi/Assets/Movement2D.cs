using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
// This is an Attribute in C#, specifically it is one defined by the...
// Unity API called RequireComponent which means this component cannot exist on a...
// GameObject without the other required component type
public class Movement2D : MonoBehaviour
{
    [SerializeField] // SerializeField is an attribute which says that this variable can be saved,
                     // and will be saved in the scene
    public Rigidbody2D rb;


    [Range(0f, 100f)]
    public float moveSpeed = 6;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector =
            new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
                );

        rb.velocity = inputVector.normalized * moveSpeed;

        Vector3 position;
        position.x = 1;
    }
}

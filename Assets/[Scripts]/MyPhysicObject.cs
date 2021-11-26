using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CollisionShapeE
{
    SPHERE,
    PLANE
}

public class MyPhysicObject : MonoBehaviour
{
    public float Mass;
    public Vector3 Velocity;
    public CollisionShapeE shape;
    float radius = 0.0f;

    private Vector3 PreviousPosition;
    private Vector3 NewPosition;


    // Start is called before the first frame update
    void Start()
    {
        if(shape == CollisionShapeE.SPHERE)
        {
            radius = transform.localScale.x / 2;
        }
        NewPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shape != CollisionShapeE.PLANE)
        {

            PreviousPosition = transform.position;
            transform.position = NewPosition;


            // for gravity, Df = Di + AT
            Velocity.y = Velocity.y + Vector3.down.y * MyPhysicsSystem.GRAVITY * Time.deltaTime;
            //Velocity.x = Velocity.x * Time.deltaTime * 0.001f;
            //Velocity.z = Velocity.z * Time.deltaTime * 0.001f;

            NewPosition = transform.position + Velocity * Time.deltaTime;
        }
    }

    public void Collision(MyPhysicObject otherObj)
    {
        //Debug.Log("Collision!! with " + otherObj.name);
        NewPosition = transform.position;
        //InitVelocity();
        if (shape != CollisionShapeE.PLANE)
        {
            ChagneMaterialColorRandom();
        }

    }

    public float GetRadius()
    {
        return radius;
    }

    public void InitVelocity()
    {
        Velocity.x = 0;
        Velocity.y = 0;
        Velocity.z = 0;
    }

    public Vector3 GetNewPosition()
    {
        return NewPosition;
    }

    private void ChagneMaterialColorRandom()
    {
        Color ownColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        GetComponent<Renderer>().material.color = ownColor;
    }

}

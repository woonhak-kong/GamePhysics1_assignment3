using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPhysicsSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public List<MyPhysicObject> gameObjectList;
    public float Gravity = 9.81f;
    public static float GRAVITY;


    private void Awake()
    {
        GRAVITY = Gravity;
        gameObjectList = new List<MyPhysicObject>(FindObjectsOfType<MyPhysicObject>());
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (MyPhysicObject obj in gameObjectList)
        {
            //float newPositionY = obj.transform.position.y + gravity * Time.deltaTime * 0.1f;
            //obj.transform.position = new Vector3(obj.transform.position.x, newPositionY, 0);
            //Vector3 newAccelerationVector3 = obj.GetComponent<MyPhysicObject>().Velocity + Vector3.down * gravity * Time.deltaTime;
            //obj.GetComponent<MyPhysicObject>().Velocity = newAccelerationVector3;
        }

        DetectCollision();
    }

    void DetectCollision()
    {
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            for (int j = i + 1; j < gameObjectList.Count; j++)
            {

                if (CheckCollision(gameObjectList[i], gameObjectList[j]))
                {
                    gameObjectList[i].Collision(gameObjectList[j]);
                    gameObjectList[j].Collision(gameObjectList[i]);
                }
            }
        }
    }

    bool CheckCollision(MyPhysicObject obj1, MyPhysicObject obj2)
    {
        MyPhysicObject first = obj1;
        MyPhysicObject second = obj2;


        // PLANE Shape is always should be in first;
        switch(obj1.shape)
        {
            case CollisionShapeE.SPHERE:
                if(obj2.shape == CollisionShapeE.SPHERE)
                {
                    first = obj1;
                    second = obj2;
                }
                else if(obj2.shape == CollisionShapeE.PLANE)
                {
                    first = obj2;
                    second = obj1;
                }
                break;

            case CollisionShapeE.PLANE:
                if (obj2.shape == CollisionShapeE.SPHERE)
                {
                    first = obj1;
                    second = obj2;
                }
                else if (obj2.shape == CollisionShapeE.PLANE)
                {
                    first = obj1;
                    second = obj2;
                }
                break;

            default:
                break;
        }

        if(first.shape == CollisionShapeE.SPHERE && second.shape == CollisionShapeE.SPHERE)
        {
            Vector3 subResult = second.GetNewPosition() - first.GetNewPosition();
            float distanceBetweenObjs = Mathf.Sqrt(subResult.x * subResult.x + subResult.y * subResult.y + subResult.z * subResult.z);
            //Debug.Log("--------- " + distanceBetweenObjs);
            // when the distance between two objects is less than the sum of two objects' radious, they collided.
            if(distanceBetweenObjs < first.GetRadius() + second.GetRadius())
            {
                Vector3 normalFirst = subResult.normalized;
                Vector3 normalVelocityFirst = normalFirst * Vector3.Dot(normalFirst, first.Velocity);
                Vector3 tangentialVelocityFirst = first.Velocity - normalVelocityFirst;

                Vector3 normalSecond = -normalFirst;
                Vector3 normalVelocitySecond = normalSecond * Vector3.Dot(normalSecond, second.Velocity);
                Vector3 tangentialVelocitySecond = second.Velocity - normalVelocitySecond;

                first.Velocity = (normalVelocityFirst * (first.Mass - second.Mass) + normalVelocitySecond * (2 * second.Mass)) / (first.Mass + second.Mass) + tangentialVelocityFirst;
                second.Velocity = (normalVelocitySecond * (second.Mass - first.Mass) + normalVelocityFirst * (2 * first.Mass)) / (first.Mass + second.Mass) + tangentialVelocitySecond;

                return true;
            }
        }
        else if (first.shape == CollisionShapeE.PLANE && second.shape == CollisionShapeE.SPHERE)
        {
            Vector3 upvectorOfPlane = first.transform.up;
            Vector3 positionVectorOfSphere = second.GetNewPosition() - first.GetNewPosition();
            // using dot product find distance between plane and sphere
            // if distance is less than sphere's radius, they collide.
            float distanceBetweenObjs = Vector3.Dot(upvectorOfPlane, positionVectorOfSphere);
            if(distanceBetweenObjs < second.GetRadius())
            {
                // response
                Vector3 normalVelocity =  -upvectorOfPlane * Vector3.Dot(second.Velocity, -upvectorOfPlane);
                Vector3 tangentialVelocity = second.Velocity - normalVelocity;

                second.Velocity = -normalVelocity + tangentialVelocity;


                return true;
            }

        }
        else if (first.shape == CollisionShapeE.PLANE && second.shape == CollisionShapeE.PLANE)
        {

        }

        return false;
    }
}

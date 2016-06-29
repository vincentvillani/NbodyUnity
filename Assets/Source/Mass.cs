using UnityEngine;
using System.Collections;



public class Mass 
{
    public int id;
    public float mass;
    public Vector3 velocity;
    public GameObject gameobject;

    public Mass(int id, float mass, Vector3 velocity, Vector3 position)
    {
        this.id = id;
        this.mass = mass;
        this.velocity = velocity;


        gameobject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameobject.transform.position = position;
        gameobject.transform.localScale = new Vector3(mass, mass, mass);
        GameObject.Destroy(gameobject.GetComponent<SphereCollider>());
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Manager : MonoBehaviour 
{
    const float POSITIONVALUE = 1000.0f;

    const float VELOCITYVALUE = 0.5f;

    const float MINVALUEMASS = 0.1f;
    const float MAXVALUEMASS = 100.0f;

    const int MASS_NUMBER = 300;

    List<Mass> massList;


	void Start () 
    {
        massList = new List<Mass>();

        for(int i = 0; i < MASS_NUMBER; ++i)
        {
            massList.Add(new Mass(i,
                Random.Range(MINVALUEMASS, MAXVALUEMASS),
                new Vector3(Random.Range(-VELOCITYVALUE, VELOCITYVALUE), Random.Range(-VELOCITYVALUE, VELOCITYVALUE), Random.Range(-VELOCITYVALUE, VELOCITYVALUE)),
                new Vector3(Random.Range(-POSITIONVALUE, POSITIONVALUE), Random.Range(-POSITIONVALUE, POSITIONVALUE), Random.Range(-POSITIONVALUE, POSITIONVALUE))));
        }
	}
	



	void Update () 
    {
        //Update force interactions
	    for(int i = 0; i < MASS_NUMBER; ++i)
        {
            for(int j = i + 1; j < MASS_NUMBER; ++j)
            {
                CalculateForceInteraction(massList[i], massList[j]);
            }
        }

        //Update the positions
        for(int i = 0; i < MASS_NUMBER; ++i)
        {
            massList[i].gameobject.transform.position += massList[i].velocity * Time.deltaTime;
        }

    }


    //6.674×10^−11
    public void CalculateForceInteraction(Mass mass1, Mass mass2)
    {
        //const float gravitionalConstant = 0.00000000006674f;

        //Vector is from mass1 to mass2
        Vector3 distanceVector = mass2.gameobject.transform.position - mass1.gameobject.transform.position;
        float distanceSquared = distanceVector.sqrMagnitude;
        Vector3 distanceVectorNormalised = distanceVector.normalized;

        float force = ((mass1.mass * mass2.mass) / distanceSquared); //gravitionalConstant * ( (mass1.mass * mass2.mass) / distanceSquared);

        Vector3 forceVector = force * distanceVectorNormalised * Time.deltaTime;
        mass1.velocity += forceVector;
        mass2.velocity -= forceVector;

    }
}

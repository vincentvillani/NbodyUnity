using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO: Properly calculate the forces applied to two objects of different mass colliding
//TODO: Give the bigger mass half of the other mass on collision

public class Manager : MonoBehaviour 
{
    const float POSITIONVALUE = 100.0f;

    const float VELOCITYVALUE = 0.5f;

    const float MINVALUEMASS = 0.1f;
    const float MAXVALUEMASS = 10.0f;

    const int MASS_NUMBER = 500;

    const float simulationDelta = 1.0f / 144.0f;

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
	    for(int i = 0; i < massList.Count; ++i)
        {

            for(int j = i + 1; j < massList.Count; ++j)
            {
                CalculateForceInteraction(massList[i], massList[j]);
            }
        }

        //Update the positions
        for(int i = 0; i < massList.Count; ++i)
        {
            massList[i].gameobject.transform.position += massList[i].velocity * simulationDelta;
        }

        
        //Check for collisions
        for (int i = 0; i < massList.Count; ++i)
        {
            for (int j = i + 1; j < massList.Count; ++j)
            {

                Vector3 distanceVector = massList[i].gameobject.transform.position - massList[j].gameobject.transform.position;
                float distance = distanceVector.magnitude - (massList[i].mass / 2.0f) - (massList[j].mass / 2.0f);

                //If the squared distance is less than the mass (mass is also the radius), then a collision has occured
                if(distance <= 0)
                {
                    //Normalise the distance vector
                    distanceVector.Normalize();

                    //Find the length of the component of each of the velocity vectors along the normalised distance vector
                    float a1 = Vector3.Dot(massList[i].velocity, distanceVector);
                    float a2 = Vector3.Dot(massList[j].velocity, distanceVector);

                    //Calculate the common term in both v1' and v2'
                    float commonTerm = (2.0f * (a1 - a2)) / (massList[i].mass + massList[j].mass);

                    //Calculate the new velocity vector of mass1
                    massList[i].velocity = massList[i].velocity - commonTerm * massList[j].mass * distanceVector;

                    //Calculate the new velocity vector of mass2
                    massList[j].velocity = massList[j].velocity + commonTerm * massList[i].mass * distanceVector;
                }
            }
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

        Vector3 forceVector = force * distanceVectorNormalised * simulationDelta;
        mass1.velocity += forceVector;
        mass2.velocity -= forceVector;

    }



    /*
    public void MergeMasses(int index1, int index2)
    {
        const float scaleFactor = 8.0f;

        Mass mass1 = massList[index1];
        Mass mass2 = massList[index2];


        mass1.velocity += mass2.velocity / 2.0f;
        mass1.mass += (mass2.mass / scaleFactor);
        mass1.gameobject.transform.localScale = new Vector3(mass1.mass, mass1.mass, mass1.mass);

        GameObject.Destroy(mass2.gameobject);
        massList.RemoveAt(index2);
    }
    */
}

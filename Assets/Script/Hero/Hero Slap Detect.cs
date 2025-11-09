using UnityEngine;

public class HeroSlapDetect : MonoBehaviour
{
    public Vector2 locationDifference;

    public void CalculateSlapLocation(Vector2 slapVector2)
    {
        Vector2 heroLocation = this.transform.position;

        locationDifference = heroLocation - slapVector2;

        if (locationDifference.x < -0.3 && locationDifference.y > 0)
        {

            Debug.Log("Somewhere Low Right");
        }
        else if(locationDifference.x > 0.3 && locationDifference.y > 0)
        {

            Debug.Log("Somewhere Low Left");
        }
        else if(locationDifference.x < 0 && locationDifference.y < 0)
        {

            Debug.Log("Somewhere Top Right");
        }
        else if(locationDifference.x > 0 && locationDifference.y < 0)
        {

            Debug.Log("Somewhere Top Left");
        }
        else if (locationDifference.x < 0.3 && locationDifference.x > -0.3 && locationDifference.y > 0)
        {

            Debug.Log("Middle");
        }
    }
}

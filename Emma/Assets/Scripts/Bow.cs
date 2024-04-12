using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add missing import for Slider

public class Bow : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] SpriteRenderer arrowGFX;


    [SerializeField] Slider bowPowerSlider;
    [Range(0, 10)]
    public float bowPower;
    [Range(0, 3)]
    public float maxBowCharge;
    float bowCharge;
    bool canFire = true;

    public GameObject point;
    GameObject[] points;
    public Transform shotPoint;
    public int numberOfPoints;
    public float spaceBetweenPoints;
    Vector2 direction;

    private void Start() {
        bowPowerSlider.value = 0f;
        bowPowerSlider.maxValue = maxBowCharge;

        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, shotPoint.position, Quaternion.identity);
        }
    }

    void Update()
    {
        // Rotates the bow to face the mouse
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - bowPosition;
        transform.right = direction;

        if (Input.GetMouseButtonDown(0)&& canFire)
        {
            chargeBow();
        }

        // If the player has released the button, shoot the arrow
        else if (Input.GetMouseButtonUp(0))
        {
           shootBow();
        } else {
            if (bowCharge > 0f)
            {
                bowCharge -= 1f * Time.deltaTime;
            } else {
                bowCharge = 0f;
                canFire = true;
            }

            bowPowerSlider.value = bowCharge;
        }

        // Update the position of the points
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i].transform.position = PointPosition(i * spaceBetweenPoints);
        }
    }

    void chargeBow() {
        arrowGFX.enabled = true;
        bowCharge += Time.deltaTime;

        bowPowerSlider.value = bowCharge;

        if (bowCharge > maxBowCharge)
        {
            bowPowerSlider.value = maxBowCharge;
            
        }
    }
  
    void shootBow() {

        if (bowCharge > maxBowCharge) bowCharge = maxBowCharge;

        float arrowSpeed = bowCharge + bowPower;
        
        GameObject newArrow = Instantiate(arrowPrefab, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * arrowSpeed;

        canFire = false;
        arrowGFX.enabled = false;
    }

    Vector2 PointPosition(float t) {
        Vector2 position = (Vector2)shotPoint.position + (direction.normalized * bowCharge * t) + 0.5f * Physics2D.gravity * (t * t);
        return position;
    }
}

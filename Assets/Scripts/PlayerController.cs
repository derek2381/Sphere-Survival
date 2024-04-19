using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public float speed = 3.0f;
    public bool hasPowerUp = false;
    private float powerUpStrength = 15.0f;
    public GameObject powerUpIndicator;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        // if(transform.position.y < -10){
        //   // Debug.Log("Game Over");
        //   Application.LoadLevel(0);
        // }
    }

    private void OnTriggerEnter(Collider other){
      if(other.CompareTag("PowerUp")){
        hasPowerUp = true;
        Destroy(other.gameObject);
        StartCoroutine(PowerUpCountdownRoutine());
        powerUpIndicator.gameObject.SetActive(true);
      }
    }

    IEnumerator PowerUpCountdownRoutine(){
      yield return new WaitForSeconds(7);
      hasPowerUp = false;
      powerUpIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision){
      if(collision.gameObject.CompareTag("Enemy") && hasPowerUp){
        Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

        Debug.Log("Player has collided with : " + collision.gameObject.name + " with power up set to " + hasPowerUp);
        enemyRigidBody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
      }
    }
}

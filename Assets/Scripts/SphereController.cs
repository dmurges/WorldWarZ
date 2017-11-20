using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
    }

    void FixedUpdate() {

        //Vector3 dir = Vector3.zero;
        //dir.x = Input.acceleration.x;
        //dir.z = -Input.acceleration.z;
        //if (dir.sqrMagnitude > 1)
        //    dir.Normalize();

        //dir *= Time.deltaTime;
        //transform.Translate(dir * 10.0f);
        // Adding force to rigidbody
        //GetComponent<Rigidbody>().(movement * 10.0f * Time.deltaTime);

        Vector3 movement = new Vector3(Input.acceleration.x, 0.0f, -Input.acceleration.z);
        GetComponent<Rigidbody>().AddForce(movement * 1500.0f * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col) {
        GameObject characterObject = col.gameObject;
        ZombieCharacter characterComponent = characterObject.GetComponent<ZombieCharacter>();
        if (characterComponent)
        {
            characterComponent.TakeImpact();
            characterObject.GetComponent<Health>().DecreaseHealth(200.0f);
        }
    }
}

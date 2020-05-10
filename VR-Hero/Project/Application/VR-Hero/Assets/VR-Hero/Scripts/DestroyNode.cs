using UnityEngine;
using System.Collections;

public class DestroyNode : MonoBehaviour {

    public float lifeTime = 10;

    void Awake() {
        Destroy(gameObject, lifeTime);
    }

}

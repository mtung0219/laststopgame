using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedCameraSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveCameraRightOne()
    {
        this.transform.position = new Vector3(this.transform.position.x + 10, this.transform.position.y, this.transform.position.z);
    }
}

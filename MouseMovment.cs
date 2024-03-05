using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovment : MonoBehaviour
{

    public float mouseSensetivity = 100f;

    float xRotation = 0;
    float yRotation = 0;

    public float topClamp = -90f;
    public float bottomClam = 90f;

    // Start is called before the first frame update
    void Start()
    {
        //locking the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //get the input for mouse 
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;

        //rotation around the x axis (look up and down)
        xRotation -= mouseY;

        //clamp the rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClam);

        //rotation around the y axis (look ledt and right)
        yRotation += mouseX;

        //apply the rotation to our transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void onInput(Vector3 direction);

    public event onInput OnInput;

    public static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Left
            if (OnInput != null)
            {
                OnInput(Vector3.left);
            }
        }

        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Right 
            if (OnInput != null)
            {
                OnInput(Vector3.right);
            }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Up
            if (OnInput != null)
            {
                OnInput(Vector3.forward);
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Down
            if (OnInput != null)
            {
                OnInput(Vector3.back);
            }
        }
    }


}

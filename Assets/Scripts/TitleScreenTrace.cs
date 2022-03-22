using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TitleScreenTrace : MonoBehaviour
{
    private Vector2 cursorPosition;
    private Vector2 lightPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //float HeightLimit = PositionRestriction(cursorPosition.x);

        if (cursorPosition.y <= 400)
        {
            lightPosition.x = cursorPosition.x;
            lightPosition.y = cursorPosition.y;
            transform.position = lightPosition;
        }
    }

    private float PositionRestriction(float x)
    {
        return x;
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        cursorPosition = context.ReadValue<Vector2>();
    }
}

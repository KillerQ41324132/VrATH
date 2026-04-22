using UnityEngine;
using UnityEngine.InputSystem;

public class locomotion_by_VR : MonoBehaviour
{
    [SerializeField] private InputActionReference rightJoystick;
    private float joystickDeadzone = 0.5f; // martwa strefa, by unikn¹æ drgañ
    [SerializeField] private float moveSpeed;

    private void OnEnable()
    {
        if (rightJoystick != null)
            rightJoystick.action.Enable();
    }

    private void OnDisable()
    {
        if (rightJoystick != null)
            rightJoystick.action.Disable();
    }

    void Update()
    {
        if (rightJoystick == null) return;

        Vector2 input = rightJoystick.action.ReadValue<Vector2>();

        // Sprawdzenie, czy ga³ka jest przesuniêta ponad deadzone
        if (input.magnitude > joystickDeadzone)
        {
            // Sprawdzenie, czy ruch jest w przód (Y jest dodatnie i wiêksze ni¿ bezwzglêdna wartoœæ X)
            if (input.y > Mathf.Abs(input.x))
            {
                // Ruch jest wykonywany ca³y czas, gdy joystick jest wciœniêty do przodu
                transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
            }
        }
    }
}
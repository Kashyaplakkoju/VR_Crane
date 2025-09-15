using UnityEngine;

public class CraneMechanism : MonoBehaviour
{
    public GameObject belowCube; // Reference to the below cube with the configurable joint
    public float movementSpeed = 5f; // Speed of the kinematic cube's movement
    public float anchorAdjustmentSpeed = 0.1f; // Speed of adjusting the joint's connected anchor Y-axis
    private ConfigurableJoint configurableJoint;

    void Start()
    {
        if (belowCube != null)
        {
            configurableJoint = belowCube.GetComponent<ConfigurableJoint>();
        }
    }

    void Update()
    {
        HandleMovement();
        AdjustJointAnchor();
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);
    }

    void AdjustJointAnchor()
    {
        if (configurableJoint != null)
        {
            if (Input.GetKey(KeyCode.X))
            {
                Vector3 newAnchor = configurableJoint.connectedAnchor;
                newAnchor.y += anchorAdjustmentSpeed * Time.deltaTime;
                configurableJoint.connectedAnchor = newAnchor;
            }
            else if (Input.GetKey(KeyCode.C))
            {
                Vector3 newAnchor = configurableJoint.connectedAnchor;
                newAnchor.y -= anchorAdjustmentSpeed * Time.deltaTime;
                configurableJoint.connectedAnchor = newAnchor;
            }
        }
    }
}
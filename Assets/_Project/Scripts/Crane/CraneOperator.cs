//using GLTFast.Schema;
using System.Collections;
using UnityEngine;

public class CraneOperator : MonoBehaviour
{
    [SerializeField] Transform main;
    [SerializeField] Transform block;
    [SerializeField] float craneSpeed = 1f;
    [SerializeField] float distance = 0.1f;
    [SerializeField] float decelerationFactor = 0.9f; // Factor to decrease speed as rope goes down
    [SerializeField] float currentspeed =80;
    [SerializeField] Transform sloop1;
    [SerializeField] Transform sloop2;
    [SerializeField] Transform Pulley;
    [SerializeField] Transform hostingGear;
    [SerializeField] Transform missileGear;


    [SerializeField] Vector2 horizontalLimit;
    [SerializeField] Vector2 verticalLimit;  // Modify this dynamically
    [SerializeField] Vector2 lateralLimit;

    private float originalVerticalLimitY;  // Store the original vertical limit y


    [SerializeField] public float hookLength = 0f;  // Length limit for hosting gear
    [SerializeField] public float missileLength = 0f; // Length limit for missile gear

    private float currentRopeLength = 0f;
    private float maxRopeLength = 100f;
    private float minRopeLength = 0f;


 


    public bool isHostingGear = false; // For Hosting Gear mode
    public bool ismissile = false;     // For Missile mode

    private bool movingDown;
    private bool movingUp;

    private HingeJoint joint1;
    private HingeJoint joint2;


    // public Collider pulleyCollider;

    void Start()
    {
        joint1 = sloop1.GetComponent<HingeJoint>();
        joint2 = sloop2.GetComponent<HingeJoint>();

        // Store the original vertical limit y
        originalVerticalLimitY = verticalLimit.x;

    }

    void Update()
    {
        // Adjust verticalLimit.y based on the current mode (hosting or missile)
        if (isHostingGear)
        {
            verticalLimit.x = hookLength;
        }
        else if (ismissile)
        {
            verticalLimit.x = missileLength;
        }
        else
        {
            // If both are false, revert to the original vertical limit y
            verticalLimit.x = originalVerticalLimitY;
        }

        // Max limit for the rope length based on the active mode
        float maxLimit = isHostingGear ? hookLength : (ismissile ? missileLength : maxRopeLength);

        if (Pulley.position.y > verticalLimit.y && !movingDown)
        {
            Stop();
        }
        if (Pulley.position.y < verticalLimit.x && !movingUp)
        {
            Stop();
        }

        // Stop the crane if the rope length exceeds the maximum limit for the mode
        if (currentRopeLength > maxLimit)
        {
            Stop();
        }


            float distanceToHostingGear = Vector3.Distance(Pulley.position, hostingGear.transform.position);
            float distanceToMissileGear = Vector3.Distance(Pulley.position, missileGear.transform.position);

            if (distanceToHostingGear < distance) // Adjust range as needed
            {
                SetMode(true, false);
                Debug.Log("Hosting Gear Attached");
            }
            else if (distanceToMissileGear < distance)
            {
                SetMode(false, true);
                Debug.Log("Missile Gear Attached");
            }
            else
            {
                SetMode(false, false);
                Debug.Log("No Attatchement");
            }
        

    }

    public void SetMode(bool hostingGear, bool missile)
    {
        isHostingGear = hostingGear;
        ismissile = missile;

        // Set max rope length dynamically based on the selected mode
        maxRopeLength = isHostingGear ? hookLength : (ismissile ? missileLength : maxRopeLength);
    }

    public void SetBlock(Transform Block)
    {
        block = Block;
    }
    public void SetPulley(Transform Pulleys)
    {
        Pulley = Pulleys;
    }

    public void SetSpool1(Transform Spool1)
    {
        sloop1 = Spool1;
    }

    public void SetSpool2(Transform Spool2)
    {
        sloop2 = Spool2;
    }


    public void StartCour(string func)
    {
        StartCoroutine(func);
    }

    public IEnumerator MoveRight()
    {
        while (block.position.z < lateralLimit.x)
        {
            block.position += new Vector3(0, 0, craneSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator MoveLeft()
    {
        while (block.position.z > lateralLimit.y)
        {
            block.position -= new Vector3(0, 0, craneSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator MoveFront()
    {
        while (main.position.x < horizontalLimit.x)
        {
            main.position += new Vector3(craneSpeed * Time.deltaTime, 0, 0);
            yield return null;
        }
    }

    public IEnumerator MoveBack()
    {
        while (main.position.x > horizontalLimit.y)
        {
            main.position -= new Vector3(craneSpeed * Time.deltaTime, 0, 0);
            yield return null;
        }
    }

    public void MoveDown()
    {
        movingDown = true;
        movingUp = false;
        JointMotor motor1 = joint1.motor;
        motor1.targetVelocity = -80;
        JointMotor motor2 = joint2.motor;
        motor2.targetVelocity = 80;
        joint1.motor = motor1;
        joint2.motor = motor2;
    }

    public void MoveUp()
    {
        movingUp = true;
        movingDown = false;
        JointMotor motor1 = joint1.motor;
        motor1.targetVelocity = 80;
        JointMotor motor2 = joint2.motor;
        motor2.targetVelocity = -80;
        joint1.motor = motor1;
        joint2.motor = motor2;
    }


    public void Stop()
    {
        JointMotor motor1 = joint1.motor;
        motor1.targetVelocity = 0;
        JointMotor motor2 = joint2.motor;
        motor2.targetVelocity = 0;

        joint1.motor = motor1;
        joint2.motor = motor2;
    }

    // Add Gizmos to visualize limits
    private void OnDrawGizmos()
    {
        // Draw horizontal limits
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(horizontalLimit.x, transform.position.y, transform.position.z), new Vector3(horizontalLimit.y, transform.position.y, transform.position.z));

        // Draw vertical limits
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x, verticalLimit.x, transform.position.z), new Vector3(transform.position.x, verticalLimit.y, transform.position.z));

        // Draw lateral limits
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, lateralLimit.x), new Vector3(transform.position.x, transform.position.y, lateralLimit.y));
    }
}


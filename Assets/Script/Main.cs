using System.Collections;
using UnityEngine;
using dynamixel_sdk;

public class Main : MonoBehaviour
{
    public Transform platform1;
    public Transform platform2;
    public GameObject ball;
    private GameObject currentBallGauche;
    private GameObject currentBallDroite;

    [Header("Dynamixel")]
    public string portName = "COM3";
    public int baudRate = 57600;
    public int protocolVersion = 2;
    public byte platform1Id = 1;
    public byte platform2Id = 2;
    public ushort goalPositionAddress = 116;
    public ushort presentPositionAddress = 132;
    public ushort torqueEnableAddress = 64;
    public ushort torqueLimitAddress = 34;
    public ushort torqueLimitBase = 200;
    public ushort torqueLimitStep = 10;
    public ushort torqueLimitMax = 1023;

    private int portHandler = -1;
    private bool isPortOpen = false;

    private const int COMM_SUCCESS = 0;
    private const byte ERROR_NONE = 0;

    public float[] platform1Position = { 1000, 618, 347 };
    public float[] platform2Position = { 1500, 1000, 600 };
    private static float[] translationPosition = { 22, 0, -22 };
    private float facility = 7f;

    private int a;
    private int previousA;
    private int b;
    private int previousB;
    public bool hasBallFallen1 = true;
    private bool instanciated1 = false;
    public bool hasBallFallen2 = true;
    private bool instanciated2 = false;

    private bool isWaiting1 = false;
    private bool isWaiting2 = false;
    private int kpMX = 0;
    private int kpXM = 0;
    private int wrongPlayer1 = 0;
    private int wrongPlayer2 = 0;

    void Start()
    {
        isPortOpen = InitializeDynamixel();
        if (isPortOpen)
        {
            EnableTorque(platform1Id);
            EnableTorque(platform2Id);
            ApplyTorqueLimit(platform1Id, kpMX);
            ApplyTorqueLimit(platform2Id, kpXM);
        }
    }

    void Update()
    {
        if (isPortOpen)
        {
            TrySyncPlatform(platform1, platform1Id, platform1Position);
            TrySyncPlatform(platform2, platform2Id, platform2Position);
        }

        if (hasBallFallen1 && !instanciated1)
        {
            float actualRotation1 = platform1.eulerAngles.z;
            if (actualRotation1 >= -facility && actualRotation1 <= facility)
            {
                SpawnBall1WithDelay();
                hasBallFallen1 = false;
            }
        }
        instanciated1 = false;

        if (hasBallFallen2 && !instanciated2)
        {
            float actualRotation2 = platform2.eulerAngles.z;
            if (actualRotation2 >= -facility && actualRotation2 <= facility)
            {
                SpawnBall2WithDelay();
                hasBallFallen2 = false;
            }
        }
        instanciated2 = false;
    }

    private void SpawnBall1WithDelay()
    {
        if (!isWaiting1)
        {
            StartCoroutine(SpawnBall1(1.0f));
        }
    }

    private void SpawnBall2WithDelay()
    {
        if (!isWaiting2)
        {
            StartCoroutine(SpawnBall2(1.0f));
        }
    }

    private void Rotate(Transform platform, float angularPositions)
    {
        float currentRotation = platform.eulerAngles.z;
        float targetRotation = angularPositions;
        float angleDiff = Mathf.DeltaAngle(currentRotation, targetRotation);
        float angleRotation = angleDiff;
        platform.Rotate(Vector3.forward * angleRotation);
    }

    private IEnumerator SpawnBall1(float delay)
    {
        isWaiting1 = true;
        yield return new WaitForSeconds(delay);
        isWaiting1 = false;

        int i = UnityEngine.Random.Range(0, 3);
        if (currentBallGauche != null)
        {
            Destroy(currentBallGauche);
        }
        currentBallGauche = Instantiate(ball, new Vector3(-5, 5, 0), Quaternion.identity);
        currentBallGauche.tag = "Gauche";

        if (i == 0)
        {
            currentBallGauche.GetComponentInChildren<Renderer>().material.color = Color.yellow;
            a = (int)platform1Position[0];
            if (a != previousA)
            {
                SendRotationCommand(platform1Id, a);
                previousA = a;
            }
        }
        else if (i == 1)
        {
            currentBallGauche.GetComponentInChildren<Renderer>().material.color = Color.red;
            a = (int)platform1Position[2];
            if (a != previousA)
            {
                SendRotationCommand(platform1Id, a);
                previousA = a;
            }
        }
        else if (i == 2)
        {
            currentBallGauche.GetComponentInChildren<Renderer>().material.color = Color.cyan;
            PhysicsMaterial2D bouncyMaterial = new PhysicsMaterial2D();
            bouncyMaterial.bounciness = 4.5f;
            bouncyMaterial.friction = 0.0f;
            Collider2D collider = currentBallGauche.GetComponentInChildren<Collider2D>();
            collider.sharedMaterial = bouncyMaterial;
            a = (int)platform1Position[2];
            if (a != previousA)
            {
                SendRotationCommand(platform1Id, a);
                previousA = a;
            }
        }
    }

    private IEnumerator SpawnBall2(float delay)
    {
        isWaiting2 = true;
        yield return new WaitForSeconds(delay);
        isWaiting2 = false;

        int j = UnityEngine.Random.Range(0, 3);
        if (currentBallDroite != null)
        {
            Destroy(currentBallDroite);
        }
        currentBallDroite = Instantiate(ball, new Vector3(5, 5, 0), Quaternion.identity);
        currentBallDroite.tag = "Droite";

        if (j == 0)
        {
            currentBallDroite.GetComponentInChildren<Renderer>().material.color = Color.green;
            b = (int)platform2Position[0];
            if (b != previousB)
            {
                SendRotationCommand(platform2Id, b);
                previousB = b;
            }
        }
        else if (j == 1)
        {
            currentBallDroite.GetComponentInChildren<Renderer>().material.color = Color.red;
            b = (int)platform2Position[2];
            if (b != previousB)
            {
                SendRotationCommand(platform2Id, b);
                previousB = b;
            }
        }
        else if (j == 2)
        {
            currentBallDroite.GetComponentInChildren<Renderer>().material.color = Color.magenta;
            PhysicsMaterial2D bouncyMaterial = new PhysicsMaterial2D();
            bouncyMaterial.bounciness = 4.5f;
            bouncyMaterial.friction = 0.0f;
            Collider2D collider = currentBallDroite.GetComponentInChildren<Collider2D>();
            collider.sharedMaterial = bouncyMaterial;
            b = (int)platform2Position[2];
            if (b != previousB)
            {
                SendRotationCommand(platform2Id, b);
                previousB = b;
            }
        }
    }

    public void WrongPlace1()
    {
        wrongPlayer1 += 1;
        if ((wrongPlayer1 % 2 == 0) && (kpXM <= 45))
        {
            kpXM += 5;
            ApplyTorqueLimit(platform2Id, kpXM);
        }
    }

    public void WrongPlace2()
    {
        wrongPlayer2 += 1;
        if ((wrongPlayer2 % 2 == 0) && (kpMX <= 45))
        {
            kpMX += 5;
            ApplyTorqueLimit(platform1Id, kpMX);
        }
    }

    public void CollisionDetectedMagenta()
    {
        ScoreScript.scoreValue += 2;
        hasBallFallen2 = true;
    }

    public void CollisionDetectedYellow()
    {
        ScoreScript.scoreValue += 1;
        hasBallFallen1 = true;
    }

    public void CollisionDetectedRedDroite()
    {
        ScoreScript.scoreValue += 1;
        hasBallFallen2 = true;
    }

    public void CollisionDetectedRedGauche()
    {
        ScoreScript.scoreValue += 1;
        hasBallFallen1 = true;
    }

    public void CollisionDetectedGreen()
    {
        ScoreScript.scoreValue += 1;
        hasBallFallen2 = true;
    }

    public void CollisionDetectedCyan()
    {
        ScoreScript.scoreValue += 2;
        hasBallFallen1 = true;
    }

    public void NotCollisionDetected1()
    {
        a = (int)platform1Position[1];
        if (a != previousA)
        {
            SendRotationCommand(platform1Id, a);
            previousA = a;
        }
    }

    public void NotCollisionDetected2()
    {
        b = (int)platform2Position[1];
        if (b != previousB)
        {
            SendRotationCommand(platform2Id, b);
            previousB = b;
        }
    }

    float InterpolatePlatformRotation(float interpolated, float[] motorPositions)
    {
        // Certifique-se de que a posição do motor está dentro dos limites conhecidos
        if (interpolated >= motorPositions[0])
        {
            return translationPosition[0];
        }
        else if (interpolated <= motorPositions[2])
        {
            return translationPosition[2];
        }
        else
        {
            // Interpolação linear entre os pares de valores conhecidos
            for (int i = 0; i < motorPositions.Length - 1; i++)
            {
                if (interpolated <= motorPositions[i] && interpolated >= motorPositions[i + 1])
                {
                    float t = (interpolated - motorPositions[i]) / (motorPositions[i + 1] - motorPositions[i]);
                    return Mathf.Lerp(translationPosition[i], translationPosition[i + 1], t);
                }
            }
        }
        return 0f; // Valor padrão se nenhuma correspondência for encontrada
    }

    private bool InitializeDynamixel()
    {
        portHandler = dynamixel.portHandler(portName);
        dynamixel.packetHandler();
        if (!dynamixel.openPort(portHandler))
        {
            Debug.LogError($"[Dynamixel] falha ao abrir {portName}");
            return false;
        }
        if (!dynamixel.setBaudRate(portHandler, baudRate))
        {
            Debug.LogError("[Dynamixel] falha ao configurar a taxa de transmissão");
            dynamixel.closePort(portHandler);
            return false;
        }
        return true;
    }

    private void TrySyncPlatform(Transform platform, byte id, float[] motorPositions)
    {
        if (!TryReadPresentPosition(id, out uint servoValue))
        {
            return;
        }
        float targetRotation = InterpolatePlatformRotation(servoValue, motorPositions);
        Rotate(platform, targetRotation);
    }

    private void SendRotationCommand(byte id, int position)
    {
        if (!isPortOpen)
        {
            return;
        }
        uint safePosition = (uint)Mathf.Max(0, position);
        dynamixel.write4ByteTxRx(portHandler, protocolVersion, id, goalPositionAddress, safePosition);
        CheckPacketResult($"posição alvo ID {id}");
    }

    private bool TryReadPresentPosition(byte id, out uint position)
    {
        position = 0;
        if (!isPortOpen)
        {
            return false;
        }
        dynamixel.read4ByteTxRx(portHandler, protocolVersion, id, presentPositionAddress);
        if (!CheckPacketResult($"leitura da posição ID {id}"))
        {
            return false;
        }
        position = dynamixel.getDataRead(portHandler, protocolVersion, 4, presentPositionAddress);
        return true;
    }

    private void EnableTorque(byte id)
    {
        if (!isPortOpen)
        {
            return;
        }
        dynamixel.write1ByteTxRx(portHandler, protocolVersion, id, torqueEnableAddress, 1);
        CheckPacketResult($"ativar torque ID {id}");
    }

    private void DisableTorque(byte id)
    {
        if (!isPortOpen)
        {
            return;
        }
        dynamixel.write1ByteTxRx(portHandler, protocolVersion, id, torqueEnableAddress, 0);
        CheckPacketResult($"desativar torque ID {id}");
    }

    private void ApplyTorqueLimit(byte id, int kpValue)
    {
        if (!isPortOpen)
        {
            return;
        }
        ushort limit = ComputeTorqueLimitValue(kpValue);
        dynamixel.write2ByteTxRx(portHandler, protocolVersion, id, torqueLimitAddress, limit);
        CheckPacketResult($"limite de torque ID {id}");
    }

    private ushort ComputeTorqueLimitValue(int kpValue)
    {
        uint rawLimit = (uint)torqueLimitBase + (uint)kpValue * torqueLimitStep;
        if (rawLimit > torqueLimitMax)
        {
            rawLimit = torqueLimitMax;
        }
        return (ushort)rawLimit;
    }

    private bool CheckPacketResult(string context)
    {
        if (!isPortOpen)
        {
            return false;
        }
        int commResult = dynamixel.getLastTxRxResult(portHandler, protocolVersion);
        if (commResult != COMM_SUCCESS)
        {
            Debug.LogWarning($"[Dynamixel] {context}: resultado de comunicação {commResult}");
            return false;
        }
        byte error = dynamixel.getLastRxPacketError(portHandler, protocolVersion);
        if (error != ERROR_NONE)
        {
            Debug.LogWarning($"[Dynamixel] {context}: erro {error}");
            return false;
        }
        return true;
    }

    private void CloseDynamixel()
    {
        if (!isPortOpen)
        {
            return;
        }
        DisableTorque(platform1Id);
        DisableTorque(platform2Id);
        dynamixel.closePort(portHandler);
        isPortOpen = false;
    }

    private void OnDestroy()
    {
        CloseDynamixel();
    }
}

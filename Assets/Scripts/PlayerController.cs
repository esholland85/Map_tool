using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    Material mat;
    TerrainGenerator mesh;

    class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        public void SetFromTransform(Transform t)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
            x = t.position.x;
            y = t.position.y;
            z = t.position.z;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

            x += rotatedTranslation.x;
            y += rotatedTranslation.y;
            z += rotatedTranslation.z;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);
        }

        public void UpdateTransform(Transform t)
        {
            t.eulerAngles = new Vector3(pitch, yaw, roll);
            t.position = new Vector3(x, y, z);
        }
    }

    CameraState m_TargetCameraState = new CameraState();
    CameraState m_InterpolatingCameraState = new CameraState();

    [Header("Movement Settings")]
    [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
    public float boost = 3.5f;

    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
    public float positionLerpTime = 0.2f;

    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;

    void OnEnable()
    {
        m_TargetCameraState.SetFromTransform(transform);
        m_InterpolatingCameraState.SetFromTransform(transform);
    }
    private void Start()
    {
        //mat = GetComponent<MeshRenderer>().material;
        //StartCoroutine(__RandomizeColor());
        //CmdRequestAuthority(angel.netIdentity);

        if (!hasAuthority)
        {
            // if cameras become irregular, re-enable in OnStartAuthority
            gameObject.GetComponent<Camera>().enabled = false;
            return;
        }
    }

    public override void OnStartAuthority()
    {
        //possibly a better location for the above change made in start. Start does not reliably run after network connectivity. Implement and test that only one player moves a camera.
        //"if" is possibly superfluous. OnStartAuthority may ONLY trigger if the script actually gains authority.
        base.OnStartAuthority();
        gameObject.GetComponent<Camera>().enabled = true;
        mesh = FindObjectOfType<TerrainGenerator>();
        mesh.playerStarted();

        if (isServer)
        {
            CmdRequestAuthority(mesh.GetComponent<NetworkIdentity>());
        }
    }

    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction += Vector3.down;
        }
        if (Input.GetKey(KeyCode.E))
        {
            direction += Vector3.up;
        }
        return direction;
    }

    void Update()
    {
        // Exit Sample  
        /*
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; 
			#endif
        }
        // Hide and lock cursor when right mouse button pressed
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Unlock and show cursor when right mouse button released
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        */
        // Rotation

        if (hasAuthority)
        {
            if (Input.GetMouseButton(1))
            {
                var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
                m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
            }

            // Translation
            var translation = GetInputTranslationDirection() * Time.deltaTime;

            // Speed up movement when shift key held
            if (Input.GetKey(KeyCode.LeftShift))
            {
                translation *= 10.0f;
            }

            // Modify movement by a boost factor (defined in Inspector and modified in play mode through the mouse scroll wheel) disabled for other scroll uses
            //boost += Input.mouseScrollDelta.y * 0.2f;
            translation *= Mathf.Pow(2.0f, boost);

            m_TargetCameraState.Translate(translation);

            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            m_InterpolatingCameraState.UpdateTransform(transform);
        }
    }

    /* multiplayer example by color change
        * private IEnumerator __RandomizeColor()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
        {
            CmdChangeColor();
            yield return wait;
        }
    }

    private void SetColor(Color32 color)
    {
        mat.SetColor("_Color", color);
    }

    [Command]
    public void CmdChangeColor()
    {
        Color32 color = Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 1f, 1f, 1f);
        SetColor(color);
        RpcChangeColor(color);
    }

    [ClientRpc]
    private void RpcChangeColor(Color32 color)
    {
        SetColor(color);
    }*/

    [Command]
    private void CmdRequestAuthority(NetworkIdentity otherId)
    {
        otherId.AssignClientAuthority(base.connectionToClient);
    }

    string mapPath;
    private IPAddress clientIp;
    
    private async void SendImage()
    {
        FileInfo file;
        FileStream fileStream;
        mapPath = Application.persistentDataPath + "/Map Images/";

        try
        {
            Debug.Log("Attempting to open: " + mapPath + GameController.imageName + ".jpg");
            file = new FileInfo(mapPath + GameController.imageName + ".jpg");
            fileStream = file.OpenRead();
            Debug.Log("File found: " + file.Name);
        }
        catch
        {
            Debug.LogError("Error opening file");
            return;
        }

        TcpClient client = new TcpClient();
        try
        {
            Debug.Log("server requested connection");
            await client.ConnectAsync(clientIp, GameController.imagePort);
        }
        catch
        {
            Debug.LogError("Error connecting");
            Debug.LogError("Trying IPv4");
            try
            {
                Debug.Log(clientIp.AddressFamily);
                await client.ConnectAsync(clientIp.MapToIPv4(), GameController.imagePort);
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting");
                Debug.LogError($"Generic Exception Handler: {e}");
                return;
            }
        }
        NetworkStream ns = client.GetStream();

        {
            byte[] fileName = ASCIIEncoding.ASCII.GetBytes(file.Name);
            byte[] fileNameLength = BitConverter.GetBytes(fileName.Length);
            byte[] fileLength = BitConverter.GetBytes(file.Length);
            await ns.WriteAsync(fileLength, 0, fileLength.Length);
            await ns.WriteAsync(fileNameLength, 0, fileNameLength.Length);
            await ns.WriteAsync(fileName, 0, fileName.Length);
        }

        {
            byte[] permission = new byte[1];
            await ns.ReadAsync(permission, 0, 1);
            if (permission[0] != 1)
            {
                Debug.LogError("Permission Denied");
                return;
            }
        }

        int read;
        int totalWritten = 0;
        byte[] buffer = new byte[32 * 1024];
        while ((read = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await ns.WriteAsync(buffer, 0, read);
            totalWritten += read;
        }


        fileStream.Dispose();
        client.Close();
    }

    [Command]
    private void CmdRequestImage(NetworkIdentity requestId)
    {
        if (requestId.connectionToClient.address == "localhost")
        {
            return;
        }

        if (!IPAddress.TryParse(requestId.connectionToClient.address, out clientIp))
        {
            Debug.LogError("Error Collecting IP Address");
        }
        else
        {
            SendImage();
        }

    }

    public void RequestImage()
    {
        CmdRequestImage(GetComponent<NetworkIdentity>());
    }
}
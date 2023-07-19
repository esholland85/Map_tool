using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;
using hDandD;

public class TerrainGenerator : NetworkBehaviour
{
    // tutorial link: https://www.youtube.com/watch?v=64NblGkAabk

    [SerializeField] private UI userInterface;
    [SerializeField] private CharBar CharBar;
    public Mesh mesh;
    //private OnscreenDebug debugger;

    public Vector3[] vertices;
    public int[] triangles;
    public Color[] colors;
    public Vector2[] uvs;

    [SyncVar] public int meshWidth = 20;
    [SyncVar] public int meshLength = 20;
    public SyncListFloat heightValues = new SyncListFloat();
    public Gradient gradient;

    float minTerrainHeight;
    float maxTerrainHeight;

    public NodeController nodePrefab;
    public FlexibleMesh overlayPrefab;
    FlexibleMesh newMesh;

    string mapPath;
    [SyncVar] string syncedMapName = GameController.imageName;
    [SyncVar] string syncedFileExt = GameController.fileExt;
    private IPAddress myIp;
    private PlayerController player;

    private bool isOriginal = true;
    GameObject flipSide;

    private void OnValidate()
    {
        if (meshWidth < 1)
        {
            meshWidth = 1;
        }
        if (meshLength < 1)
        {
            meshLength = 1;
        }
    }

    private void Start()
    {
        GameController.map = this;
        GameController.currentNodes = new NodeController[(GameController.meshWidth + 1) * (GameController.meshLength + 1)];
        heightValues.Callback += UpdateClientTerrain;
    }

    private void UpdateClientTerrain(SyncList<float>.Operation op, int itemIndex, float oldItem, float newItem)
    {
        if (!isServer)
        {
            switch (op)
            {
                case SyncListFloat.Operation.OP_ADD:
                    if (itemIndex == ((meshWidth + 1) * (meshLength + 1) - 1))
                    {
                        updateHeights();
                        UpdateMesh();
                    }
                    break;
                case SyncListFloat.Operation.OP_CLEAR:
                    // list got cleared
                    break;
                case SyncListFloat.Operation.OP_INSERT:
                    // index is where it got added in the list
                    // item is the new item
                    break;
                case SyncListFloat.Operation.OP_REMOVEAT:
                    // index is where it got removed in the list
                    // item is the item that was removed
                    break;
                case SyncListFloat.Operation.OP_SET:
                    // index is the index of the item that was updated
                    // item is the previous item
                    break;
            }
        }

        //throw new NotImplementedException();
    }

    // Start is called before the first frame update
    public void playerStarted()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //debugger = FindObjectOfType<OnscreenDebug>();
        mapPath = GameController.imageFolderPath;
        player = FindObjectOfType<PlayerController>();

        if (!Directory.Exists(mapPath))
        {
            Directory.CreateDirectory(mapPath);
        }
        if (syncedMapName != null && !File.Exists(mapPath + syncedMapName + syncedFileExt))
        {
            WriteImage();
            player.RequestImage();
        }
        else
        {
            GenerateMesh();
        }
    }

    private void GenerateMesh()
    {
        if (isServer)
        {
            if ((GameController.meshWidth + 1) * (GameController.meshLength + 1) < 65535)
            {
                meshWidth = GameController.meshWidth;
                meshLength = GameController.meshLength;
            }
            else if (GameController.meshWidth > GameController.meshLength)
            {
                meshWidth = 255;
                meshLength = 255 * GameController.meshLength / GameController.meshWidth;
            }
            else
            {
                meshWidth = 255 * GameController.meshWidth / GameController.meshLength;
                meshLength = 255;
            }

            if (GameController.loadPath != null)
            {
                Material mat = GetComponent<MeshRenderer>().material;
                mat.SetTexture("_BaseColorMap", Load2d(GameController.loadPath));
            }
            else if (GameController.imageName != null)
            {
                Material mat = GetComponent<MeshRenderer>().material;
                mat.SetTexture("_BaseColorMap", Load2d(mapPath + syncedMapName + syncedFileExt));
            }
            CreateShape();
            if (!GameController.newMap)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = new Vector3(vertices[i].x, GameController.yValues[i], vertices[i].z);
                }
                colors = GameController.colorMap;
                CharBar.Repopulate();
            }
            updateHeights();
            UpdateMesh();
            if (GetComponent<MeshRenderer>().material.GetTexture("_BaseColorMap") != null)
            {
                GameController.imageName = GetComponent<MeshRenderer>().material.GetTexture("_BaseColorMap").name;
                syncedMapName = GameController.imageName;
                //this can probably move up into the previous if. Check when doing multiplayer again.
            }
            /*else
            {
                //material.mainTexture and other references to _MainTex need to be replaced by _BaseColorMap for HDRP shaders (at least the HDRP/Lit)
                MapData data = SaveSystem.LoadMap(GameController.imageName);
                meshWidth = data.meshWidth;
                meshLength = data.meshLength;

                if (data.imageName != null)
                {
                    Material mat = GetComponent<MeshRenderer>().material;
                    mat.SetTexture("_BaseColorMap", Load2d(mapPath + syncedMapName + ".jpg"));
                }

                CreateShape();

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = new Vector3(vertices[i].x, data.height[i], vertices[i].z);
                }
                updateHeights();
                UpdateMesh();
            }*/
        }
        else
        {
            Material mat = GetComponent<MeshRenderer>().material;
            mat.SetTexture("_BaseColorMap", Load2d(mapPath + syncedMapName + syncedFileExt));
            CreateShape();
            updateHeights();
            UpdateMesh();
        }
    }

    public void CreateShape()
    {
        vertices = new Vector3[(meshWidth + 1) * (meshLength + 1)];

        for (int i = 0, z = 0; z <= meshLength; z++)
        {
            for (float x = 0; x <= meshWidth; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        //vertices = new Vector3[4] {new Vector3(0f,0f,0f), new Vector3(0f, 0f, 1f), new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, 1f)};
        int vert = 0;
        int tris = 0;
        triangles = new int[meshWidth * meshLength * 6];
        for (int z = 0; z < meshLength; z++)
        {
            for (int x = 0; x < meshWidth; x++)
            {
                triangles[0 + tris] = vert + 0;
                triangles[1 + tris] = vert + meshWidth + 1;
                triangles[2 + tris] = vert + 1;
                triangles[3 + tris] = vert + 1;
                triangles[4 + tris] = vert + meshWidth + 1;
                triangles[5 + tris] = vert + meshWidth + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= meshLength; z++)
        {
            for (float x = 0; x <= meshWidth; x++)
            {
                uvs[i] = new Vector2((float)x / meshWidth, (float)z / meshLength);
                i++;
            }
        }

        if (colors == null || colors.Length != vertices.Length)
        {
            colors = new Color[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                colors[i] = Color.white;
            }
        }
    }

    public void UpdateMesh()
    {
        if (flipSide != null)
        {
            Destroy(flipSide);
        }
        //Activate in MapData too
        if (syncedMapName == null)
        {
            colors = new Color[vertices.Length];
            for (int c = 0, z = 0; z <= meshLength; z++)
            {
                for (int x = 0; x <= meshWidth; x++)
                {
                    float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[c].y);
                    colors[c] = gradient.Evaluate(height);
                    c++;
                }
            }
        }
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;

        //Material mat = GetComponent<Material>();
        //mat.

        //updates mesh collider to match mesh renderer
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        mesh.RecalculateNormals();

        //may be possible to duplicate the mesh and flip the normals, but so far I'm having troubles from the network ID, even on the host
        /*flipSide = Instantiate(this.gameObject, transform.position, Quaternion.identity);
        Mesh flipMesh = flipSide.mesh;

        Vector3[] normals = flipMesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        flipMesh.normals = normals;

        for (int m = 0; m < flipMesh.subMeshCount; m++)
        {
            int[] triangles = flipMesh.GetTriangles(m);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i + 0];
                triangles[i + 0] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            flipMesh.SetTriangles(triangles, m);
        }*/
    }


    /*See vertices
    wprivate void OnDrawGizmos()
    {

        if (vertices == null)
        {
            return;
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
    */

    //make 5 nodes at the nearest point to mouse click on the terrain
    private void OnMouseDown()
    {
        //this "if" makes sure there's not a UI field in the way before reading a click on the terrain

        if (GameController.currentMode == MapMode.terrainMode && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                int primaryIndex = (Mathf.RoundToInt(hit.point.z) - (int)transform.position.z) * (meshWidth + 1) + (Mathf.RoundToInt(hit.point.x) - (int)transform.position.x);
                int rectWidth = GameController.brushWidth;
                int rectLength = GameController.brushLength;
                int currentIndex = primaryIndex;

                switch (GameController.terrainBrush)
                {
                    case "details":
                        GenerateNode(primaryIndex);
                        GameController.userInterface.NodeSelected();
                        break;
                    case "rectDetailed":
                        for (int z = 0; z < rectLength; z++)
                        {
                            for (int x = 0; x < rectWidth; x++)
                            {
                                if (currentIndex < vertices.Length && vertices[currentIndex].z == vertices[primaryIndex].z)
                                {
                                    GenerateNode(currentIndex);
                                    currentIndex++;
                                }
                            }
                            if (primaryIndex + meshWidth + 1 < vertices.Length)
                            {
                                primaryIndex += meshWidth + 1;
                                currentIndex = primaryIndex;
                            }
                            else
                            {
                                return;
                            }
                        }
                        break;
                    case "rect":
                        for (int i = 0; i < GameController.currentNodes.Length; i++)
                        {
                            if (GameController.currentNodes[i] != null)
                            {
                                Destroy(GameController.currentNodes[i].gameObject);
                                GameController.currentNodes[i] = null;
                            }
                        }
                        NodeController bottomLeft = GenerateNode(primaryIndex);
                        GameController.activeNode = bottomLeft;
                        GameController.userInterface.NodeSelected();

                        int primaryColumn = primaryIndex % (meshWidth + 1);
                        int primaryRow = primaryIndex / (meshWidth + 1);
                        int secondaryColumn;
                        int secondaryRow;
                        if ((rectWidth + primaryColumn) > (meshWidth + 1))
                        {
                            GenerateNode((meshWidth + 1) * (primaryRow + 1) - 1);
                            secondaryColumn = meshWidth;
                        }
                        else
                        {
                            GenerateNode(rectWidth - 1 + primaryIndex);
                            secondaryColumn = primaryColumn + rectWidth - 1;
                        }
                        if ((meshLength + 1 - primaryRow) < rectLength)
                        {
                            GenerateNode(((meshWidth + 1) * meshLength) + primaryColumn);
                            secondaryRow = meshLength;
                        }
                        else
                        {
                            secondaryRow = primaryRow + rectLength - 1;
                            GenerateNode(secondaryRow * (meshWidth + 1) + primaryColumn);
                        }
                        NodeController upperRight = GenerateNode(secondaryRow * (meshWidth + 1) + secondaryColumn);
                        generateOverlay(bottomLeft, upperRight);
                        break;
                }
            }
        }
    }
    private NodeController GenerateNode(int newIndex)
    {
        if (GameController.currentNodes[newIndex] == null)
        {
            NodeController newNode = Instantiate(nodePrefab, vertices[newIndex] + transform.position, Quaternion.identity);
            newNode.GetComponent<NodeController>().index = newIndex;
            newNode.GetComponent<NodeController>().height = vertices[newIndex].y;
            return newNode;
        }
        else
        {
            return GameController.currentNodes[newIndex];
        }
    }
    private void generateOverlay(NodeController bottomLeft, NodeController upperRight)
    {
        if (newMesh != null)
        {
            Destroy(newMesh.gameObject);
        }
        int newWidth;
        int newLenth;
        List<float> newHeightValues = new List<float>();
        if (bottomLeft.index % (meshWidth + 1) == upperRight.index % (meshWidth + 1)) //same column
        {
            newWidth = 0;
            newLenth = upperRight.index / (meshWidth + 1) - bottomLeft.index / (meshWidth + 1);
            for (int i = bottomLeft.index; i <= upperRight.index; i += meshWidth + 1)
            {
                newHeightValues.Add(vertices[i].y);
            }
        }
        else if (bottomLeft.index / (meshWidth + 1) == upperRight.index / (meshWidth + 1)) //same row
        {
            newWidth = upperRight.index % (meshWidth + 1) - bottomLeft.index % (meshWidth + 1);
            newLenth = 0;
            for (int i = bottomLeft.index; i <= upperRight.index; i++)
            {
                newHeightValues.Add(vertices[i].y);
            }
        }
        else //rectangle
        {
            newWidth = upperRight.index % (meshWidth + 1) - bottomLeft.index % (meshWidth + 1);
            newLenth = upperRight.index / (meshWidth + 1) - bottomLeft.index / (meshWidth + 1);
            for (int i = bottomLeft.index; i <= upperRight.index; i += meshWidth + 1)
            {
                for (int x = 0; x <= upperRight.index % (meshWidth + 1) - bottomLeft.index % (meshWidth + 1); x++)
                {
                    newHeightValues.Add(vertices[i + x].y);
                }
            }
        }

        newMesh = Instantiate(overlayPrefab, new Vector3(bottomLeft.transform.position.x, bottomLeft.transform.position.y + .01f - bottomLeft.height, bottomLeft.transform.position.z), Quaternion.identity);
        // something weird happening when y value is negative
        newMesh.meshWidth = newWidth;
        newMesh.meshLength = newLenth;
        newMesh.heightValues = newHeightValues;
    }

    public void ChangeHeight()
    {
        int index = GameController.activeNode.index;
        //for some reason if it's not called both locally and in RPC, the node won't move, even though the mesh does.
        vertices[index] = new Vector3(vertices[index].x, GameController.activeNode.height, vertices[index].z);
        UpdateMesh();
        if (hasAuthority)
        {
            updateHeights();
        }

        GameController.activeNode.transform.SetPositionAndRotation(vertices[index] + transform.position, Quaternion.identity);
    }
    public void SetHeight(int bottomLeft, int upperRight, float newHeight)
    {
        List<int> activeVertices = new List<int>();
        if (bottomLeft % (meshWidth + 1) == upperRight % (meshWidth + 1)) //same column
        {
            for (int i = bottomLeft; i <= upperRight; i += meshWidth + 1)
            {
                activeVertices.Add(i);
            }
        }
        else if (bottomLeft/(meshWidth + 1) == upperRight/(meshWidth+1)) //same row
        {
            for (int i = bottomLeft; i <= upperRight; i++)
            {
                activeVertices.Add(i);
            }
        }
        else //rectangle
        {
            for (int i = bottomLeft; i <= upperRight; i += meshWidth + 1)
            {
                for (int x = 0; x <= upperRight % (meshWidth + 1) - bottomLeft % (meshWidth + 1); x++)
                {
                    activeVertices.Add(i + x);
                }
            }
        }

        foreach (int index in activeVertices)
        {
            vertices[index] = new Vector3(vertices[index].x, newHeight, vertices[index].z);
            if (newHeight < minTerrainHeight)
            {
                minTerrainHeight = newHeight;
            }
            if (newHeight > maxTerrainHeight)
            {
                maxTerrainHeight = newHeight;
            }
        }
        UpdateMesh();

        foreach (NodeController node in GameController.currentNodes)
        {
            if (node != null)
            {
                node.transform.SetPositionAndRotation(vertices[node.index] + transform.position, Quaternion.identity);
                node.height = vertices[node.index].y;
            }
        }

        generateOverlay(GameController.currentNodes[bottomLeft], GameController.currentNodes[upperRight]);

        if (hasAuthority)
        {
            updateHeights();
        }
    }
    public void AddHeight(int bottomLeft, int upperRight, float plusHeight)
    {
        List<int> activeVertices = new List<int>();
        if (bottomLeft % (meshWidth + 1) == upperRight % (meshWidth + 1)) //same column
        {
            for (int i = bottomLeft; i <= upperRight; i += meshWidth + 1)
            {
                activeVertices.Add(i);
            }
        }
        else if (bottomLeft / (meshWidth + 1) == upperRight / (meshWidth + 1)) //same row
        {
            for (int i = bottomLeft; i <= upperRight; i++)
            {
                activeVertices.Add(i);
            }
        }
        else //rectangle
        {
            for (int i = bottomLeft; i <= upperRight; i += meshWidth + 1)
            {
                for (int x = 0; x <= upperRight % (meshWidth + 1) - bottomLeft % (meshWidth + 1); x++)
                {
                    activeVertices.Add(i + x);
                }
            }
        }

        foreach (int index in activeVertices)
        {
            vertices[index] = new Vector3(vertices[index].x, vertices[index].y + plusHeight, vertices[index].z);
            float newHeight = vertices[index].y + plusHeight;
            if (newHeight < minTerrainHeight)
            {
                minTerrainHeight = newHeight;
            }
            if (newHeight > maxTerrainHeight)
            {
                maxTerrainHeight = newHeight;
            }
        }
        UpdateMesh();

        foreach (NodeController node in GameController.currentNodes)
        {
            if (node != null)
            {
                node.transform.SetPositionAndRotation(vertices[node.index] + transform.position, Quaternion.identity);
                node.height = vertices[node.index].y;
            }
        }

        generateOverlay(GameController.currentNodes[bottomLeft], GameController.currentNodes[upperRight]);

        if (hasAuthority)
        {
            updateHeights();
        }
    }

    public void ToggleTransparent(int bottomLeft, int upperRight)
    {
        List<int> activeVertices = new List<int>();
        if (bottomLeft % (meshWidth + 1) == upperRight % (meshWidth + 1)) //same column
        {
            for (int i = bottomLeft; i <= upperRight; i += meshWidth + 1)
            {
                activeVertices.Add(i);
            }
        }
        else if (bottomLeft / (meshWidth + 1) == upperRight / (meshWidth + 1)) //same row
        {
            for (int i = bottomLeft; i <= upperRight; i++)
            {
                activeVertices.Add(i);
            }
        }
        else //rectangle
        {
            for (int i = bottomLeft; i <= upperRight; i += meshWidth + 1)
            {
                for (int x = 0; x <= upperRight % (meshWidth + 1) - bottomLeft % (meshWidth + 1); x++)
                {
                    activeVertices.Add(i + x);
                }
            }
        }

        if (colors == null || colors.Length != vertices.Length)
        {
            colors = new Color[vertices.Length];
        }

        bool containsTrans = false;
        bool containsSolid = false;
        foreach (int index in activeVertices)
        {
            if (colors[index].a == 0)
            {
                containsTrans = true;
            }
            if (colors[index].a != 0)
            {
                containsSolid = true;
            }
            if (containsSolid && containsTrans)
            {
                break;
            }
        }

        foreach (int index in activeVertices)
        {
            if (containsTrans && containsSolid)
            {
                colors[index] = new Color(colors[index].r, colors[index].g, colors[index].b, 0);
            }
            else if (containsTrans)
            {
                colors[index] = new Color(colors[index].r, colors[index].g, colors[index].b, 1);
            }
            else
            {
                colors[index] = new Color(colors[index].r, colors[index].g, colors[index].b, 0);
            }
        }
        UpdateMesh();
    }

    private void updateHeights()
    {
        if (isServer)
        {
            heightValues.Clear();
            foreach (Vector3 vertex in vertices)
            {
                heightValues.Add(vertex.y);
            }
        }
        else
        {
            for (int i = 0; i < heightValues.Count; i++)
            {
                vertices[i] = new Vector3(vertices[i].x, heightValues[i], vertices[i].z);
            }
        }
    }

    protected async void WriteImage()
    {
        TcpListener listener = TcpListener.Create(GameController.imagePort);
        listener.Start();

        //debugger.Log("listening for server connection");
        TcpClient client = await listener.AcceptTcpClientAsync();
        //debugger.Log("server connected");
        NetworkStream ns = client.GetStream();

        long fileLength;
        string fileName;
        {
            byte[] fileNameBytes;
            byte[] fileNameLengthBytes = new byte[4];
            byte[] fileLengthBytes = new byte[8];
            await ns.ReadAsync(fileLengthBytes, 0, 8);
            await ns.ReadAsync(fileNameLengthBytes, 0, 4);
            fileNameBytes = new byte[BitConverter.ToInt32(fileNameLengthBytes, 0)];
            await ns.ReadAsync(fileNameBytes, 0, fileNameBytes.Length);

            fileLength = BitConverter.ToInt64(fileLengthBytes, 0);
            fileName = ASCIIEncoding.ASCII.GetString(fileNameBytes);
        }

        //Get Permission. I've attempted to set it to yes. Possible that it could be removed entirely for a completely internal file transfer, but I'm minimizing my changes
        ns.WriteByte(1); //0 is permission denied

        FileStream fileStream = File.Open(mapPath + syncedMapName + ".jpg", FileMode.Create);

        int read;
        int totalRead = 0;
        byte[] buffer = new byte[32 * 1024];
        while ((read = await ns.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await fileStream.WriteAsync(buffer, 0, read);
            totalRead += read;
        }
        fileStream.Dispose();
        client.Close();
        GenerateMesh();
    }

    private Texture2D Load2d(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (File.Exists(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            texture.name = GameController.imageName;
            return texture;
        }
        else
        {
            Debug.Log("file not found");
        }
        return null;
    }
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class PortalCamera : MonoBehaviour
{

    // Use this for initialization

    public Transform SelfTF;
    public Transform TargetTF;
    public Renderer TargetProjection;
    public RenderTexture TargetTexture0;
    public RenderTexture TargetTexture1;
    public Transform SelfBack;
    public Transform Player;
    public Transform PlayerRoot;
    public Transform MPlayer;
    public Transform SelfMPlayer;
    private Camera selfCamera;
    private Transform cameraTF;


    void Start()
    {
        Player = Camera.main.transform;
        cameraTF = transform;
        selfCamera = GetComponent<Camera>();
        selfCamera.clearFlags = CameraClearFlags.Skybox;
        TargetTexture0 = new RenderTexture(Screen.width, Screen.height, 24);
        TargetTexture1 = new RenderTexture(Screen.width, Screen.height, 24);

        lastposition = Player.position;
    }


    Vector3 lastposition;
    Vector3 normal;
    float t;
    static int TransformTime = 0;
    void Update()
    {

        selfCamera.targetTexture = Time.frameCount % 2 == 0 ? TargetTexture0 : TargetTexture1;
        TargetProjection.sharedMaterial.mainTexture = Time.frameCount % 2 == 1 ? TargetTexture0 : TargetTexture1;


        normal = Player.position - TargetTF.position;
        cameraTF.position = SelfBack.position + SelfBack.up * Vector3.Dot(normal, TargetTF.up) + SelfBack.right * Vector3.Dot(normal, TargetTF.right) + SelfBack.forward * Vector3.Dot(normal, TargetTF.forward);

        MPlayer.position = Player.position;
        MPlayer.rotation = Player.rotation;


        cameraTF.localRotation = MPlayer.localRotation;

        normal = Player.position - lastposition;

        if (Time.frameCount != TransformTime && normal != Vector3.zero && Vector3.Dot(normal, TargetTF.forward) > 0)
        {
            t = getT(TargetTF.position, lastposition, normal, TargetTF.forward);
            if (t > 0 && t < 1)
            {
                normal = lastposition + normal * t;
                //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //go.transform.position = normal;
                //go.transform.localScale = Vector3.one * 0.1f;

                normal = normal - TargetTF.position;

                if (Mathf.Abs(Vector3.Dot(normal, TargetTF.right)) < TargetTF.localScale.x * 0.5f)
                {
                    if (Mathf.Abs(Vector3.Dot(normal, TargetTF.up)) < TargetTF.localScale.y * 0.5f)
                    {
                        normal = TargetTF.InverseTransformPoint(PlayerRoot.position);
                        normal.z *= -1;
                        normal.x *= -1;
                        normal = SelfTF.TransformPoint(normal);
                        PlayerRoot.position = normal;
                        PlayerRoot.eulerAngles = Vector3.up * cameraTF.eulerAngles.y;
                        Player.rotation = cameraTF.rotation;
                        TransformTime = Time.frameCount;
                        //Debug.LogError("传送" + TargetTF.name + Time.frameCount);
                    }
                }
            }
        }



        lastposition = Player.position;
    }

    float getT(Vector3 center, Vector3 start, Vector3 dir, Vector3 normal)
    {
        Vector3 c = center - start;
        return Vector3.Dot(c, normal) / Vector3.Dot(dir, normal);
    }


}

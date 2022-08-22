using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public static MeshTrail instance;
    [Header("Mesh Related")]

    [Tooltip("This is how long the effect is applied for")]
    public float ActiveTime = 2f;
    [Tooltip("The gap between each 'ghost'")]
    public float MeshRefreshRate = 0.1f;
    [Tooltip("The orgin point of the 'ghosting' effect")]
    public Transform SpawnPOS;

    [Tooltip("How long a ghost stays before it is destoried")]
    public float lifetime = 0.3f;

    [Header("Shader Related")]

    [Tooltip("The material used on the spawned Mesh Renderers/'Ghosts'")]
    public Material GhostsMat;

    [Tooltip("What specific float being altered, the referance name can be found in the shader graph e.g for a fade effect _Alpha")]
    public string ShaderVarRef;

    [Tooltip("The rate of fade")]
    public float ShaderVarRate = 0.1f;
    [Tooltip("How often the shader is updated")]
    public float ShaderVarRefreshRate = 0.05f;

    private bool isTrailActive;

    [SerializeField, Header("Skinned Mesh Array")]
    [Tooltip("The current meshs being called for this 'ghosting' effect")]
    private SkinnedMeshRenderer[] SkinnedMeshRenders;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SkinnedMeshRenders = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void PlayerMeshTrail()
    {
        if(!isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActiveTrail(ActiveTime));
        }
    }

    IEnumerator ActiveTrail (float timeActivated)
    {
        while(timeActivated > 0)
        {
            timeActivated -= MeshRefreshRate;
            //this is were the trail effect is applied happens
            if(SkinnedMeshRenders == null)
            {
                SkinnedMeshRenders = GetComponentsInChildren<SkinnedMeshRenderer>();
            }
                
            
            for(int i=0; i<SkinnedMeshRenders.Length; i++)
            {
                GameObject GhostObj = new GameObject();
                GhostObj.transform.SetPositionAndRotation(SpawnPOS.position, SpawnPOS.rotation);

                MeshRenderer _MR = GhostObj.AddComponent<MeshRenderer>();
                MeshFilter _MF = GhostObj.AddComponent<MeshFilter>();

                Mesh _Mesh = new Mesh();

                SkinnedMeshRenders[i].BakeMesh(_Mesh);

                _MF.mesh = _Mesh;

                _MR.material = GhostsMat;

                StartCoroutine(AnimmatFloat(_MR.material, 0, ShaderVarRate, ShaderVarRefreshRate));

                Destroy(GhostObj, lifetime);
            }

            yield return new WaitForSeconds(MeshRefreshRate);
        }

        isTrailActive = false;
    }

    IEnumerator AnimmatFloat (Material _mat, float _goal, float _rate, float _refreshRate)
    {
        float valueToAnimate = _mat.GetFloat(ShaderVarRef);

        while(valueToAnimate > _goal)
        {
            valueToAnimate -= _rate;

            _mat.SetFloat(ShaderVarRef, valueToAnimate);

            yield return new WaitForSeconds(_refreshRate);
        }
    }

}

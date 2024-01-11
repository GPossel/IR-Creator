using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class PlayerMovementGeneral : MonoBehaviour, IBoost<int>, IDamagable<int>
{
    [SerializeField] GameManager2 myGameManager;
    [SerializeField] public PlayerMovementInfoBase PlayerMovementInfoBase;
    public Rigidbody playerRigidBody;
    private int doOnce = 0;

    [SerializeField] public bool isDamaged = false;

    [SerializeField] public bool[] isFullyRed;
    [SerializeField] public Material[] mySkinMaterials;
    [SerializeField] public Color[] fromColors;
    [SerializeField] public Color toColor;

    public void BindToPlayer(PlayerMovementInfoBase playerMovementScript)
    {
        PlayerMovementInfoBase = playerMovementScript;
    }

    private void Awake()
    {
        if (myGameManager == null)
            myGameManager = FindObjectOfType<GameManager2>();

        Assert.IsNotNull(myGameManager);

        playerRigidBody = GetComponent<Rigidbody>();
        Assert.IsNotNull(playerRigidBody);

        mySkinMaterials = transform.GetComponentInChildren<SkinnedMeshRenderer>().materials;
        Assert.IsNotNull(mySkinMaterials);

        if(fromColors == null)
            fromColors = new Color[mySkinMaterials.Length];

        if (isFullyRed == null)
            isFullyRed = new bool[mySkinMaterials.Length];

        for (int i = 0; i < fromColors.Length; i++)
        {
            fromColors[i] = mySkinMaterials[i].color;
            isFullyRed[i] = false;
        }

        toColor = Color.red;
        lastTimeHit = Time.time;
    }

    void Update()
    {
        if (PlayerMovementInfoBase.canFallOffPlane == true)
        {
            if (playerRigidBody.position.y < PlayerMovementInfoBase.FallOffPlaneYvalue)
            {
                if (doOnce == 0)
                {
                    myGameManager.ChangeState(GameManager2.GameState.EndRun);
                    this.enabled = false;
                    doOnce++;
                }
            }
        }

        if (isDamaged)
            SetRedGlowOnPlayerDamage();
        else
            mySkinMaterials.ToList().ForEach(x => x.color = fromColors[0]);
    }

    public void Booster(int amount, GameObject boostObject)
    {
        // TODO: start some coroutine making the player faster for 2 seconds... 
    }

    private int penalties = 0;
    private float lastTimeHit;
    private float currTimeHit;
    private float hitTimeout = 1f;

    public void Damage(int amount, GameObject attackerObject)
    {
        currTimeHit = Time.time;

        Debug.Log($"Diff in lastTimeHit and currTimeHit: {lastTimeHit - currTimeHit}");

        if ((currTimeHit - lastTimeHit) < hitTimeout) // 1:01 - 1:03 = 0.02 < 0.6... 
            return;
        else
        {
            DoDamage();
            lastTimeHit = currTimeHit;
        }
    }

    private void DoDamage()
    {
        StartCoroutine(SetDamaged());
        penalties++;

        if (penalties > 1)
        {
            if (doOnce == 0)
            {
                myGameManager.RaiseGameStateChanged(GameManager2.GameState.EndRun);
                this.enabled = false;
                doOnce++;
            }
        }
        else
        {
            StartCoroutine(RemovePenaltySlowly());
        }
    }

    public void SetRedGlowOnPlayerDamage()
    {
        for (int i = 0; i < mySkinMaterials.Length; i++)
        {
            if (!isFullyRed[i])
            {
                mySkinMaterials[i].color = Color.Lerp(mySkinMaterials[i].color, toColor, 0.08f);
                if (mySkinMaterials[i].color.r > 0.9f && mySkinMaterials[i].color.g < 0.3f && mySkinMaterials[i].color.b < 0.3f)
                    isFullyRed[i] = true;
            }

            if (isFullyRed[i])
            {
                mySkinMaterials[i].color = Color.Lerp(mySkinMaterials[i].color, fromColors[i], 0.08f);
                if (mySkinMaterials[i].color.g > 0.9f && mySkinMaterials[i].color.b > 0.9f) // leave red out, if the material is white, red will be 1f
                    isFullyRed[i] = false;
            }
        }
    }

    private IEnumerator RemovePenaltySlowly()
    {
        yield return new WaitForSecondsRealtime(5);
        penalties = 0;
        Debug.Log("Reset penalties to 0..");
    }

    private IEnumerator SetDamaged()
    {
        isDamaged = true;
        yield return new WaitForSecondsRealtime(5);
        isDamaged = false;
    }
}
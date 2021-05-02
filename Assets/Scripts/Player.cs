using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    private float currentHealth;

    public float GetHealthPct()
    {
        return (float)currentHealth / maxHealth;
    }

    public int kills;
    public int deaths;

    [SerializeField]
    private Behaviour[] disableOnDeath;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;

    private bool[] wasEnabledOnStart;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject spawnEffect;

    private bool firstSetup = true;

    public void Setup()
    {
        if(isLocalPlayer)
        {
            // Changement de caméra
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadcastNewPlayerSetup();
    }

    [Command(ignoreAuthority = true)]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if(firstSetup)
        {
            wasEnabledOnStart = new bool[disableOnDeath.Length];
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                wasEnabledOnStart[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        // Ré-active les scripts du joueur
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }

        // Ré-active les gameobjects du joueur
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        // Ré-active le collider du joueur
        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.enabled = true;
        }

        // Apparition du système de particules de mort
        GameObject _gfxIns = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        Setup();
    }

    private void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(25, "Joueur");
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(float amount, string sourceID)
    {
        if(isDead)
        {
            return;
        }

        currentHealth -= amount;
        Debug.Log(transform.name + " a maintenant : " + currentHealth + " points de vies.");

        if(currentHealth <= 0)
        {
            Die(sourceID);
        }
    }

    private void Die(string sourceID)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(sourceID);
        if(sourcePlayer != null)
        {
            sourcePlayer.kills++;
            GameManager.instance.onPlayerKilledCallback.Invoke(transform.name, sourcePlayer.name);
        }

        deaths++;

        // Désactive les components du joueur lors de la mort
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        // Désactive les gameobjects du joueur lors de la mort
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        // Désactive le collider du joueur
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Apparition du système de particules de mort
        GameObject _gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        // Changement de caméra
        if(isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " a été éliminé.");

        StartCoroutine(Respawn());
    }
}

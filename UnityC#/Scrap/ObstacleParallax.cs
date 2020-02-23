using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleParallax : MonoBehaviour
{
    /*#region Singleton
    public static ObstacleParallax instance;
    void Awake()
    {
        Configure();
        if (instance != null)
        {
            Debug.LogWarning("Multiple ObstacleParallax exist");
            return;
        }
        instance = this;
        
    }
    #endregion*/

    class PoolObject {
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t) { transform = t; }
        public void Use() { inUse = true; }
        public void Dispose() { inUse = false; }
    }

    //prefab as platform;
    public GameObject Prefab;
    //public Transform BreakableParent;
    public GameObject BreakablePrefab;
    //public Transform exitScreen;
    public int poolSize;
    public int breakablePoolSize;
    
    public float spawnRate;
    public float platformGap;

    public Vector3 defaultSpawnPos;
    public bool spawnImmediate;
    //public bool startingPlatform;
    //public bool SpawnMultiple;
    //public bool energy;
    public Vector2 targetAspectRatio;

    float shiftSpeed;
    float parallaxPosition;
    float bossTimer;
    float targetAspect;
    //int lastSlot;
    int platCount;
    bool boss;
    bool startBoss;
    bool bossKilled;

    PoolObject[] poolObjects;
    PoolObject[] breakableObjects;

    //int[,] runes;
    //int[] checkDuplicate;
    //public int[,] inputRunes;
    //int currentRow;
    //bool boost;
    bool platformEnded;
    //bool spawnBreakable;

    //float spawnPos;
    //float spawnEndPos;
    
    
    GameManager manager;
    //Rune runeGenrator;

    //GameObject[] blocks;
    //GameObject go;

    private void Awake()
    {
        Configure();
        //breakableObjects = new PoolObject[9];
        //if (BreakablePrefab != null)
        //ConfigureBreakable();
    }

    void Start()
    {
        manager = GameManager.instance;
        //runes = new int[poolSize, 2];
        //inputRunes = new int[poolSize, 2];
        //currentRow = 0;
        //lastSlot = 0;
        platCount = 1;
        boss = false;
        platformEnded = false;
        //spawnBreakable = false;
        bossTimer = 0;
    }

    void FixedUpdate()
    {
        Shift();
    }
    void Update()
    {
        
        shiftSpeed = manager.GetSpeed();
        
        //if (BreakablePrefab != null)
        //ShiftBreakable();

        /*
        if (energy && spawnTimer > spawnRate && spawnBreakable)
        {
            //Debug.Log("spawn energy");
            Spawn();
        }
        */
        if(platCount == 5)
        {
            boss = true;
        }
        if (startBoss) { bossTimer += Time.deltaTime; }

        if(bossTimer > 20 || bossKilled)
        {
            BossEnded();
        }

        if (platformEnded)
        {
            Spawn();
            //Configure();
        }

        /*
        if (spawnBreakable && spawnTimer > spawnRate)
        {
            //if (BreakablePrefab != null)
            //BreakableSpawner();
            spawnTimer = 0;
        }
        */

        // Debug test Key
        if (Input.GetKeyDown(KeyCode.K)) {
            bossKilled = true;
        }
   
    }

    // configure transform position and 
    // instantiate prefab as game object
    void Configure()
    {
        //set screen ratio
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;

        // initiate array
        poolObjects = new PoolObject[poolSize];

        
        for (int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            //go.GetComponent<Obstacle>().SetIndex(index);

            // get Shift() requirements
            Transform t = go.transform;

            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
            poolObjects[i].transform.gameObject.SetActive(true);
            //t.GetComponent<Obstacle>().SetIndex(index);
        }

        if (spawnImmediate)
        {
            PoolObject p = GetPoolObject();
            if (p != null)
            {
                float x = p.transform.GetComponent<PlatformStatesSwitch>().GetSpriteLength() / p.transform.GetComponent<PlatformStatesSwitch>().GetSpriteCount();
                p.transform.position = new Vector3(x, -2, 0);
                p.transform.gameObject.SetActive(true);
            }
            p.transform.GetComponent<PlatformStatesSwitch>().StartingPlatform();
        }
    }

    /*
    public void PopulateBreakable(int i, GameObject breakable)
    {
        //Debug.Log(breakable.transform.Find("intact").GetComponent<SpriteRenderer>().sprite.name);
        GameObject go = Instantiate(breakable) as GameObject;
        //go.GetComponent<Obstacle>().SetIndex(index);

        // get Shift() requirements
        Transform t = go.transform;

        t.SetParent(transform);
        t.position = Vector3.one * 1000;
        breakableObjects[i] = new PoolObject(t);
        breakableObjects[i].SetLength();
    }
    */

    /*
    void ConfigureBreakable()
    {
        //set screen ratio
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;

        // initiate array
        breakableObjects = new PoolObject[breakablePoolSize];

        for (int i = 0; i < breakableObjects.Length; i++)
        {
            GameObject go = Instantiate(BreakablePrefab) as GameObject;
            //go.GetComponent<Obstacle>().SetIndex(index);

            // get Shift() requirements
            Transform t = go.transform;

            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            breakableObjects[i] = new PoolObject(t);
            //t.GetComponent<Obstacle>().SetIndex(index);
        }
    }
    */

    // Spawn platforms
    void Spawn()
    {

        // Platform Spawning
        //go.GetComponent<Obstacle>().SetIndex(index);
        //if (startingPlatform)
        //{
        //    PoolObject p = GetPoolObject();
        //    if (p != null)
        //    {
        //        float x = p.spriteLength / p.spriteCount;
        //        p.transform.position = new Vector3(x, 0, 0);
        //        p.transform.gameObject.SetActive(true);
        //    }
        //    startingPlatform = false;

        //}
        if (bossKilled)
        {
            bossKilled = false;
            platCount = 0;
        }
        
        platformEnded = false;

        // get list of unuse platforms
        List<PoolObject> p = GetPoolObjects();

        // only run when there's an unused platform 
        if(p.Count > 0)
        {
            // pick one of them to spawn
            int platformIndex = Random.Range(0, p.Count);
            //Debug.Log(platformIndex);

            // only increment when it isn't boss time
            if (platCount < 5) {
                //p[platformIndex].transform.GetComponent<PlatformStatesSwitch>().DisableBossStage();
                platCount++;
            }
            else
            {
                platCount = 6;
                p[platformIndex].transform.GetComponent<PlatformStatesSwitch>().SetBossStage();
                startBoss = true;
            }
            

            Vector3 pos = new Vector3(0, -2, 0);
            pos.x = p[platformIndex].transform.GetComponent<PlatformStatesSwitch>().GetSpriteLength() + parallaxPosition;
            p[platformIndex].transform.position = pos;
            p[platformIndex].transform.gameObject.SetActive(true);
        }
        
        
        

        /* ----- Not useful in current build -----
        if (SpawnMultiple)
        {
            List<PoolObject> p = GetPoolObjects();
            //Debug.Log(t.Count);
            
            for (int i = 0; i < p.Count; i++)
            {
                platCount++;
                // Get character current position in Y;
                Vector3 pos = manager.GetPlayerPlatformPos();
                //Debug.Log("plaform: " + i);
                if (p[i] == null) return;

                // overwrite character position in X (outside screen bound)
                pos.x = p[i].transform.GetComponent<PlatformStatesSwitch>().GetSpriteLength() / p[i].transform.GetComponent<PlatformStatesSwitch>().GetSpriteCount() * 3f;
                pos.z = 0;

                //pos.x = defaultSpawnPos.x;
                if (i == 0)
                {
                    //Debug.Log(p[i].transform.GetComponent<PlatformStatesSwitch>().GetSpriteLength());
                    pos.x = p[i].transform.GetComponent<PlatformStatesSwitch>().GetSpriteLength() + parallaxPosition;
                }
                //else if(i == 1) { pos.y += -platformGap; }
                //else { pos.y += platformGap; }

                //Debug.Log(p[i].transform.Find("sprite_parent").GetComponentsInChildren<SpriteRenderer>().Length);

                p[i].transform.position = pos;
                p[i].transform.gameObject.SetActive(true);
            }
            
        }
        */
        /*
        if (OnPlatform)
        {
            List<PoolObject> t = GetPoolObjects();
            //Debug.Log(t.Count);
            Vector3 pos = manager.GetPlayerPlatformPos();
            pos.y += defaultSpawnPos.y;

            for (int i = 0; i < t.Count; i++)
            {
                //Debug.Log("plaform: " + i);
                if (t[i] == null) return;
                pos.x = defaultSpawnPos.x;
                if(i == 1) { pos.x += 30; }
                pos.z = 0;

                t[i].transform.position = pos;
                t[i].transform.gameObject.SetActive(true);
            }
        }
        */

        // for energy spawn
        /*
        else
        {
            PoolObject p = GetPoolObject();
            if (p == null) return;
            Vector3 pos = manager.GetPlayerPlatformPos();
            pos.x = defaultSpawnPos.x;
            pos.y += defaultSpawnPos.y;
            p.transform.position = pos;
            p.transform.gameObject.SetActive(true);
        }
        */
    }

    /*
    void BreakableSpawner()
    {
        // Spawn multiple at once
        //List<PoolObject> p = GetBreakablePoolObjects();

        // Now is spawning 1 per call
        PoolObject p = GetBreakablePoolObject();
        for (int i = 0; i < 1; i++)
        {
            //Debug.Log(p[i].spriteLength);
            //int e = Random.Range(0, p.Count - 1);
            Vector3 pos = new Vector3();
            int slot = 0;
            if (lastSlot == 0 || lastSlot == 1) { slot = Random.Range(0, 3); }
            else { slot = Random.Range(0, 2); }
             
            //pos.y += 1;
            //Debug.Log("plaform: " + i);
            if (p == null) return;
            pos.x = defaultSpawnPos.x + i * p.transform.GetComponent<PlatformStatesSwitch>().GetSpriteLength() * 5;
            if (slot == 0) { pos.y = 0; }
            else if (slot == 1){ pos.y += 5; }
            else { pos.y += 10; }
            //if (i == 1) { pos.x += 30; }
            pos.z = i / 10;

            p.transform.position = pos;
            p.transform.gameObject.SetActive(true);
            lastSlot = slot;
        }
    }
    */

    void Shift()
    {
        //float speed = (float)manager.get_team_speed() / 1000;
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if(poolObjects[i].inUse)
            //Debug.Log(poolObjects[i].);
                poolObjects[i].transform.Translate(Vector2.left * shiftSpeed/100);
            CheckDisposeObject(poolObjects[i]);
            // Debug.Log(poolObjects[i].spriteLength);
        }
        
    }

    void BossEnded()
    {
        platCount = 0;
        bossTimer = 0;
        boss = false;
        startBoss = false;
        //bossKilled = false;
    }

    /*
    void ShiftBreakable()
    {
        float speed = (float)manager.GetSpeed() / 1000;
        for (int i = 0; i < breakableObjects.Length; i++)
        {
            //Debug.Log(poolObjects[i].);
            breakableObjects[i].transform.Translate(Vector2.left * shiftSpeed * speed);
            CheckDisposeObject(breakableObjects[i]);
        }
    }
    */

    void CheckDisposeObject(PoolObject poolObject) {


        // randomize energy generation on scren
        if (poolObject.transform.name.Contains("energy"))
        {
            if (poolObject.transform.position.x < -defaultSpawnPos.x)
            {
                poolObject.Dispose();
                poolObject.transform.GetComponent<RunePickup>().Refresh();
                //poolObject.transform.position = new Vector2(1000, 0);
            }
        }
            
        /*
            // Reset breakable states to intact
        if (poolObject.transform.name.Contains("Breakable"))
        {
            if (poolObject.transform.position.x < -defaultSpawnPos.x * 1.2)
            {
                poolObject.Dispose();
                poolObject.transform.GetComponent<Breakable>().ResetStates();
                poolObject.transform.position = new Vector2(1000, 0);
              
            }
            if(!spawnBreakable && !poolObject.transform.GetComponentInChildren<SpriteRenderer>().isVisible)
            {
                //poolObject.transform.gameObject.SetActive(false);
            }
            //Debug.Log("dispose breakable");
            //poolObject.transform.GetComponent<Breakable>().ResetStates();
        }
        */
            // move object out of screen
            //poolObject.transform.position = new Vector2(1000, 0);

        // Recycle unused platform if player didn't choose it
        if (poolObject.transform.name.Contains("platform"))
        {
            if (poolObject.transform.position.x < -poolObject.transform.GetComponent<PlatformStatesSwitch>().GetSpriteLength() / 2f - 25) {
                
                poolObject.Dispose();
                poolObject.transform.position = new Vector2(1000, 0);
                poolObject.transform.GetComponent<PlatformStatesSwitch>().ResetStates();
            }
            //else if (poolObject.transform.position.x < 0 && poolObject.transform.GetComponent<PlatformStatesSwitch>().PlayerOnPlatform() == false) { poolObject.Dispose(); }
            //if(poolObject.transform.GetComponent<PlatformStatesSwitch>().PlayerOnPlatform() == true) { spawnBreakable = true; }
            //Debug.Log("Reset States " + poolObject.inUse);
        }
    }

    PoolObject GetPoolObject()
    {
        //List<Transform> spawnList = new List<Transform>();
        for(int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse) {
                //spawnList.Add(poolObjects[i].transform);
                poolObjects[i].Use();
                return poolObjects[i];
                //spawnList.Add(poolObjects[i].transform);
            }
            //Debug.Log(spawnList.Count);
            
            //Debug.Log(poolObjects[i].inUse);
        }
        return null;
    }

    List<PoolObject> GetPoolObjects()
    {
        List<PoolObject> spawnList = new List<PoolObject>();
        //Debug.Log("Pool Obects: " + poolObjects.Length);
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                spawnList.Add(poolObjects[i]);
                poolObjects[i].Use();                
                //spawnList.Add(poolObjects[i].transform);
            }
            else { parallaxPosition = poolObjects[i].transform.position.x - 0.2f; }
            //Debug.Log(poolObjects[i].inUse);
        }
        return spawnList;
    }

    /*
    List<PoolObject> GetBreakablePoolObjects()
    {
        List<PoolObject> spawnList = new List<PoolObject>();
        //Debug.Log("Pool Obects: " + poolObjects.Length);
        for (int i = 0; i < breakableObjects.Length; i++)
        {
            if (!breakableObjects[i].inUse)
            {
                spawnList.Add(breakableObjects[i]);
                breakableObjects[i].Use();
                //spawnList.Add(poolObjects[i].transform);
            }
            //Debug.Log(poolObjects[i].inUse);
        }
        return spawnList;
    }

    PoolObject GetBreakablePoolObject()
    {
        //List<PoolObject> spawnList = new List<PoolObject>();
        //Debug.Log("Pool Obects: " + poolObjects.Length);
        for (int i = 0; i < breakableObjects.Length; i++)
        {
            if (!breakableObjects[i].inUse)
            {
                breakableObjects[i].Use();
                return breakableObjects[i];
                //spawnList.Add(poolObjects[i].transform);
            }
            //Debug.Log(poolObjects[i].inUse);
        }
        return null;
    }
    */

    public void Restart()
    {
        /*
        for(int i = 0; i < breakableObjects.Length; i++)
        {
            breakableObjects[i].transform.position = new Vector2(1000, 0);
            breakableObjects[i].transform.GetComponent<Breakable>().ResetStates();
            breakableObjects[i].Dispose();
        }
        */
    }

    public int GetPlatformPassed() { return platCount; }

    public void SetPlatformEnded(bool status) { platformEnded = status; }
    //public void EnableBreakableSpawner(bool status) { spawnBreakable = status; }
}


// ******************* //
// END OF USEFUL STUFF //
// ******************* //


/* 
 * Archived Stuff 
 */

// allow button to change rune elemental
/*
public void Earth() { go.GetComponent<Obstacle>().SetRow(0); }
public void Water() { go.GetComponent<Obstacle>().SetRow(1); }
public void Fire() { go.GetComponent<Obstacle>().SetRow(2); }
*/
/*
public void ActivateBoost()
{
    manager.GetBoost();
}

public void AcceptRuneInput(int[] input)
{
    //input is the current control UI input index

    for(int i = 0; i < input.Length; i++)
    {
        inputRunes[currentRow, i] = input[i];
    }
    if (inputRunes[0, 0] == 1 && inputRunes[1, 0] == 3) { ActivateBoost(); }
    // set rune on obstacle

    if (currentRow < poolSize)
    {
        //poolObjects[currentRow].transform.GetComponent<SpellCast>().RecordRune(input);
        currentRow++;
        if(currentRow == 2) { ResetIndex(); currentRow = 0; }

    } 
}

void ResetIndex()
{
    if (currentRow == 2)
    {
        for (int i = 0; i < 2; i++)
        {
            // reset input runes
            inputRunes[0, i] = -1;
            inputRunes[1, i] = -1;
        }
    }
}

// Rune Index for checking match
void UpdateRunesIndex() 
{
    int[] temp;
    for(int i = 0; i < poolObjects.Length; i++)
    {
        temp = poolObjects[i].transform.GetComponent<SpellCast>().CheckRune();
        for (int j = 0; j < 2; j++)
        {
            runes[i, j] = temp[j];
        }
    }
}
*/
/*
void CheckActivateRune()
{
    int match = 0;
    for(int i = 0; i < poolObjects.Length; i++)
    {
        for(int j = 0; j < inputRunes.Length; j++)
        {
            if(inputRunes[j] == runes[i, j]) { match++; }         
        }
        if (match == 2)
        {
            poolObjects[i].transform.GetComponent<Obstacle>().RuneActivation();
        }
        //else { poolObjects[i].transform.GetComponent<Obstacle>().RuneDeactivation(); }
        //Debug.Log("-----total: " + match + " matched.");
        match = 0;
    }
}
*/

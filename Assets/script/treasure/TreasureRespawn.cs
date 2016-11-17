using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class TreasureRespawn : NetworkBehaviour {

    class position_rotation
    {
        public position_rotation(Vector3 position, Quaternion rotation, int number ) { this.position = position; this.rotation = rotation; this.number = number; }
        public Vector3 position;
        public Quaternion rotation;
        public int number;
    }



    private List<GameObject> treasureCases = new List<GameObject>();
    private List<position_rotation> caseTransform = new List<position_rotation>() ; 
    public GameObject treasureCasePrefab;

    bulletCenter bulCenter;
    BulletType[] allBullet;
    List<int> spawnableCaseIndex = new List<int>();
    List<int> caseReadyToRespawn = new List<int>() ;



    private float timer = 0;
    private float respawnTimeOffset = 3f ;


	// Use this for initialization
    [ServerCallback]
	void Start () {
        
        bulCenter = GameObject.Find("BulletCenter").GetComponent<bulletCenter>();
        allBullet = bulCenter.GetAllBullet();
        

	    // search all treasure case in child
        for (int  i = 0; i < transform.childCount; i++ )
        {
            if (transform.GetChild(i).tag == "TreasureCase"  )
            {
                this.treasureCases.Add(transform.GetChild(i).gameObject);
                position_rotation pr = new position_rotation(treasureCases[i].transform.position, treasureCases[i].transform.rotation,i);
                this.caseTransform.Add(pr);
            }
        }

        foreach (GameObject go in treasureCases)
            NetworkServer.Destroy(go);


        spawnAllCase();
	}
	
	// Update is called once per frame
	void Update () {
	    if ( timer >= respawnTimeOffset  )
        {
            if (caseReadyToRespawn.Count > 0)
            {
                int index = Random.Range(0, caseReadyToRespawn.Count-1);

                respawnACase(caseReadyToRespawn[index]);
                caseReadyToRespawn.RemoveAt(index);
                timer = 0;

            }

        }

        timer += Time.deltaTime;
            
	}

    


    void respawnACase( int index )
    {
        GameObject go = GameObject.Instantiate(treasureCasePrefab);
        go.transform.position = caseTransform[index].position;
        go.transform.rotation = caseTransform[index].rotation;

        treasureCase tCase = go.GetComponent<treasureCase>();
        tCase.setNumber(caseTransform[index].number);
        RandomItem(tCase);
        tCase.responder += CaseRespond;
        NetworkServer.Spawn(go);
    }


    void RandomItem( treasureCase tCase )
    {
        tCase.setCaseContend(allBullet[Random.Range(0, allBullet.Length - 1)]);
    }



    public void CaseRespond( int number )
    {

        Debug.Log("case number :" + number.ToString() + " opened.");
        StartCoroutine(caseReadyDelay(number));

    }

    IEnumerator caseReadyDelay(int number)
    {
        yield return new WaitForSeconds(5);
        if (!caseReadyToRespawn.Contains( number ))
        {
            caseReadyToRespawn.Add(number);
        }

    }



    void spawnAllCase()
    {
        for( int i = 0; i < caseTransform.Count; i++ )
        {
            
            GameObject go = GameObject.Instantiate(treasureCasePrefab);
            NetworkServer.Spawn(go);
            go.transform.parent = this.transform;
            go.transform.position = caseTransform[i].position;
            go.transform.rotation = caseTransform[i].rotation;
            treasureCase tCase = go.GetComponent<treasureCase>();
            if ( tCase)
            {
                RandomItem(tCase);
                tCase.setNumber(caseTransform[i].number);
                tCase.responder += CaseRespond;
            }

        }
    }


}

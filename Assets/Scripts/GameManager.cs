using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject PlayerPrefab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public Text PingText;
    public GameObject disconnectUI;
    private bool Off = false;

    //for spawn player
    public Transform SpawnPosition;
    public float PositionOffset = 2.0f;
    public GameObject[] PrefabsToInstantiate;

    public GameObject PlayerFeed;
    public GameObject FeedGrid;
    /*    public string playerPrefabName = "Player";*/
    /*    public Transform spawnPoint;*/

    //spawn enemy
    public Transform EnemySpawnPosition;
    public float EnemyPositionOffset = 2.0f;
    public GameObject[] EnemyToInstantiate;

    private void Awake()
    {
        GameCanvas.SetActive(true);
    }
    private void Update()
    {
        CheckInput();
        PingText.text = "Ping: " + PhotonNetwork.GetPing();       
    }

    private void CheckInput()
    {
        if(Off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUI.SetActive(false);
            Off = false;
        }
        else if(!Off && Input.GetKeyDown(KeyCode.Escape))
        {
            disconnectUI.SetActive(true);
            Off = true;
        }
    }
    public void SpawnPlayer()
    {
/*        float randomValue = Random.Range(-8f, -9f);*/
/*        Debug.Log("spawning");
        Debug.Log(PlayerPrefab.name);
        Debug.Log("Instantiating: " + PlayerPrefab.name);*/

        Vector3 spawnPos = Vector3.up;
        if (this.SpawnPosition != null)
        {
            spawnPos = this.SpawnPosition.position;
        }
        if (this.PrefabsToInstantiate != null)
        {
            foreach (GameObject o in this.PrefabsToInstantiate)
            {
                Debug.Log("Instantiating: " + o.name);

                Vector3 spawnPosit = Vector3.up;
                if (this.SpawnPosition != null)
                {
                    spawnPosit = this.SpawnPosition.position;
                }

                Vector3 randomPos = Random.insideUnitSphere;
                randomPos.y = 0;
                randomPos = randomPos.normalized;
                Vector3 itemposition = spawnPosit + this.PositionOffset * randomPos;

                PhotonNetwork.Instantiate(o.name, itemposition, Quaternion.identity, 0);
                //instance player here
            }
        }

        Vector3 random = Random.insideUnitSphere;
        random.y = 0;
        random = random.normalized;
        Vector3 itempos = spawnPos + this.PositionOffset * random;
        /*PhotonNetwork.Instantiate(PlayerPrefab.name, itempos, Quaternion.identity, 0);*/
        //tại saoooo tôi lại instance 2 lần để rồi mất 3 tiếng để fix và tìm hiểu lý do vì sao có 2 con player được spawn : )
        /*        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);*/
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
    }

    public void SpawnEnemy()
    {
        System.Threading.Thread.Sleep(2000);
        if (this.EnemyToInstantiate != null)
        {
            foreach (GameObject o in this.EnemyToInstantiate)
            {

                Vector3 spawnPos = Vector3.up;
                if (this.EnemySpawnPosition != null)
                {
                    spawnPos = this.EnemySpawnPosition.position;
                }

                Vector3 random = Random.insideUnitSphere;
                random.y = 0;
                random = random.normalized;
                Vector3 itempos = spawnPos + this.EnemyPositionOffset * random;

                PhotonNetwork.Instantiate(o.name, itempos, Quaternion.identity, 0);
            }
        }
    }
    public void LeaveRoom()
    {
        Debug.Log("leaving room");
        StartCoroutine(SavePlayerData());
/*        PhotonNetwork.LeaveRoom();*/
/*        PhotonNetwork.LoadLevel("Open");*/
    }

    IEnumerator SavePlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("level", DBManager.level);
        form.AddField("coins", DBManager.coins);

        WWW www = new WWW("http://localhost/sqlconnect/savedata.php", form);
        yield return www;
        if (www.text == "0")
        {
            Debug.Log("Game Saved.");
        }
        else
        {
            Debug.Log("SAVE FAILED." + www.text);
        }
        PhotonNetwork.LeaveRoom();
        DBManager.LogOut();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(FeedGrid.transform, false);
        obj.GetComponent<Text>().text = player.name + "joined the game";
        obj.GetComponent<Text>().color = Color.green;
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameObject obj = Instantiate(PlayerFeed, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(FeedGrid.transform, false);
        obj.GetComponent<Text>().text = player.name + "left the game";
        obj.GetComponent<Text>().color = Color.red;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : Fuzzy
{
    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }


    public Chessman[,] Chessmans { set; get; }
    private Chessman selectedChessman;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    public bool isWhiteTurn = true;

    public GameObject timertext1, timertext2;
    private string whitetime, blacktime;
    private float secondCount;
    private int minuteCount;
    private float whiteMinute = 12,blackMinute = 0;
    private int hourCount;
    private bool stop = true;

    public GameObject movetext1, movetext2;
    private float move1 = 46;
    private float move2 = 0;

    public GameObject pointtext1, pointtext2;
    private float blackpoint = 0;
    private float whitepoint = 26;

    public GameObject whiteTurn, blackTurn;

    public GameObject winBanner;
    public GameObject spawnpoint;
    public GameObject winText;

    private void Start()
    {
        Instance = this;
        SpawnAllCessmans();
    }

    private void Update()
    {
        UpdateSelection();
        DrawChessboard();

        if (isWhiteTurn)
        {
            if (stop)
            {
                updatescorewhite();
            }
        }
        else
        {
            if (stop)
            {
                updatescoreblack();
            }
        }
    
        if (Input.GetMouseButtonDown(0))
        {
            if(selectionX >=0 && selectionY >= 0)
            {
                if(selectedChessman == null)
                {                  
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
    }

    private void SelectChessman(int x, int y)
    {
        if(Chessmans[x,y] == null)
        {
            return;
        }
        if(Chessmans[x,y].isWhite != isWhiteTurn)
        {
            return;
        }

        bool hasAtleastOneMove = false;
        allowedMoves = Chessmans[x, y].PossibleMove();
        for(int i=0; i < 8; i++)
        {
            for(int j=0; j<8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtleastOneMove = true;
                }
            }
        }
        if (!hasAtleastOneMove)
        {
            return;
        }

        selectedChessman = Chessmans[x, y];
        //previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        //selectedMat.mainTexture = previousMat.mainTexture;
        //selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x,y])
        {
            Chessman c = Chessmans[x, y];
            
            if(c!=null && c.isWhite != isWhiteTurn)
            {
                if(c.GetType() == typeof(king))
                {
                    stop = false;
                    EndGame();
                    return;
                }

                if(c.GetType() == typeof(pawn))
                {
                    if (isWhiteTurn)
                    {
                        whitepoint = whitepoint + 1;
                    }
                    else
                    {
                        blackpoint = blackpoint + 1;
                    }
                }

                if (c.GetType() == typeof(rook))
                {
                    if (isWhiteTurn)
                    {
                        whitepoint = whitepoint + 3;
                    }
                    else
                    {
                        blackpoint = blackpoint + 3;
                    }
                }

                if (c.GetType() == typeof(horse))
                {
                    if (isWhiteTurn)
                    {
                        whitepoint = whitepoint + 3;
                    }
                    else
                    {
                        blackpoint = blackpoint + 3;
                    }
                }

                if (c.GetType() == typeof(bishop))
                {
                    if (isWhiteTurn)
                    {
                        whitepoint = whitepoint + 5;
                    }
                    else
                    {
                        blackpoint = blackpoint + 5;
                    }
                }

                if (c.GetType() == typeof(queen))
                {
                    if (isWhiteTurn)
                    {
                        whitepoint = whitepoint + 10;
                    }
                    else
                    {
                        blackpoint = blackpoint + 10;
                    }
                }

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            if (isWhiteTurn)
            {
                move1++;
            }
            else
            {
                move2++;
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;
        }

        BoardHighlights.Instance.Hidehighlights();
        selectedChessman = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Chessplane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x,y), Quaternion.identity) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x , y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);

    }

    private void SpawnAllCessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        //spawn putih
        //raja
        SpawnChessman(5, 3, 0);
        //ratu
        SpawnChessman(4, 4, 0);
        //mentri
        SpawnChessman(3, 2, 0);
        SpawnChessman(3, 5, 0);
        //kuda
        SpawnChessman(2, 1, 0);
        SpawnChessman(2, 6, 0);
        //benteng
        SpawnChessman(1, 0, 0);
        SpawnChessman(1, 7, 0);
        //prajurit
        for (int i=0 ; i < 8; i++)
        {
            SpawnChessman(0, i, 1);
        }

        //spawn hitam
        //raja
        SpawnChessman(6, 3, 7);
        //ratu
        SpawnChessman(7, 4, 7);
        //mentri
        SpawnChessman(8, 2, 7);
        SpawnChessman(8, 5, 7);
        //kuda
        SpawnChessman(9, 1, 7);
        SpawnChessman(9, 6, 7);
        //benteng
        SpawnChessman(10, 0, 7);
        SpawnChessman(10, 7, 7);
        //prajurit
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, i, 6);
        }
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for(int i=0; i<=8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for(int j=0; j<=8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            } 
        }

        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX, 
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
            Debug.DrawLine(
                Vector3.forward * (selectionY+1) + Vector3.right * selectionX,
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    public void updatescorewhite()
    {
        secondCount += Time.deltaTime;
        timertext1.GetComponent<Text>().text = "Timer : " + whiteMinute + "m : " + System.Math.Round(secondCount, 2) + "s";
        whitetime = "Timer : " + minuteCount + "m : " + System.Math.Round(secondCount, 2) + "s";
        movetext1.GetComponent<Text>().text = "Move : " + move1;
        pointtext1.GetComponent<Text>().text = "Point : " + whitepoint;
        whiteTurn.GetComponent<Text>().text = "PLAYER 1 TURN";
        blackTurn.GetComponent<Text>().text = "PLAYER 2";
        if (secondCount >= 60)
        {
            minuteCount++;
            whiteMinute = minuteCount;
            secondCount = 0;
        }
    }

    public void updatescoreblack()
    {
        secondCount += Time.deltaTime;
        timertext2.GetComponent<Text>().text = "Timer : " + blackMinute + "m : " + System.Math.Round(secondCount, 2) + "s";
        blacktime = "Timer : " + minuteCount + "m : " + System.Math.Round(secondCount, 2) + "s";
        movetext2.GetComponent<Text>().text = "Move : " + move2;
        pointtext2.GetComponent<Text>().text = "Point : " + blackpoint;
        whiteTurn.GetComponent<Text>().text = "PLAYER 1";
        blackTurn.GetComponent<Text>().text = "PLAYER 2 TURN";
        if (secondCount >= 60)
        {
            minuteCount++;
            blackMinute = minuteCount;
            secondCount = 0;
        }
    }

    private void EndGame()
    {
        if (isWhiteTurn)
        {
            anggotaTime(whiteMinute);
            anggotaMove(move1);
            anggotaPoint(whitepoint);
            whiteTurn.GetComponent<Text>().text = "PLAYER 1";
            GameObject win = GameObject.Instantiate(winBanner, spawnpoint.transform) as GameObject;
            winText.GetComponent<Text>().text = "===PLAYER 1 WIN===\n\n===FUZZIFIKASI===\n"+fuzzyresult()+"\n===INTERFERENSI===\n"+inferensi()+"\n===DEFUZZIFIKASI===\n"+defuzzifikasi();
            GameObject win1 = GameObject.Instantiate(winText, spawnpoint.transform) as GameObject;
        }
        else
        {
            anggotaTime(blackMinute);
            anggotaMove(move2);
            anggotaPoint(blackMinute);
            blackTurn.GetComponent<Text>().text = "PLAYER 2";
            GameObject win = GameObject.Instantiate(winBanner,spawnpoint.transform)as GameObject;
            winText.GetComponent<Text>().text = "===PLAYER 2 WIN===\n\n===FUZZIFIKASI===\n"+fuzzyresult()+"\n===INTERFERENSI===\n"+inferensi()+"\n===DEFUZZIFIKASI===\n"+defuzzifikasi();
            GameObject win1 = GameObject.Instantiate(winText, spawnpoint.transform) as GameObject;
        }
        foreach(GameObject go in activeChessman)
            Destroy(go);
        isWhiteTurn = true;
        BoardHighlights.Instance.Hidehighlights();
        SpawnAllCessmans();
    }

}

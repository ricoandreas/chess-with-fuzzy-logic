using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuzzy : MonoBehaviour
{
    private int i, j, k, l;

    //variabel fuzzyfikasi
    private float memberTimeCepat = 0;
    private float memberTimeRata = 0;
    private float memberTimeLambat = 0;

    private float memberMoveBanyak = 0;
    private float memberMoveSedang = 0;
    private float memberMoveSedikit = 0;

    private float memberPointBesar = 0;
    private float memberPointSedang = 0;
    private float memberPointKecil = 0;

    //variabel defuzzyfikasi
    private float pengali1;
    private float pengali2;
    private float pengali3;
    private float pembilang;
    private float penyebut;
    private float defuzzifikasiResult;
    private int node;
    private float X;
    private float Y;
    private float Z;
    private float XY;
    private float YZ;
    private float[] pengali4;
    private float[] node_sample;
    private int delta = 10;
    private int a, b;

    //variabel inferensi
    private float[] nilaiTime;
    private float[] nilaiMove;
    private float[] nilaiPoint;
    private float[] nilaiRank;
    private float terbesarA, terbesarB, terbesarC;
    private string[] rank;
    private string interferensiResult1, interferensiResult2, interferensiResult3, interferensiResultFix;

    protected void anggotaTime(float inputTime)
    {
        //Time cepat
        if (inputTime <= 10)
        {
            memberTimeCepat = 1;
        }
        else if (inputTime > 10 && inputTime < 20)
        {
            memberTimeCepat = (float)((20 - inputTime) / 10);
        }
        else
        {
            memberTimeCepat = 0;
        }

        //Time rata-rata
        if (inputTime >= 20 && inputTime <= 40)
        {
            memberTimeRata = 1;
        }
        else if ((inputTime <= 10) || (inputTime >= 60))
        {
            memberTimeRata = 0;
        }
        else if (inputTime > 10 && inputTime < 20)
        {
            memberTimeRata = (float)((inputTime - 10) / 10);
        }
        else
        {
            memberTimeRata = (float)((60 - inputTime) / 20);
        }

        //Time lambat
        if (inputTime >= 60)
        {
            memberTimeLambat = 1;
        }
        else if (inputTime <= 40)
        {
            memberTimeLambat = 0;
        }
        else
        {
            memberTimeLambat = (float)((inputTime - 40) / 20);
        }
    }

    protected void anggotaMove(float inputMove)
    {
        //Move sedikit
        if (inputMove <= 15)
        {
            memberMoveSedikit = 1;
        }
        else if (inputMove >= 25)
        {
            memberMoveSedikit = 0;
        }
        else
        {
            memberMoveSedikit = (float)((25 - inputMove) / 10);
        }

        //Move sedang
        if (inputMove >= 25 && inputMove <= 45)
        {
            memberMoveSedang = 1;
        }
        else if (inputMove <= 15 || inputMove >= 50)
        {
            memberMoveSedang = 0;
        }
        else if (inputMove > 15 && inputMove < 25)
        {
            memberMoveSedang = (float)((inputMove - 15) / 10);
        }
        else
        {
            memberMoveSedang = (float)((50 - inputMove) / 5);
        }

        //Move banyak
        if (inputMove >= 50)
        {
            memberMoveBanyak = 1;
        }
        else if (inputMove <= 45)
        {
            memberMoveBanyak = 0;
        }
        else
        {
            memberMoveBanyak = (float)((inputMove - 45) / 5);
        }

    }

    protected void anggotaPoint(float inputPoint)
    {
        //Point kecil
        if (inputPoint <= 10)
        {
            memberPointKecil = 1;
        }
        else if (inputPoint >= 15)
        {
            memberPointKecil = 0;
        }
        else
        {
            memberPointKecil = (float)((15 - inputPoint) / 5);
        }

        //Point sedang
        if (inputPoint >= 15 && inputPoint <= 25)
        {
            memberPointSedang = 1;
        }
        else if (inputPoint <= 10 || inputPoint >= 30)
        {
            memberPointSedang = 0;
        }
        else if (inputPoint > 10 && inputPoint < 15)
        {
            memberPointSedang = (float)((inputPoint - 10) / 5);
        }
        else
        {
            memberPointSedang = (float)((30 - inputPoint) / 5);
        }

        //Point besar
        if (inputPoint >= 30)
        {
            memberPointBesar = 1;
        }
        else if (inputPoint <= 25)
        {
            memberPointBesar = 0;
        }
        else
        {
            memberPointBesar = (float)((inputPoint - 25) / 5);
        }
    }

    protected string inferensi()
    {
        nilaiTime = new float[3];
        nilaiMove = new float[3];
        nilaiPoint = new float[3];
        nilaiRank = new float[27];
        rank = new string[27];

        nilaiTime[0] = memberTimeLambat;
        nilaiTime[1] = memberTimeRata;
        nilaiTime[2] = memberTimeCepat;

        nilaiMove[0] = memberMoveBanyak;
        nilaiMove[1] = memberMoveSedang;
        nilaiMove[2] = memberMoveSedikit;

        nilaiPoint[0] = memberPointKecil;
        nilaiPoint[1] = memberPointSedang;
        nilaiPoint[2] = memberPointBesar;

        for (i = 0; i < 3; i++)
        {
            for (j = 0; j < 3; j++)
            {
                for (k = 0; k < 3; k++)
                {
                    if (nilaiTime[i] > 0 && nilaiMove[j] > 0 && nilaiPoint[k] > 0)
                    {
                        if (nilaiTime[i] <= nilaiMove[j] && nilaiTime[i] <= nilaiPoint[k])
                        {
                            nilaiRank[l] = nilaiTime[i];
                        }
                        else if (nilaiMove[j] <= nilaiTime[i] && nilaiMove[j] <= nilaiPoint[k])
                        {
                            nilaiRank[l] = nilaiMove[j];
                        }
                        else
                        {
                            nilaiRank[l] = nilaiPoint[k];
                        }
                        //Rule
                        //untuk Rank A
                        if (( i == 0) && (j == 2) && (k == 2))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 1) && (j == 1) && (k == 2))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 1) && (j == 2) && (k == 1))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 1) && (j == 2) && (k == 2))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 2) && (j == 0) && (k == 2))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 2) && (j == 1) && (k == 1))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 2) && (j == 1) && (k == 2))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 2) && (j == 2) && (k == 0))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 2) && (j == 2) && (k == 1))
                        {
                            rank[l] = "A";
                        }
                        else if ((i == 2) && (j == 2) && (k == 2))
                        {
                            rank[l] = "A";
                        }
                        //untuk Rank B
                        else if ((i == 0) && (j == 0) && (k == 2))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 0) && (j == 1) && (k == 1))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 0) && (j == 1) && (k == 2))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 0) && (j == 2) && (k == 0))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 0) && (j == 2) && (k == 1))
                        {
                            rank[l] = "B";
                        }
                        else if ((i== 1) && (j == 0) && (k == 1))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 1) && (j == 0) && (k == 2))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 1) && (j == 1) && (k == 0))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 1) && (j == 1) && (k == 1))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 1) && (j == 2) && (k == 0))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 2) && (j == 0) && (k == 0))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 2) && (j == 0) && (k == 1))
                        {
                            rank[l] = "B";
                        }
                        else if ((i == 2) && (j == 1) && (k == 0))
                        {
                            rank[l] = "B";
                        }
                        //untuk Rank C
                        else if ((i == 0) && (j == 0) && (k == 0))
                        {
                            rank[l] = "C";
                        }
                        else if ((i == 0) && (j == 0) && (k == 1))
                        {
                            rank[l] = "C";
                        }
                        else if ((i == 0) && (j == 1) && (k == 0))
                        {
                            rank[l] = "C";
                        }
                        else
                        {
                            rank[l] = "C";
                        }
                        Debug.Log("if " + nilaiTime[i] + " and " + nilaiMove[j] + " and " + nilaiPoint[k] + " then " + rank[l]+"nilai Rank : "+nilaiRank[l]);

                        l = l + 1;
                    }
                }
            }
        }

        for (i = 0; i < l; i++)
        {
            if (rank[i] == "A")
            {
                if (i == 0)
                {
                    terbesarA = nilaiRank[i];
                }
                else
                {
                    if (nilaiRank[i] > terbesarA)
                    {
                        terbesarA = nilaiRank[i];
                    }
                }
            }
            else if(rank[i] == "B")
            {
                if (i == 0)
                {
                    terbesarB = nilaiRank[i];
                }
                else
                {
                    if (nilaiRank[i] > terbesarB)
                    {
                        terbesarB = nilaiRank[i];
                    }
                }
            }
            else
            {
                if (i == 0)
                {
                    terbesarC = nilaiRank[i];
                }
                else
                {
                    if (nilaiRank[i] > terbesarB)
                    {
                        terbesarC = nilaiRank[i];
                    }
                }
            }
        }

        if(terbesarA > 0)
        {
            interferensiResult1 = "A";

        }
        if(terbesarB > 0)
        {
            interferensiResult2 = "B";
        }
        if(terbesarC > 0)
        {
            interferensiResult3 = "C";
        }
        
        interferensiResultFix = "Result :\nA : " + terbesarA + "\nB : " + terbesarB + "\nC : " + terbesarC;
        

        return interferensiResultFix;
    }

    protected string fuzzyresult()
    {
        string hasil;
        hasil = "Time :\n(Cepat : " + memberTimeCepat + ")   (Rata - rata : " + memberTimeRata + ")   (Lambat : " + memberTimeLambat +
                ")\nMove :\n(Banyak : " + memberMoveBanyak + ")   (Sedang : " + memberMoveSedang + ")   (Sedikit : " + memberMoveSedikit +
                ")\nPoint :\n(Besar : " + memberPointBesar + ")   (Sedang : " + memberPointSedang + ")   (Kecil : " + memberPointKecil + ")\n";

        return hasil;
    }

    protected float defuzzifikasi()
    {
        pengali1 = terbesarA;
        pengali2 = terbesarB;
        pengali3 = terbesarC;
        pengali4 = new float[10];
        node_sample = new float[10];

        node = delta;
        for(i=1; i<=10; i++)
        {
            if (node == 110)
            {
                break;
            }
            if (node >= 80)
            {
                pembilang += node * pengali1;
                X += 1;

            }else if((node >= 40) && (node <= 60))
            {
                pembilang += node * pengali2;
                Y += 1;
            }
            else if(node <= 20)
            {
                pembilang += node * pengali3;
                Z += 1;
            }
            else if((node > 60) && (node < 80))
            {
                if(pengali1 > pengali2)
                {
                    node_sample[a] = node;
                    pengali4[a] = (int)(((node_sample[a] - 60) / 20)*100);
                    pengali4[a] = (float)(pengali4[a] / 100);
                    if(pengali4[a] > pengali1)
                    {
                        pembilang += node * pengali1;
                        XY = pengali1;
                    }
                    else
                    {
                        pembilang += node * pengali4[a];
                        XY = pengali4[a];
                    }
                    
                }
                else
                {
                    node_sample[a] = node;
                    pengali4[a] = (int)(((80 - node_sample[a]) / 20)*100);
                    pengali4[a] = (float)(pengali4[a] / 100);
                    if (pengali4[a] > pengali2)
                    {
                        pembilang += node * pengali2;
                        XY = pengali2;
                    }
                    else
                    {
                        pembilang += node * pengali4[a];
                        XY = pengali4[a];
                    }
                }
                a += 1;
            }
            else
            {
                if (pengali2 > pengali3)
                {
                    node_sample[b] = node;
                    pengali4[b] = (int)(((node_sample[b] - 20) / 20)*100);
                    pengali4[b] = (float)(pengali4[b] / 100);
                    if (pengali4[b] > pengali2)
                    {
                        pembilang += node * pengali2;
                        YZ = pengali2;
                    }
                    else
                    {
                        pembilang += node * pengali4[b];
                        YZ = pengali4[b];
                    }
                }
                else
                {
                    node_sample[b] = node;
                    pengali4[b] = (int)(((40 - node_sample[b]) / 20)*100);
                    pengali4[b] = (float)(pengali4[b] / 100);
                    if (pengali4[b] > pengali3)
                    {
                        pembilang += node * pengali3;
                        YZ = pengali3;
                    }
                    else
                    {
                        pembilang += node * pengali4[b];
                        YZ = pengali4[b];
                    }
                }
                b += 1;
                
            }
            node += delta;
            Debug.Log(pembilang);
        }
        penyebut = (X * pengali1) + (Y * pengali2) + (Z * pengali3) + XY + YZ;
        Debug.Log("penyebut : " + penyebut + "   pembilang : " + pembilang);

        defuzzifikasiResult = pembilang / penyebut;

        return defuzzifikasiResult;
    }
}

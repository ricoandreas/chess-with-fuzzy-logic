using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horse : Chessman
{

    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];

        //atas kiri
        horseMove(CurrentX - 1, CurrentY + 2, ref r);
        
        //kiri atas
        horseMove(CurrentX - 2, CurrentY + 1, ref r);

        //atas kanan
        horseMove(CurrentX + 1, CurrentY + 2, ref r);

        //kanan atas
        horseMove(CurrentX + 2, CurrentY + 1, ref r);

        //bawah kiri
        horseMove(CurrentX - 1, CurrentY - 2, ref r);
        
        //kiri bawah
        horseMove(CurrentX - 2, CurrentY - 1, ref r);

        //bawah kanan
        horseMove(CurrentX + 1, CurrentY - 2, ref r);

        //kanan bawah
        horseMove(CurrentX + 2, CurrentY - 1, ref r);

        return r;
    }

    public void horseMove(int x, int y, ref bool[,] r)
    {
        Chessman c;
        if(x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            c = BoardManager.Instance.Chessmans[x, y];
            if (c == null)
                r[x, y] = true;
            else if (isWhite != c.isWhite)
                r[x, y] = true;
        }
    }
}

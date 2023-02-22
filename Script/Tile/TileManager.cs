using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RANGETYPE
{
    RECT = 0,
    RHOMBUS,
    
}

public class TileManager : MonoBehaviour
{
    public Tile tile_0_pre;
    public Tile tile_1_pre;
    public int size_width;
    public int size_hieght;

    public Tile[,] tiles;

    public Tile selectTile;
    public List<Tile> selectList = new List<Tile>();


    public List<_NODE> openList = new List<_NODE>();
    public List<_NODE> closeList = new List<_NODE>();
    public List<_NODE> finalList = new List<_NODE>();
    void Start()
    {
        
    }


    void Update()
    {
        
    }


    public Enemy CreateEnemy(int x,int z, string c_name)                 //unit이 아니라 enemy라는 스크립트를 하나 만들어야 할거같기도
    {
        if (x < 0 || z < 0)
            return null;
        if (x >= tiles.GetLength(0) || z >= tiles.GetLength(1))
            return null;

        if (tiles[x, z].GetParticipant() != null)
        {
            return null;
        }

        Enemy enemy = Instantiate(Resources.Load<Enemy>("Enemy/" + c_name));
        enemy.init();
        enemy.CreateModel();
        tiles[x, z].Pile(enemy);
        return enemy;
    }

    public Character CreateCharacter(int x,int z, string c_name)        //이거 안씀 만들어진거 모델을 만들고 init,pile하는 방법으로 바꿀듯
    {
        if (x < 0 || z < 0)
            return null;
        if (x >= tiles.GetLength(0) || z >= tiles.GetLength(1))
            return null;

        if (tiles[x, z].GetParticipant() != null)
        {
            return null;
        }

        Character character = Instantiate(Resources.Load<Character>("Character/" + c_name));
        character.init();
        tiles[x, z].Pile(character);
        return character;
    }

    public Participant CreateParticipant(int x,int z, string p_name)
    {
        if (x < 0 || z < 0)
            return null;
        if (x >= tiles.GetLength(0) || z >= tiles.GetLength(1))
            return null;

        if(tiles[x, z].GetParticipant() != null)
        {
            return null;
        }

        Participant participant = Instantiate(Resources.Load<Participant>("Barricade/" + p_name));
        participant.init();
        participant.CreateModel();
        tiles[x, z].Pile(participant);
        return participant;
    }

    public void SelectMoveType(int x,int z,int size, RANGETYPE type)
    {
        switch(type)
        {
            case RANGETYPE.RECT:
                SelectMoveTile_Rect(x, z, size);
                break;
            case RANGETYPE.RHOMBUS:
                SelectMoveTile_Rhombus(x, z, size);
                break;  
        }
    }
    public void SelectAttackType(int x, int z, int size, RANGETYPE type)
    {
        switch (type)
        {
            case RANGETYPE.RECT:
                SelectAttackTile_Rect(x, z, size);
                break;
            case RANGETYPE.RHOMBUS:
                SelectAttackTile_Rhombus(x, z, size);
                break;
        }
    }
    public bool FindParticipantType(Participant participant ,int x, int z, int size, RANGETYPE type)       //enemy가 플레이어를 찾을때 쓸거임 (공격할 수 있는 거리인지)     
    {
        switch (type)
        {
            case RANGETYPE.RECT:
                return FindParticipantRect(participant, x, z, size);
            case RANGETYPE.RHOMBUS:
                return FindParticipantRhombus(participant, x, z, size);
        }
        return false;
    }
    private bool FindParticipantRect(Participant participant, int x, int z, int size)
    {
        List<Participant> participants = new List<Participant>();

        for (int i = -size; i <= size; i++)
        {
            if (x + i < 0 || x + i >= tiles.GetLength(0))
            {
                continue;
            }
            for (int j = -size; j <= size; j++)
            {
                if (z + j < 0 || z + j >= tiles.GetLength(1))
                {
                    continue;
                }

                if (tiles[x+i,z+j].GetParticipant() != null)
                {
                    participants.Add(tiles[x+i,z+j].GetParticipant());
                }

            }
        }

        if (participants.Contains(participant))
            return true;
        return false;
    }
    private bool FindParticipantRhombus(Participant participant, int x, int z, int size)
    {
        List<Participant> participants = new List<Participant>();

        for (int i = -size; i <= size; i++)
        {
            if (x + i < 0 || x + i >= tiles.GetLength(0))
            {
                continue;
            }
            for (int j = -size + Mathf.Abs(i); j <= Mathf.Abs(-size + Mathf.Abs(i)); j++)
            {
                if (z + j < 0 || z + j >= tiles.GetLength(1))
                {
                    continue;
                }

                if (tiles[x + i, z + j].GetParticipant() != null)
                {
                    participants.Add(tiles[x + i, z + j].GetParticipant());
                }

            }
        }

        if (participants.Contains(participant))
            return true;
        return false;
    }

    private void SelectMoveTile_Rect(int x,int z, int size)
    {
        SelectRect(x, z, size, true, true);
        tiles[x, z].DeselectTile();
    }
    private void SelectMoveTile_Rhombus(int x, int z, int size)
    {
        SelectRhombus(x, z, size, true, true);
        tiles[x, z].DeselectTile();
    }

    private void SelectAttackTile_Rect(int x, int z, int size)
    {
        SelectRect(x, z, size, true, false);
        tiles[x, z].DeselectTile();
    }
    private void SelectAttackTile_Rhombus(int x, int z, int size)
    {
        SelectRhombus(x, z, size, true, false);
        tiles[x, z].DeselectTile();
    }

    public void SelectRhombus(int x,int z, int size, bool immotal, bool move)     //마름모
    {
        ClearTile();
        for (int i = -size; i <= size; i++)
        {
            if (x + i < 0 || x + i >= tiles.GetLength(0))
            {
                continue;
            }
            for (int j = -size + Mathf.Abs(i); j <= Mathf.Abs(-size + Mathf.Abs(i)); j++)
            {
                if (z + j < 0 || z + j >= tiles.GetLength(1))
                {
                    continue;
                }

                AddTile(tiles[x + i, z + j], immotal, move);

            }
        }
    }
    public void SelectRect(int x,int z,int size, bool immotal, bool move)        //네모
    {
        ClearTile();
        for(int i = -size; i <= size; i++)
        {
            if(x + i  < 0 || x + i >= tiles.GetLength(0))
            {
                continue;
            }
            for(int j = -size; j <= size; j++)
            {
                if (z + j < 0 || z + j >= tiles.GetLength(1))
                {
                    continue;
                }

                AddTile(tiles[x+i, z+j], immotal, move);

            }
        }
    }


    public void OneTileOn(Tile tile)
    {
        if(selectTile != null)
        {
            selectTile.DeselectTile();
        }
        selectTile = tile;
        selectTile.SelectTile();
    }
    public void OneTileDelete()
    {
        if (selectTile != null && !selectList.Contains(selectTile))
        {
            selectTile.DeselectTile();
        }
        selectTile = null;
    }

    public Participant AddTile(Tile tile,bool immortal, bool onside)
    {
        bool ck1 = !immortal;
        bool ck2 = !onside;

        if(immortal)
        {
            if (!tile.Immortality())        //무적이 아니여야 true
                ck1 = true;
        }
        if(onside)
        {
            if (tile.CanMove())             //위에 아무것도 없어야 true
                ck2 = true;
        }

        if(ck1 == true && ck2 == true)
        {
            if (!selectList.Contains(tile))
            {
                tile.SelectTile();
                selectList.Add(tile);
                return tile.GetParticipant();
            }
        }

        return null;
    }

    public void ClearTile()
    {
        for(int i = 0; i < selectList.Count; i++)
        {
            selectList[i].DeselectTile();
        }
        selectList.Clear();
    }

    public int Difference(int x1, int z1, int x2, int z2)       //거리
    {
        int one = x1 - x2;
        int two = z1 - z2;
        return Mathf.Abs(one) + Mathf.Abs(two);
    }

    public void CreateTile(int x, int z)
    {
        size_width = x;
        size_hieght = z;
        tiles = new Tile[size_width, size_hieght];

        for (int i = 0; i < size_width; i++)     //가로
        {
            for (int j = 0; j < size_hieght; j++)    //세로
            {
                if ((i + j) % 2 == 0)
                {
                    Tile tile = Instantiate(tile_0_pre, new Vector3(i, 0, j), Quaternion.identity, transform);
                    tiles[i, j] = tile;
                }
                else
                {
                    Tile tile = Instantiate(tile_1_pre, new Vector3(i, 0, j), Quaternion.identity, transform);
                    tiles[i, j] = tile;
                }
            }
        }

        Camera.main.transform.position = new Vector3(size_width / 2 - 0.5f, Camera.main.transform.position.y, size_hieght / 2 - 0.5f);
    }
    public void AddTile(int x, int z)
    {
        Tile[,] tile = new Tile[size_width + x, size_hieght + z];

        for(int i = 0; i < size_width + x; i++)
        {
            for(int j = 0; j < size_hieght + z; j++)
            {
                if(i < size_width && j < size_hieght)
                {
                    tile[i, j] = tiles[i, j];
                }
                else
                {
                    if ((i + j) % 2 == 0)
                    {
                        tile[i, j] = Instantiate(tile_0_pre, new Vector3(i, 0, j), Quaternion.identity, transform);
                    }
                    else
                    {
                        tile[i, j] = Instantiate(tile_1_pre, new Vector3(i, 0, j), Quaternion.identity, transform);
                    }
                }
            }
        }
        //tiles = new Tile[size_width + x, size_hieght + z];
        tiles = tile;
        size_width = size_width + x;
        size_hieght = size_hieght + z;
        Camera.main.transform.position = new Vector3(size_width / 2 - 0.5f, Camera.main.transform.position.y, size_hieght / 2 - 0.5f);
    }

    public void AStar(int x1, int z1, int x2, int z2, bool diagonal)
    {
        openList.Clear();
        closeList.Clear();
        finalList.Clear();

        if (x1 < 0 || x2 < 0)
            return;
        if (z1 < 0 || z2 < 0)
            return;
        if (x1 > tiles.GetLength(0) || x2 > tiles.GetLength(0))
            return;
        if (z1 > tiles.GetLength(1) || z2 > tiles.GetLength(1))
            return;

        openList.Add(new _NODE(x1, z1, Difference(x1, z1, x2, z2)));
        openList[openList.Count - 1].previous = -1;
        int test = 0;
        while (true)
        {
            test++;
            if (openList.Count <= 0)
            {
                Debug.Log("길이 없음");
                return;
            }
            int index = FindMinDis(openList);

            closeList.Add(openList[index]);
            openList.RemoveAt(index);

            if (closeList[closeList.Count - 1].distance == 0)
            {
                break;
            }


            if(diagonal)
            {
                // x x 0
                // x x x
                // x x x
                //오른쪽 위
                if (closeList[closeList.Count - 1].x + 1 < tiles.GetLength(0) && closeList[closeList.Count - 1].z + 1 < tiles.GetLength(1))
                {
                    if (tiles[closeList[closeList.Count - 1].x + 1, closeList[closeList.Count - 1].z + 1].CanMove() || (closeList[closeList.Count - 1].x + 1 == x2 && closeList[closeList.Count - 1].z + 1 == z2))
                    {
                        openList.Add(new _NODE(closeList[closeList.Count - 1].x + 1, closeList[closeList.Count - 1].z + 1));
                        openList[openList.Count - 1].Difference(x2, z2);
                        openList[openList.Count - 1].previous = closeList.Count - 1;
                    }
                }
                // x x x
                // x x x
                // 0 x x
                //왼쪽 아래
                if (closeList[closeList.Count - 1].x - 1 >= 0 && closeList[closeList.Count - 1].z - 1 >= 0)
                {
                    if (tiles[closeList[closeList.Count - 1].x - 1, closeList[closeList.Count - 1].z - 1].CanMove() || (closeList[closeList.Count - 1].x - 1 == x2 && closeList[closeList.Count - 1].z - 1 == z2))
                    {
                        openList.Add(new _NODE(closeList[closeList.Count - 1].x - 1, closeList[closeList.Count - 1].z - 1));
                        openList[openList.Count - 1].Difference(x2, z2);
                        openList[openList.Count - 1].previous = closeList.Count - 1;
                    }
                }
                // 0 x x
                // x x x
                // x x x
                //위 왼쪽
                if (closeList[closeList.Count - 1].x - 1 >= 0 && closeList[closeList.Count - 1].z + 1 < tiles.GetLength(1))
                {
                    if (tiles[closeList[closeList.Count - 1].x - 1, closeList[closeList.Count - 1].z + 1].CanMove() || (closeList[closeList.Count - 1].x - 1 == x2 && closeList[closeList.Count - 1].z + 1 == z2))
                    {
                        openList.Add(new _NODE(closeList[closeList.Count - 1].x - 1, closeList[closeList.Count - 1].z + 1));
                        openList[openList.Count - 1].Difference(x2, z2);
                        openList[openList.Count - 1].previous = closeList.Count - 1;
                    }
                }
                // x x x
                // x x x
                // x x 0
                //아래 오른쪽
                if (closeList[closeList.Count - 1].x + 1 < tiles.GetLength(0) && closeList[closeList.Count - 1].z - 1 >= 0)
                {
                    if (tiles[closeList[closeList.Count - 1].x + 1, closeList[closeList.Count - 1].z - 1].CanMove() || (closeList[closeList.Count - 1].x + 1 == x2 && closeList[closeList.Count - 1].z - 1 == z2))
                    {
                        openList.Add(new _NODE(closeList[closeList.Count - 1].x + 1, closeList[closeList.Count - 1].z - 1));
                        openList[openList.Count - 1].Difference(x2, z2);
                        openList[openList.Count - 1].previous = closeList.Count - 1;
                    }
                }
            }

            // x x x
            // x x 0
            // x x x
            //오른쪽
            if (closeList[closeList.Count - 1].x + 1 < tiles.GetLength(0))
            {
                if (tiles[closeList[closeList.Count - 1].x + 1, closeList[closeList.Count - 1].z].CanMove() || (closeList[closeList.Count - 1].x + 1 == x2 && closeList[closeList.Count - 1].z == z2))
                {
                    openList.Add(new _NODE(closeList[closeList.Count - 1].x + 1, closeList[closeList.Count - 1].z));
                    openList[openList.Count - 1].Difference(x2, z2);
                    openList[openList.Count - 1].previous = closeList.Count - 1;
                }
            }
            // x x x
            // 0 x x
            // x x x
            //왼쪽
            if (closeList[closeList.Count - 1].x - 1 >= 0)
            {
                if (tiles[closeList[closeList.Count - 1].x - 1, closeList[closeList.Count - 1].z].CanMove() || (closeList[closeList.Count - 1].x - 1 == x2 && closeList[closeList.Count - 1].z == z2))
                {
                    openList.Add(new _NODE(closeList[closeList.Count - 1].x - 1, closeList[closeList.Count - 1].z));
                    openList[openList.Count - 1].Difference(x2, z2);
                    openList[openList.Count - 1].previous = closeList.Count - 1;
                }
            }
            // x 0 x
            // x x x
            // x x x
            //위
            if (closeList[closeList.Count - 1].z + 1 < tiles.GetLength(1))
            {
                if (tiles[closeList[closeList.Count - 1].x, closeList[closeList.Count - 1].z + 1].CanMove() || (closeList[closeList.Count - 1].x == x2 && closeList[closeList.Count - 1].z + 1 == z2))
                {
                    openList.Add(new _NODE(closeList[closeList.Count - 1].x, closeList[closeList.Count - 1].z + 1));
                    openList[openList.Count - 1].Difference(x2, z2);
                    openList[openList.Count - 1].previous = closeList.Count - 1;
                }
            }
            // x x x
            // x x x
            // x 0 x
            //아래
            if (closeList[closeList.Count - 1].z - 1 >= 0)
            {
                if (tiles[closeList[closeList.Count - 1].x, closeList[closeList.Count - 1].z - 1].CanMove() || (closeList[closeList.Count - 1].x == x2 && closeList[closeList.Count - 1].z - 1 == z2))
                {
                    openList.Add(new _NODE(closeList[closeList.Count - 1].x, closeList[closeList.Count - 1].z - 1));
                    openList[openList.Count - 1].Difference(x2, z2);
                    openList[openList.Count - 1].previous = closeList.Count - 1;
                }
            }
            


            if (test > 100)
            {
                Debug.Log("오류 길을 못찾음");
                break;
            }

        }
        int temp = closeList.Count - 1;

        while (true)
        {

            finalList.Add(closeList[temp]);
            temp = closeList[temp].previous;

            if (finalList[finalList.Count - 1].previous == -1)
            {
                break;
            }
        }
        finalList.Reverse();
        Debug.Log("길을 찾음");
    }

    public Tile GetNextTile()
    {
        return tiles[finalList[1].x, finalList[1].z];
    }

    public int FindMinDis(List<_NODE> list)
    {
        int min = list[0].distance;
        int index = 0;

        for (int i = 1; i < list.Count; i++)
        {
            if (min > list[i].distance)
            {
                min = list[i].distance;
                index = i;

            }
        }

        return index;
    }
}

[System.Serializable]
public class _NODE     //부모노드 gfh
{
    public int x;    //위치
    public int z;
    public int distance;      //거리
    public int previous;
    public _NODE(int x, int z, int d)
    {
        this.x = x;
        this.z = z;
        distance = d;
    }
    public _NODE(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    public void Difference(int x2, int z2)
    {
        int one = x - x2;
        int two = z - z2;
        distance = Mathf.Abs(one) + Mathf.Abs(two);
    }
}

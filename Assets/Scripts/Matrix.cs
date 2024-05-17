using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Matrix : MonoBehaviour
{
    public GameObject cellModel;
    public int colCount, rowCount;

    public Color color;

    List<List<Cell>> cells = new List<List<Cell>>();

    void Start()
    {
        for (int row = 0; row < rowCount; row++)
        {
            var line = new List<Cell>();

            for (int col = 0; col < colCount; col++)
            {
                var cellGO = Instantiate(cellModel, new Vector3(col, row), Quaternion.identity, transform);
                var cell = cellGO.GetComponent<Cell>();
                cell.TurnOff();
                ChangeColor(cell);
                line.Add(cell);
            }
            cells.Add(line);
        }
    }

    void ChangeColor(Cell cell)
    {
        var r = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);

        var color = new Color(r, g, b);

        cell.spriteRenderer.color = color;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var cell = GetCell(Input.mousePosition);
            if (cell != null)
            {
                cell.TurnOn();
                cell.spriteRenderer.color = color;
            }
        }

        if (Input.GetMouseButton(1))
        {
            var cell = GetCell(Input.mousePosition);
            if (cell != null)
            {
                cell.TurnOff();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveImage();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            Run();
        }
    }

    Cell GetCell(Vector3 mousePos)
    {
        var coord = Camera.main.ScreenToWorldPoint(mousePos);
        int col = Mathf.RoundToInt(coord.x);
        int row = Mathf.RoundToInt(coord.y);

        if (col < 0 || row < 0 || col >= colCount || row >= rowCount)
        {
            return null;
        }

        var cell = cells[row][col];
        return cell;
    }
    
    void SaveImage()
    {
        var texture = new Texture2D(colCount, rowCount);

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                var cell = cells[col][row];

                var color = cell.isAlive ? Color.white : Color.black;

                texture.SetPixel(col, row, color);
            }
        }

        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();

        var dirPath = Application.dataPath + "/Images/";

        if (!Directory.Exists(dirPath))
        {
            // Criar o diretório
            Directory.CreateDirectory(dirPath);
        }

        File.WriteAllBytes(dirPath + "imagem.png", bytes);
    }

    //ALGORITMO GAME OF LIFE CONWAY
    int ContaCelulasVivas(int col, int row)
    {
        int contador = 0;

        for (int subCol = col - 1; subCol <= col + 1; subCol++)
        {
            for (int subRow = row - 1; subRow <= row + 1; subRow++)
            {
                if (subCol == col && subRow == row)
                {
                    // É ela mesma
                    continue;
                }

                if (subCol < 0 ||  subRow < 0 || subCol >= colCount || subRow >= rowCount)
                {
                    // Está fora da matriz
                    continue;
                }

                var cell = cells[subCol][subRow];

                if (cell.isAlive)
                {
                    contador++;
                }
            }
        }

        return contador;
    }

    void Run()
    {
        for (int col = 0; col < colCount; col++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                var cell = cells[col][row];

                var count = ContaCelulasVivas(col, row);

                // Qualquer célula viva com menos de dois vizinhos vivos morre de solidão
                if (cell.isAlive && count < 2)
                {
                    cell.nextStatus = false;
                }

                // Qualquer célula viva com mais de três de vizinhos vivos, morre
                if (cell.isAlive && count > 3)
                {
                    cell.nextStatus = false;
                }

                // Qualquer célula com exatamente três vizinhos vivos se torna uma célula viva
                if (count == 3)
                {
                    cell.nextStatus = true;
                }
            }
        }

        for (int col = 0; col < colCount; col++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                var cell = cells[col][row];

                cell.RunNextStatus();
            }
        }
    }
}

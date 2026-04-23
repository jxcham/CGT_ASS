using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PerformanceTest : MonoBehaviour
{
    [Header("References")]
    public GridGenerator gridGenerator;
    public DijkstraPathfinding pathfinding;

    [Header("Test Settings")]
    public int gridSize = 10;
    public int testRuns = 20;
    public int innerLoop = 50; // 不要太高（因为你有动画）

    private List<double> times = new List<double>();

    void Start()
    {
        RunTest();
    }

    void RunTest()
    {
        times.Clear();

        gridGenerator.width = gridSize;
        gridGenerator.height = gridSize;

        UnityEngine.Debug.Log("===== TEST 2 START =====");

        for (int i = 0; i < testRuns; i++)
        {
            // 生成 grid
            gridGenerator.GenerateGrid();

            // 设置起点终点
            Tile start = gridGenerator.grid[0, 0];
            Tile end = gridGenerator.grid[gridSize - 1, gridSize - 1];

            pathfinding.startTile = start;
            pathfinding.endTile = end;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // 执行多次（提升精度）
            for (int j = 0; j < innerLoop; j++)
            {
                pathfinding.FindPath();
            }

            stopwatch.Stop();

            double time = stopwatch.Elapsed.TotalMilliseconds / innerLoop;
            times.Add(time);

            UnityEngine.Debug.Log("Run " + (i + 1) + ": " + time + " ms");
        }

        CalculateResult();
    }

    void CalculateResult()
    {
        double total = 0;
        double min = times[0];
        double max = times[0];

        foreach (double t in times)
        {
            total += t;
            if (t < min) min = t;
            if (t > max) max = t;
        }

        double avg = total / times.Count;

        UnityEngine.Debug.Log("===== FINAL RESULT =====");
        UnityEngine.Debug.Log("Average: " + avg + " ms");
        UnityEngine.Debug.Log("Min: " + min + " ms");
        UnityEngine.Debug.Log("Max: " + max + " ms");
    }
}
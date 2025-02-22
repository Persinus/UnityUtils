using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class U005_UniTaskTest : MonoBehaviour
{

    async void Start()
    {
        Debug.Log("Bắt đầu test UniTask...");

        await TestDelay();
        await TestParallelTasks();

        Debug.Log("UniTask Test Hoàn Tất!");
    }

    async UniTask TestDelay()
    {
        Debug.Log("Bắt đầu chờ 2 giây...");
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        Debug.Log("Đã chờ xong!");
    }

    async UniTask TestParallelTasks()
    {
        Debug.Log("Chạy song song 2 task...");
        await UniTask.WhenAll(TaskA(), TaskB());
        Debug.Log("Cả hai task đã hoàn thành!");
    }

    async UniTask TaskA()
    {
        await UniTask.Delay(1000);
        Debug.Log("Task A hoàn thành!");
    }

    async UniTask TaskB()
    {
        await UniTask.Delay(1500);
        Debug.Log("Task B hoàn thành!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Nhấn phím "T" để test
        {
            Debug.Log("Bắt đầu TestWaitForInput...");
            TestWaitForInput().Forget(Debug.LogException);
        }
    }

    async UniTask TestWaitForInput()
    {
        Debug.Log("Nhấn SPACE để tiếp tục...");
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        Debug.Log("Đã nhấn SPACE!");
    }

}

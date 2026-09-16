using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssyncLogic : MonoBehaviour
{
    // Start is called before the first frame update
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PurchaseService s = new PurchaseService(new PalyerDataItems(), new SuncSaveLoadServbice());

            s.Purchase(2);
           
        }
    }
}


public class PalyerDataItems
{
    public HashSet<int> Items = new HashSet<int>();
}

public class SuncSaveLoadServbice
{
    private readonly string path = $"{Path.Combine(Application.dataPath, $"{nameof(PalyerDataItems)}") }";


    public PalyerDataItems Load()
    {
        return default;
    }

    public   Task  Save(PalyerDataItems data)
    {
      //  Task.Delay(5000);
        string json = JsonUtility.ToJson(data);
       // return  File.WriteAllTextAsync(path, json);

       
        return Task.Run(Foo);
    }

    
    public async Task Foo()
    {
        Debug.Log("��� 4 �������");
        await Task.Delay(40040);
        Debug.Log("� 2 ����� � �  4 �������");
    }
}

public class PurchaseService
{
    private readonly PalyerDataItems playerDataItems;
    private readonly SuncSaveLoadServbice saveLoadServbice;



    public PurchaseService(PalyerDataItems playerDataItems, SuncSaveLoadServbice saveLoadServbice)
    { 
        this.playerDataItems = playerDataItems; 
        this.saveLoadServbice = saveLoadServbice;    
    }


    public async void Purchase(int product)
    {
        if (playerDataItems.Items.Contains(product))
        {
            throw new InvalidDataException();
        }

        playerDataItems.Items.Add(product);
        Debug.Log("� ������ �����");
        saveLoadServbice.Save(playerDataItems);
        Debug.Log("� 1 ����� � � ��� ������� 2 ������� ������� � �� ������ ������");


    }
}


public class PlayerNewMovement<T>:MonoBehaviour
{

    private async void Start()
    {
        //DoSomethigDelayedTask();
        //  await DoSomethigDelayedTask();//await ���� ������� �����, � ����� � ����� ���� ����� ��������� 



        /* ������������� 
        Task<string> task = DoSomethigDelayedTask();
        Task task2 = DoSomethigDelayedTask1();
        Task task3 = DoSomethigDelayedTask2();
        await task;
        await task2;
        await task3;
        */


        //  string result =  await task;   
        // name = result;   

        /* ���������������
        Task<string> task = DoSomethigDelayedTask();
        await task;
        Task task2 = DoSomethigDelayedTask1();
        await task2;
        Task task3 = DoSomethigDelayedTask2();
  
        await task3;
        */

        Task<string> task = DoSomethigDelayedTask();
        Task task2 = DoSomethigDelayedTask1();
        Task task3 = DoSomethigDelayedTask2();
        
        //  await Task.WhenAll(task, task2, task3);
        Task allTasks =   Task.WhenAll(task, task2, task3);// ��������� ��� ������ ������ ���� ������
        await allTasks;
        // all ����� ������ �������� - ������ ������� ��� ������ ������ ����������� � ����� ���� ��� ���������� �������� ����� ������� ����
        Task allTasks2 = Task.WhenAny(task, task2, task3);// ��������� ��� ������ ������ ���� ������, ����� ���������� ��������� ������ �������� (�������� 1 �� 3 ��������)
        await allTasks;

    }

   
    private async Task<string> DoSomethigDelayedTask()
    {
        Task delay =  Task.Delay(3000);
        await delay;
        return  "new Name"; 

    }

    private async Task DoSomethigDelayedTask1()
    {
        Task delay = Task.Delay(3000);
        await delay;
        

    }

    private async Task DoSomethigDelayedTask2()
    {
        Task delay = Task.Delay(3000);
        await delay;

    }


}

public class AnimationLauncher : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private AnimationButton animButton;
    private CancellationTokenSource tokenSouce;
    private int currentClick = 0;
    private const int ClicksToCansel = 3;


    public bool AnimationsIsPlaying => tokenSouce != null;


    private void Awake()
    {
        button.onClick.AddListener(StartAnimation);
    }

    private async void StartAnimation()
    {
        StartCoroutine(CheckClick());
        await animButton.Animate(3f, tokenSouce.Token);
    }

    private IEnumerator CheckClick()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentClick++;
                if (currentClick == ClicksToCansel)
                {
                    tokenSouce.Cancel();
                      
                }
            }
            yield return null;  
        }
    }
}


public class AnimationButton:MonoBehaviour
{
    public async Task Animate(float duration, CancellationToken cancellationToken)
    {
        await  AnimationScale(Vector3.one, new Vector3(2,2,2),duration /2,cancellationToken);
        await  AnimationScale(Vector3.one, new Vector3(2, 2, 2), duration / 2, cancellationToken);

    }

    private async Task AnimationScale(Vector3 from, Vector3 to, float duration, CancellationToken token)
    {
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await Task.Yield();
          
            if (token.IsCancellationRequested)
            {
                return;
            }

        }
        transform.localScale = to;
    }
}

public class AnimationLauncher1 : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private AnimationButton animButton;
    private CancellationTokenSource tokenSouce;
    private int currentClick = 0;
    private const int ClicksToCansel = 3;


    public bool AnimationsIsPlaying => tokenSouce != null;


    private void Awake()
    {
        button.onClick.AddListener(StartAnimation);
    }

    private async void StartAnimation()
    {
        TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
        StartCoroutine(CheckClick(tcs));
        await animButton.Animate(3f, tokenSouce.Token);
        try
        {
            int a = await tcs.Task;
        }
        catch (Exception e)
        {
            Debug.LogError("Error whith animation" + e.Message);
            throw;
        }
    }

    private IEnumerator CheckClick(TaskCompletionSource<int> tcs)
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentClick++;
                if (currentClick == ClicksToCansel)
                {
                  tcs.SetResult(currentClick);
                }
            }
            yield return null;
        }
    }
}


public class AnimationButton1 : MonoBehaviour
{
    public async Task Animate(float duration, TaskCompletionSource<int> cancelAnim)
    {
        await AnimationScale(Vector3.one, new Vector3(2, 2, 2), duration / 2, cancelAnim);
        await AnimationScale(Vector3.one, new Vector3(2, 2, 2), duration / 2, cancelAnim);

    }

    private async Task AnimationScale(Vector3 from, Vector3 to, float duration, TaskCompletionSource<int> taskAnim)
    {
        float elapsedTime = 0f;
     
        int idTread = Environment.CurrentManagedThreadId;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await Task.Yield();
            int a = await taskAnim.Task;
            if (a==4)
            {
                return;
            }

        }
        transform.localScale = to;
    }
}

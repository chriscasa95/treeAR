using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using SQLiteNetExtensions;
using Cysharp.Threading.Tasks;
using System.IO;

public class database : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public async UniTask AddCustomerAsync(Customer customer)
    {
        var databasePath = Application.persistentDataPath + "/" + databaseName;
        var db = new SQLiteAsyncConnection(databasePath);

        await db.InsertAsync(customer);
    }

    public async UniTask<Customer> GetCustomerAsync(int id)
    {
        var databasePath = Path.Combine(Application.persistentDataPath, databaseName);
        var db = new SQLiteAsyncConnection(databasePath);

        Customer customer = await db.GetAsync<Customer>(customer.Id);
        return customer;
    }

    public async void Main()
    {
        var databasePath = $"{Application.persistentDataPath}/{databaseName}";
        var db = new SQLiteAsyncConnection(databasePath);

        await db.CreateTableAsync<Customer>();

        await AddCustomerAsync(new Customer());
        var customer = await GetCustomerAsync(0);
    }
}


public class Customer
{
    [AutoIncrement, PrimaryKey]
    public int Id { get; set; }

    [MaxLength(64)]
    public string FirstName { get; set; }

    [MaxLength(64)]
    public string LastName { get; set; }

    [MaxLength(64), Indexed]
    public string Email { get; set; }
}

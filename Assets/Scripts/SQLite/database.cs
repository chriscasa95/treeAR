using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using SQLiteNetExtensions;
using Cysharp.Threading.Tasks;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.Networking;

public class Database
{
    // Start is called before the first frame update

    readonly string databaseName = "tree.db";

    void Start()
    {
        //copy_db();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    //public static System.Collections.IEnumerable copy_db()
    //{
    //    UnityWebRequest uwr = new UnityWebRequest(Path.Combine(Application.streamingAssetsPath, "tree.db"));
    //    uwr.downloadHandler = new DownloadHandlerBuffer();
    //    yield return uwr.Send();
    //    if (uwr.isNetworkError || uwr.isHttpError)
    //    {
    //        Debug.Log(uwr.error);
    //    }
    //    else
    //    {
    //        File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "tree.db"), uwr.downloadHandler.data);
    //    }
    //}

    public async UniTask<Tree> GetTreeAsync(string Plant_ID)
    {
        var databasePath = Path.Combine(Application.streamingAssetsPath, databaseName);

        var db = new SQLiteAsyncConnection(databasePath);

        var query = $"SELECT * FROM tree WHERE Plant_ID='{Plant_ID}';";

        Tree tree = await db.FindWithQueryAsync<Tree>(query);

        return tree;
    }
}


public class Tree
{
    [MaxLength(64)]
    public string Plant_ID { get; set; }

    [MaxLength(256)]
    public string Plant_name_EN { get; set; }

    [MaxLength(256)]
    public string Plant_name_VN { get; set; }

    public float Plant_size_m { get; set; }

    [MaxLength(256)]
    public string Plant_name_scientific { get; set; }

    [MaxLength(256)]
    public string Leaf_material_ID { get; set; }

    public float Leaf_size_m { get; set; }

    public int Leafe_amount { get; set; }

    public float Cone_hight_m { get; set; }

    public float Cone_width_m { get; set; }

    public float LeaveFall_width_m { get; set; }


    [MaxLength(256)]
    public string Plant_family { get; set; }

    [MaxLength(256)]
    public string Occurrence { get; set; }

    [MaxLength(256)]
    public string Growing_conditions { get; set; }

    [MaxLength(1024)]
    public string Plant_description_long_EN { get; set; }

    [MaxLength(1024)]
    public string Plant_description_long_VN { get; set; }

    [MaxLength(1024)]
    public string Plant_description_long_DE { get; set; }

}

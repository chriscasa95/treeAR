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

    string databaseName = "tree.db";
    Tree tree_obj= new Tree();

    async void Start()
    {
        await GetTreeAsync("RoystoneaRegia_C1");
    }

    // Update is called once per frame
    void Update()
    {
      
    }


    public async UniTask<Tree> GetTreeAsync(string Plant_ID)
    {
        var databasePath = Application.streamingAssetsPath + "/" + databaseName;
        var db = new SQLiteAsyncConnection(databasePath);

        var query = $"SELECT * FROM tree WHERE Plant_ID='{Plant_ID}';";

        Tree tree = await db.FindWithQueryAsync<Tree>(query);

        print(tree.Plant_ID);
        print(tree.Plant_name_VN);
        print(tree.Cone_hight_m);
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

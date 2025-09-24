using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;
using MessagePack;
using Newtonsoft.Json;
using UnityEngine;
using Generated.DAO;
using Generated.Editor;

namespace Infrastructure.EditorTools
{
    public class JsonToLiteDbImporter
    {
        [UnityEditor.MenuItem("Infrastructure/Tools/Import JSON to LiteDB")]
        public static void ImportJson()
        {
            var dbPath = $"{Application.streamingAssetsPath}/DB/Main.db";
            var jsonPath = $"{Application.streamingAssetsPath}/Json";
            var dbDir = Path.GetDirectoryName(dbPath);

            if (Directory.Exists(dbDir))
            {
                Directory.Delete(dbDir, recursive: true);
            }

            Directory.CreateDirectory(dbDir);
            UnityEngine.Debug.Log($"DBディレクトリ作成: {dbDir}");

            using var db = new LiteDatabase(dbPath);

            ImporterRegistry.ImportAll(db, jsonPath);
        }

        public static void ImportJsonFile<T>(LiteDatabase db, string folderPath, string fileName, string collectionName)
        {
            var path = Path.Combine(folderPath, fileName);
            if (!File.Exists(path))
            {
                UnityEngine.Debug.LogWarning($"⚠ ファイルが存在しません: {path}");
                return;
            }

            string json;
            try
            {
                json = File.ReadAllText(path).Trim();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"❌ ファイル読み込み失敗: {path}\n{ex}");
                return;
            }

            try
            {
                var col = db.GetCollection<T>(collectionName);

                if (json.StartsWith("["))
                {
                    var list = JsonConvert.DeserializeObject<List<T>>(json);
                    foreach (var item in list)
                    {
                        var mp = MessagePackSerializer.Serialize(item);
                        var deserialized = MessagePackSerializer.Deserialize<T>(mp);
                        col.Upsert(deserialized);
                    }
                }
                else
                {
                    var item = JsonConvert.DeserializeObject<T>(json);
                    var mp = MessagePackSerializer.Serialize(item);
                    var deserialized = MessagePackSerializer.Deserialize<T>(mp);
                    col.Upsert(deserialized);
                }

                UnityEngine.Debug.Log($"✅ {fileName} → {collectionName} に Upsert 成功");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"❌ {fileName} のインポート失敗: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}

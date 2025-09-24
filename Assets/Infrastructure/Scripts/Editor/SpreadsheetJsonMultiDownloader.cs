using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks;

namespace Infrastructure.EditorTools
{
    public class SpreadsheetJsonMultiDownloader : EditorWindow
    {
        // URLはゲームの環境によって変える
        const string jsonUrl = "https://script.google.com/macros/s/AKfycbzC4wrP9LkrLh8MJPvQUI3hBRYbEkcKeUsuyPrAnXKFQku8gnV1_1BhsKGflCcL0U_7/exec";
        const string outputDirectory = "Assets/StreamingAssets/Json";

        [MenuItem("Infrastructure/Tools/Spreadsheet マルチシートDL")]
        public static void ShowWindow()
        {
            DownloadAndSaveJsonAsync().Forget();
        }

        // メイン処理本体（UniTaskでawait可能）
        static async UniTask DownloadAndSaveJsonAsync()
        {
            UnityEngine.Debug.Log($"Request URL: [{jsonUrl}]");
            EditorUtility.DisplayProgressBar("Loading json", "Loading ...", 0);

            using var request = UnityWebRequest.Get(jsonUrl);
            request.SetRequestHeader("User-Agent", "Mozilla/5.0");
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.LogError("❌ 通信エラー: " + request.error);
                EditorUtility.ClearProgressBar();
                return;
            }

            try
            {
                var json = request.downloadHandler.text;
                var allSheets = JObject.Parse(json);

                if (Directory.Exists(outputDirectory))
                {
                    Directory.Delete(outputDirectory, true);
                }

                Directory.CreateDirectory(outputDirectory);

                foreach (var sheet in allSheets)
                {
                    var fileName = $"{sheet.Key}.json";
                    var filePath = Path.Combine(outputDirectory, fileName);
                    File.WriteAllText(filePath, sheet.Value.ToString());
                    UnityEngine.Debug.Log($"✅ 保存完了: {filePath}");
                }

                AssetDatabase.Refresh();
                UnityEngine.Debug.Log("✅ すべてのシートの保存完了");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"❌ 解析/保存エラー: {ex.Message}");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}

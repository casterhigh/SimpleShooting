using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.EditorTools
{
    public static class InputActionEnumGenerator
    {
        // 対象フォルダをここで指定
        private static readonly string[] SearchFolders = { "Assets" };

        [MenuItem("Infrastructure/Tools/Input Action Enums")]
        public static void Generate()
        {
            // 指定フォルダ配下だけ検索
            var guids = AssetDatabase.FindAssets("t:InputActionAsset", SearchFolders);
            if (guids.Length == 0)
            {
                UnityEngine.Debug.LogWarning($"⚠️ 指定フォルダに InputActionAsset が見つかりませんでした: {string.Join(", ", SearchFolders)}");
                return;
            }

            var outputDir = "Assets/Infrastructure/Input/Generated/Enums";
            Directory.CreateDirectory(outputDir);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(path);
                if (asset == null) continue;

                foreach (var map in asset.actionMaps)
                {
                    var fileName = $"{map.name}Actions.cs";
                    var outputPath = Path.Combine(outputDir, fileName);

                    using (var writer = new StreamWriter(outputPath, false))
                    {
                        writer.WriteLine("// Auto-generated. Do not edit manually.");
                        writer.WriteLine("namespace Infrastructure.Input.Generated.Enums");
                        writer.WriteLine("{");
                        writer.WriteLine($"    public enum {MakeSafe(map.name)}Actions");
                        writer.WriteLine("    {");

                        foreach (var action in map.actions.Distinct())
                        {
                            var safeName = MakeSafe(action.name);
                            writer.WriteLine($"        {safeName},");
                        }

                        writer.WriteLine("    }");
                        writer.WriteLine("}");
                    }

                    UnityEngine.Debug.Log($"✅ {map.name} → {outputPath}");
                }
            }

            AssetDatabase.Refresh();
        }

        // C# の Enum 名として安全な名前に変換
        private static string MakeSafe(string name)
        {
            var safe = new string(name.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
            if (string.IsNullOrEmpty(safe))
                safe = "Action";
            if (char.IsDigit(safe[0]))
                safe = "_" + safe;
            return safe;
        }
    }
}

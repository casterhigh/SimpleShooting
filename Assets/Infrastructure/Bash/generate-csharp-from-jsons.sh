#!/bin/bash

INPUT_DIR="../../StreamingAssets/Json"
OUTPUT_DIR="../../Generated/DAO"

mkdir -p "$OUTPUT_DIR"

# JSONファイルが1つも存在しない場合は終了
shopt -s nullglob
json_files=("$INPUT_DIR"/*.json)
if [ ${#json_files[@]} -eq 0 ]; then
  echo "⚠️ JSONファイルが見つかりませんでした: $INPUT_DIR"
  exit 0
fi
shopt -u nullglob

# 既存の DAO ファイルを削除
echo "🗑️ 既存のDAOファイルを削除: $OUTPUT_DIR"
rm -f "$OUTPUT_DIR"/*.cs

for json_file in "${json_files[@]}"; do
  base=$(basename "$json_file" .json)
  safe_name=$(echo "$base" | sed 's/[^a-zA-Z0-9]/_/g')
  class_name="${safe_name}DAO"
  output_file="$OUTPUT_DIR/${class_name}.cs"

  # quicktype でベースクラス生成
  quicktype \
    --lang csharp \
    --namespace Generated.DAO \
    -t "$class_name" \
    --src "$json_file" \
    --features just-types \
    --no-enums \
    -o "$output_file"

  echo "✅ $json_file → $output_file"

  # using を挿入（先頭にまとめて追加）
  sed -i '' '1s/^/using MessagePack;\
using LiteDB;\
\
/' "$output_file"

  # クラス定義直前に属性を挿入
  sed -i '' "/public partial class /i\\
    [MessagePackObject(true)]\\
    [JsonMapping(\"$base.json\")]
" "$output_file"

  # ★ struct 定義直前にも属性を挿入（追記）
  sed -i '' "/public partial struct /i\\
    [MessagePackObject(true)]
" "$output_file"

  # Id プロパティに [BsonId] を自動付加
  sed -i '' '/public .* Id { get; set; }/i\
        [BsonId]
' "$output_file"

done

IMPORTER_OUTPUT_DIR="../../Generated/Editor"
mkdir -p "$IMPORTER_OUTPUT_DIR"

REGISTRY_FILE="$IMPORTER_OUTPUT_DIR/ImporterRegistry.cs"

rm -f "$REGISTRY_FILE"

echo "// Auto-generated. Do not edit manually." > "$REGISTRY_FILE"
echo "using LiteDB;" >> "$REGISTRY_FILE"
echo "using Generated.DAO;" >> "$REGISTRY_FILE"
echo "using Infrastructure.EditorTools;" >> "$REGISTRY_FILE"
echo "namespace Generated.Editor {" >> "$REGISTRY_FILE"
echo "public static class ImporterRegistry {" >> "$REGISTRY_FILE"
echo "    public static void ImportAll(LiteDatabase db, string folderPath) {" >> "$REGISTRY_FILE"

# JSONファイルに対応する DAO クラスの ImportJsonFile 呼び出しを生成
for json_file in "$INPUT_DIR"/*.json; do
  base=$(basename "$json_file" .json)
  safe_name=$(echo "$base" | sed 's/[^a-zA-Z0-9]/_/g')
  class_name="${safe_name}Dao"
  echo "        JsonToLiteDbImporter.ImportJsonFile<${class_name}>(db, folderPath, \"${base}.json\", nameof(${class_name}));" >> "$REGISTRY_FILE"
done

echo "    }" >> "$REGISTRY_FILE"
echo "}" >> "$REGISTRY_FILE"
echo "}" >> "$REGISTRY_FILE"

echo "✅ ImporterRegistry.cs を生成: $REGISTRY_FILE"


SETTING_JSON_FILE="../../StreamingAssets/Json/Setting.json"
SETTING_OUTPUT_DIR="../../Generated/Setting"
mkdir -p "$SETTING_OUTPUT_DIR"

SETTING_FILE="$SETTING_OUTPUT_DIR/Setting.cs"
rm -f "$SETTING_FILE"

echo "// Auto-generated. Do not edit manually." > "$SETTING_FILE"
echo "namespace Generated {" >> "$SETTING_FILE"
echo "    public static class SettingKeys {" >> "$SETTING_FILE"

# jq で Key のみを抽出
jq -r '.[] | .Key' "$SETTING_JSON_FILE" | while read key; do
    # C# 定数名として安全に変換
    safeKey=$(echo "$key" | sed 's/[^a-zA-Z0-9_]/_/g')
    if [[ $safeKey =~ ^[0-9] ]]; then
        safeKey="_$safeKey"
    fi
    echo "        public const string $safeKey = \"$key\";" >> "$SETTING_FILE"
done

echo "    }" >> "$SETTING_FILE"
echo "}" >> "$SETTING_FILE"

echo "✅ $SETTING_FILE を生成しました"

MESSAGE_JSON_FILE="../../StreamingAssets/Json/MessageText.json"
MESSAGE_OUTPUT_DIR="../../Generated/MessageText"
mkdir -p "$MESSAGE_OUTPUT_DIR"

MESSAGE_FILE="$MESSAGE_OUTPUT_DIR/MessageText.cs"
rm -f "$MESSAGE_FILE"

echo "// Auto-generated. Do not edit manually." > "$MESSAGE_FILE"
echo "namespace Generated {" >> "$MESSAGE_FILE"
echo "    public static class MessageText {" >> "$MESSAGE_FILE"

# jq で Key のみを抽出
jq -r '.[] | .Key' "$MESSAGE_JSON_FILE" | while read key; do
    # C# 定数名として安全に変換
    safeKey=$(echo "$key" | sed 's/[^a-zA-Z0-9_]/_/g')
    if [[ $safeKey =~ ^[0-9] ]]; then
        safeKey="_$safeKey"
    fi
    echo "        public const string $safeKey = \"$key\";" >> "$MESSAGE_FILE"
done

echo "    }" >> "$MESSAGE_FILE"
echo "}" >> "$MESSAGE_FILE"

echo "✅ $MESSAGE_FILE を生成しました"

SE_JSON_FILE="../../StreamingAssets/Json/SE.json"
OUTPUT_DIR="../../Generated/Enums"
mkdir -p "$OUTPUT_DIR"

OUTPUT_FILE="$OUTPUT_DIR/Se.cs"
rm -f "$OUTPUT_FILE"

echo "// Auto-generated. Do not edit manually." > "$OUTPUT_FILE"
echo "namespace Generated.Enums {" >> "$OUTPUT_FILE"
echo "    public enum Se {" >> "$OUTPUT_FILE"

jq -r '.[] | "\(.Se) \(.Id)"' "$SE_JSON_FILE" | while read clip id; do
    safeClip=$(echo "$clip" | sed 's/[^a-zA-Z0-9_]/_/g')
    if [[ $safeClip =~ ^[0-9] ]]; then
        safeClip="_$safeClip"
    fi
    echo "        $safeClip = $id," >> "$OUTPUT_FILE"
done

echo "    }" >> "$OUTPUT_FILE"
echo "}" >> "$OUTPUT_FILE"

echo "✅ $OUTPUT_FILE を生成しました"

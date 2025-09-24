// Auto-generated. Do not edit manually.
using LiteDB;
using Generated.DAO;
using Infrastructure.EditorTools;
namespace Generated.Editor {
public static class ImporterRegistry {
    public static void ImportAll(LiteDatabase db, string folderPath) {
        JsonToLiteDbImporter.ImportJsonFile<BgmDao>(db, folderPath, "Bgm.json", nameof(BgmDao));
        JsonToLiteDbImporter.ImportJsonFile<SeDao>(db, folderPath, "Se.json", nameof(SeDao));
        JsonToLiteDbImporter.ImportJsonFile<SettingDao>(db, folderPath, "Setting.json", nameof(SettingDao));
    }
}
}

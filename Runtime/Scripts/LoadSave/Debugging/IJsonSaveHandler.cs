namespace LizardKit.LoadSave
{
    public interface IJsonSaveHandler
    {
        public void LoadJson(string json);
        public string CurrentSaveJson();
    }
}
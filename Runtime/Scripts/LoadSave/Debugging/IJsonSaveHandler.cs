namespace LizardKit.LoadSave.Debugging
{
    public interface IJsonSaveHandler
    {
        public void LoadJson(string json);
        public string CurrentSaveJson();
    }
}
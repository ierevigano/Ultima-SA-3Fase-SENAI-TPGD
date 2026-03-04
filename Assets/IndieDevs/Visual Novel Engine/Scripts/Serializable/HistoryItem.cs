namespace VisualNovelEngine
{
    [System.Serializable]
    public class HistoryItem
    {
        public string speakerName;
        public string dialogueText;

        public HistoryItem(string speakerName, string dialogueText)
        {
            this.speakerName = speakerName;
            this.dialogueText = dialogueText;
        }
    }
}
namespace UI
{
    public struct LogItem
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public LogItem(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Key}: {Value}";
        }
    }
}
namespace GameDealer
{
    public class SimpleJSONBuilder
    {
        private readonly System.Text.StringBuilder stringBuilder;
        private readonly System.Collections.Generic.List<int> fieldCounter;
        
        public SimpleJSONBuilder()
        {
            this.stringBuilder = new System.Text.StringBuilder();
            this.fieldCounter = new System.Collections.Generic.List<int>() { 0 };
        }

        public SimpleJSONBuilder Start()
        {
            this.stringBuilder.Append('{');
            return this;
        }

        public SimpleJSONBuilder End()
        {
            this.stringBuilder.Append('}');
            return this;
        }

        public void Clear()
        {
            this.stringBuilder.Clear();
            this.fieldCounter.Clear();
            this.fieldCounter.Add(0);
        }

        public override string ToString()
        {
            return this.stringBuilder.ToString();
        }

        public SimpleJSONBuilder Add(string name, string value)
        {
            this.StartField();
            this.AppendString(name);
            this.stringBuilder.Append(':');
            this.AppendString(value);
            return this;
        }

        public SimpleJSONBuilder Add(string name, int value)
        {
            this.StartField();
            this.AppendString(name);
            this.stringBuilder.Append(':');
            this.stringBuilder.Append(value);
            return this;
        }

        public SimpleJSONBuilder Add(string name, bool value)
        {
            this.StartField();
            this.AppendString(name);
            this.stringBuilder.Append(':');
            this.stringBuilder.Append(value ? "true" : "false");
            return this;
        }

        public SimpleJSONBuilder StartObject(string name)
        {
            this.StartField();
            this.AppendString(name);
            this.stringBuilder.Append(":{");
            this.fieldCounter.Add(0);
            return this;
        }

        public SimpleJSONBuilder EndObject()
        {
            this.stringBuilder.Append('}');
            this.fieldCounter.RemoveAt(fieldCounter.Count - 1);
            return this;
        }

        private void StartField()
        {
            if (this.fieldCounter[^1] > 0)
            {
                this.stringBuilder.Append(',');
            }

            this.fieldCounter[^1] += 1;
        }

        private void AppendString(string value)
        {
            this.stringBuilder.Append('\"').Append(value).Append('\"');
        }
    }
}

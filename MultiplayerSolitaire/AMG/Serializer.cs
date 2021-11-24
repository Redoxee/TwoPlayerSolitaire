namespace AMG
{
    public class Serializer
    {
        private Mode mode;

        System.IO.StreamWriter writer;
        System.IO.StreamReader reader;

        enum Mode
        {
            Read,
            Write,
        }

        public void StartWrite(System.IO.StreamWriter writer)
        {
            this.mode = Mode.Write;
            this.writer = writer;
        }

        public void StartRead(System.IO.StreamReader reader)
        {
            this.mode = Mode.Read;
            this.reader = reader;
        }

        public int Serialize(string name, int value)
        {
            if (this.mode == Mode.Write)
            {
                this.WriteInt(name, value);
            }
            else
            {
                return this.ReadInt(name);
            }

            return value;
        }

        public short Serialize(string name, short value)
        {
            if (this.mode == Mode.Write)
            {
                this.WriteShort(name, value);
            }
            else
            {
                return this.ReadShort(name);
            }

            return value;
        }

        public string Serialize(string name, string value)
        {
            if (this.mode == Mode.Write)
            {
                this.WriteString(name, value);
            }
            else
            {
                return this.ReadString(name);
            }

            return value;
        }

        public T Serialize<T>(string name, T value) where T : ISerializable
        {
            if (this.mode == Mode.Write)
            {
                this.WriteSerializable(name, value);
            }
            else
            {
                return this.ReadSerializable(name, value);
            }

            return value;
        }
        
        public T[] Serialize<T>(string name, T[] value) where T : ISerializable
        {
            if (this.mode == Mode.Write)
            {
                this.WriteSerializables(name, value);
            }
            else
            {
                return this.ReadSerializables(name, value);
            }

            return value;
        }

        #region int
        private void WriteInt(string name, int value)
        {
            this.writer.WriteLine(name);
            this.writer.Write(value);
            this.writer.WriteLine();
        }

        private int ReadInt(string name)
        {
            this.ReadName(name);
            string svalue = this.reader.ReadLine();

            if (int.TryParse(svalue, out int value))
            {
                return value;
            }

            System.Console.Error.WriteLine($"Error while reading {name}.");
            return int.MaxValue;
        }

        #endregion

        #region short
        private void WriteShort(string name, short value)
        {
            this.writer.WriteLine(name);
            this.writer.Write(value);
            this.writer.WriteLine();
        }

        private short ReadShort(string name)
        {
            this.reader.ReadLine();
            string svalue = this.reader.ReadLine();

            if (short.TryParse(svalue, out short value))
            {
                return value;
            }

            System.Console.Error.WriteLine($"Error while reading {name}.");
            return short.MaxValue;
        }

        #endregion

        #region string
        private void WriteString(string name, string value)
        {
            this.writer.WriteLine(name);
            this.writer.Write(value);
            this.writer.WriteLine();
        }

        private string ReadString(string name)
        {
            this.ReadName(name);

            string svalue = this.reader.ReadLine();
            return svalue;
        }

        #endregion

        #region Serializable
        private void WriteSerializable<T>(string name, T serializable) where T : ISerializable
        {
            this.writer.WriteLine(name);
            serializable.Serialize(this);
        }

        private T ReadSerializable<T>(string name, T serializable) where T : ISerializable
        {
            this.ReadName(name);

            serializable.Serialize(this);
            return serializable;
        }

        private void WriteSerializables<T>(string name, T[] array) where T : ISerializable
        {
            this.writer.WriteLine(name);
            int nb = array?.Length ?? 0;
            this.writer.Write(nb);
            this.writer.WriteLine();
            for (int index = 0; index < nb; ++index)
            {
                array[index].Serialize(this);
            }
        }

        private T[] ReadSerializables<T>(string name, T[] array) where T : ISerializable
        {
            this.ReadName(name);

            string slength = this.reader.ReadLine();
            if (!int.TryParse(slength, out int length))
            {
                System.Console.Error.WriteLine($"Error while reading {name}.");
                return null;
            }

            if (array == null)
            {
                array = new T[length];
            }
            else if (array.Length != length)
            {
                System.Array.Resize(ref array, length);
            }

            for (int index = 0; index < length; ++index)
            {
                array[index].Serialize(this);
            }

            return array;
        }

        #endregion

        private bool ReadName(string expectedName)
        {
            string rname = this.reader.ReadLine();
            if (rname != expectedName)
            {
                System.Console.Error.WriteLine($"Name missmatch : Expected \"{expectedName}\", read \"{rname}\"");
                return false;
            }

            return true;
        }
    }

    public interface ISerializable
    {
        void Serialize(Serializer serializer);
    }
}

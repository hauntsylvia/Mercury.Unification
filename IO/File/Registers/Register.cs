using Mercury.Unification.IO.File.Records;
using Newtonsoft.Json;

namespace Mercury.Unification.IO.File.Registers
{
    public class Register<T> : IRegister<T>
    {
        public static readonly DirectoryInfo DefaultLocation = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".Mercury"));

        public Register(string RegisterName)
        {
            if (!DefaultLocation.Exists)
            {
                DefaultLocation.Create();
            }

            this.Location = new(Path.Combine(DefaultLocation.FullName, RegisterName));
            if (!this.Location.Exists)
            {
                this.Location.Create();
            }
        }

        /// <summary>
        /// The fully qualified location of this register.
        /// </summary>
        public DirectoryInfo Location { get; }

        private FileInfo GetFileInfoFromKey(string Key)
        {
            return new(Path.Combine(this.Location.FullName, Key + ".mercury"));
        }

        public IRecord<T>? GetRecord(string Key)
        {
            FileInfo FileInfo = this.GetFileInfoFromKey(Key);
            if (FileInfo.Exists)
            {
                using StreamReader Reader = new(FileInfo.OpenRead());
                Record<T>? Attempt = JsonConvert.DeserializeObject<Record<T>>(Reader.ReadToEnd());
                return Attempt;
            }
            return null;
        }

        public void SaveRecord(string Key, IRecord<T> Value)
        {
            try
            {
                FileInfo FileInfo = this.GetFileInfoFromKey(Key);
                using StreamWriter Writer = new(FileInfo.FullName);
                Writer.Write(JsonConvert.SerializeObject(Value, Formatting.Indented));
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
                throw;
            }
        }

        public IReadOnlyCollection<IRecord<T>> GetAllRecords()
        {
            List<IRecord<T>> Records = new();
            foreach (FileInfo File in this.Location.GetFiles())
            {
                using StreamReader Reader = new(File.FullName);
                IRecord<T>? Attempt = JsonConvert.DeserializeObject<IRecord<T>>(Reader.ReadToEnd());
                if (Attempt != null)
                {
                    Records.Add(Attempt);
                }
            }
            return Records;
        }
        public IRecord<T>? DeleteRecord(string Key)
        {
            FileInfo FromKey = this.GetFileInfoFromKey(Key);
            if(FromKey.Exists)
            {
                IRecord<T>? Record = this.GetRecord(Key);
                FromKey.Delete();
                return Record;
            }
            return null;
        }

        public IRegister<TA> CreateSubRegister<TA>(string RegisterName)
        {
            DirectoryInfo Info = new(Path.Combine(this.Location.FullName, RegisterName));
            return new Register<TA>(Info.FullName);
        }
    }
}

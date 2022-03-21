using Mercury.Unification.IO.File.Records;
using Newtonsoft.Json;

namespace Mercury.Unification.IO.File.Registers
{
    public class Register<T> : IRegister<T>
    {
        public static readonly DirectoryInfo DefaultLocation = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".Mercury"));

        public Register(object RegisterName)
        {
            string? RegisterNameAsStr = RegisterName?.ToString();
            if(RegisterNameAsStr != null)
            {
                if (!DefaultLocation.Exists)
                {
                    DefaultLocation.Create();
                }

                this.Location = new(Path.Combine(DefaultLocation.FullName, RegisterNameAsStr));
                if (!this.Location.Exists)
                {
                    this.Location.Create();
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(RegisterName), "ToString method and object passed can not be null.");
            }
        }

        /// <summary>
        /// The fully qualified location of this register.
        /// </summary>
        public DirectoryInfo Location { get; }

        private FileInfo GetFileInfoFromKey(object Key)
        {
            return new(Path.Combine(this.Location.FullName, Key + ".mercury"));
        }

        public IRecord<T>? GetRecord(object Key)
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

        public void SaveRecord(object Key, IRecord<T> Value)
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
        public IRecord<T>? DeleteRecord(object Key)
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

        /// <summary>
        /// </summary>
        /// <typeparam name="TA"></typeparam>
        /// <param name="RegisterName"></param>
        /// <returns>A <see cref="Register{T}"/> within only one level lower in the directory of the instance using the given name. Null if null is passed as a parameter.</returns>
        public Register<TA>? GetSubRegister<TA>(object RegisterName)
        {
            string? RegisterNameAsStr = RegisterName?.ToString();
            if(RegisterNameAsStr != null)
            {
                DirectoryInfo Info = new(Path.Combine(this.Location.FullName, RegisterNameAsStr));
                return new Register<TA>(Info.FullName);
            }
            else
            {
                return null;
            }
        }
    }
}

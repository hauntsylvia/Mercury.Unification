using Newtonsoft.Json;

namespace Mercury.Unification.IO.File
{
    public class Register
    {
        public static readonly DirectoryInfo DefaultLocation = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".Mercury"));


        public Register(string RegisterName)
        {
            if (!DefaultLocation.Exists)
                DefaultLocation.Create();
            this.location = new(Path.Combine(DefaultLocation.FullName, RegisterName));
            if (!this.Location.Exists)
                this.Location.Create();
        }


        private readonly DirectoryInfo location;
        public DirectoryInfo Location => this.location;


        private FileInfo GetFileInfoFromKey(string Key)
        {
            return new(Path.Combine(this.Location.FullName, Key + ".mercury"));
        }


        public Record<T>? GetRecord<T>(string Key)
        {
            try
            {
                FileInfo FileInfo = this.GetFileInfoFromKey(Key);
                if (FileInfo.Exists)
                {
                    using StreamReader Reader = new(FileInfo.OpenRead());
                    Record<T>? Attempt = JsonConvert.DeserializeObject<Record<T>>(Reader.ReadToEnd());
                    return Attempt;
                }
                else
                    return null;
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
                throw;
            }
        }


        public void SaveRecord<T>(string Key, Record<T> Record)
        {
            try
            {
                FileInfo FileInfo = this.GetFileInfoFromKey(Key);
                using StreamWriter Writer = new(FileInfo.FullName);
                Writer.Write(JsonConvert.SerializeObject(Record, Formatting.Indented));
            }
            catch (Exception E)
            {
                Console.WriteLine(E);
                throw;
            }
        }
    }
}

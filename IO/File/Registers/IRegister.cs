using Mercury.Unification.IO.File.Records;

namespace Mercury.Unification.IO.File.Registers
{
    public interface IRegister<TInsideRecordType>
    {
        public IRecord<TInsideRecordType>? GetRecord(string Key);
        public IReadOnlyCollection<IRecord<TInsideRecordType>> GetAllRecords();
        public void SaveRecord(string Key, IRecord<TInsideRecordType> Value);
        public DirectoryInfo Location { get; }
    }
}

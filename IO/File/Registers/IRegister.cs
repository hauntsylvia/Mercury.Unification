using Mercury.Unification.IO.File.Records;

namespace Mercury.Unification.IO.File.Registers
{
    public interface IRegister<TInsideRecordType>
    {
        public IRecord<TInsideRecordType>? GetRecord(object Key);
        public IReadOnlyCollection<IRecord<TInsideRecordType>> GetAllRecords();
        public void SaveRecord(object Key, IRecord<TInsideRecordType> Value);
        public IRecord<TInsideRecordType>? DeleteRecord(object Key);
        public DirectoryInfo Location { get; }
    }
}

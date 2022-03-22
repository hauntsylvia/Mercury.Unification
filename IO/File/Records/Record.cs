namespace Mercury.Unification.IO.File.Records
{
    public class Record<T>
    {
        public Record(T ObjectToStore, IList<string>? Notes = null)
        {
            this.ObjectToStore = ObjectToStore;
            this.Notes = Notes;
        }

        public T ObjectToStore { get; }
        public IList<string>? Notes { get; }
        public DateTime UTCTimestamp { get; } = DateTime.UtcNow;
    }
}

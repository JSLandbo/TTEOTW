namespace ModelLibrary.Abstract.Types
{
    public abstract class AType : IType
    {
        public short ID { get; set; }
        public string Name { get; set; }
        public float Worth { get; set; }
        public float Weight { get; set; }
    }
}
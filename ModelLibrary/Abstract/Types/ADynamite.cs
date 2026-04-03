namespace ModelLibrary.Abstract.Types
{
    public enum ExplosionType
    {
        Dynamite,
        Nuclear
    }

    public abstract class ADynamite : AConsumeable
    {
        public int ExplosionAreaSize { get; set; }
        public int ExplosionPlaybackFrames { get; set; }
        public float Damage { get; set; }
        public float MaxHardness { get; set; }
        public ExplosionType ExplosionType { get; set; }
    }
}

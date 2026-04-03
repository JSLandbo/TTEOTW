namespace ModelLibrary.Abstract.Types
{
    public abstract class ADynamite : AConsumeable
    {
        public int ExplosionAreaSize { get; set; }
        public int ExplosionPlaybackFrames { get; set; }
        public float Damage { get; set; }
        public float MaxHardness { get; set; }
    }
}

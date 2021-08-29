namespace MSG
{
    [System.Flags]
    public enum Failures : ulong
    {
        None                    = 0,
        Unknown                 = 1 << 0,
        WrongOrder              = 1 << 1,
        WrongPlayer             = 1 << 2,
        CardOutOfBounds         = 1 << 3,
        GameEnded               = 1 << 4,
    }
}

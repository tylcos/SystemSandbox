using System.Collections.Generic;

[System.Serializable]
public class Dimension
{
    public string Name;
    public float Min;
    public float Max;
    public float Step;

    public override int GetHashCode()
    {
        var hashCode = 1941488653;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + Min.GetHashCode();
        hashCode = hashCode * -1521134295 + Max.GetHashCode();
        hashCode = hashCode * -1521134295 + Step.GetHashCode();
        return hashCode;
    }
}
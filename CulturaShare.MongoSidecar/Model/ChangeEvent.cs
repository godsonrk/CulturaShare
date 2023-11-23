namespace CulturalShare.MongoSidecar.Model;

public class ChangeEvent
{
    public BeforeModel? Before { get; set; }
    public AfterModel? After { get; set; }
    public dynamic Source { get; set; }
    public string Op { get; set; }
    public long TsMs { get; set; }
    public dynamic Transaction { get; set; }
}

public class AfterModel 
{ 
    public int Id { get; set; }
}

public class BeforeModel
{
    public int Id { get; set; }
}

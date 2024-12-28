using System;
using System.Collections.Generic;

[Serializable]
public class BuildRequirements
{
    public int BuildStage;
    public List<ResourceRequirements> Resources;
}
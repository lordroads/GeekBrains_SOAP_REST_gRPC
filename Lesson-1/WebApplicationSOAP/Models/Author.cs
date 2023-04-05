﻿namespace WebApplicationSOAP.Models;

[Serializable]
public class Author
{
    public string Name { get; set; }
    public string Lang { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Lang})";
    }
}

﻿namespace V2ray.Model
{
    public class Config
    {
        public string Name { get; set; } = "Server Name";

        public List<string> HostNames { get; set; }

        public int Port { get; set; }

        public string Type { get; set; }

        public string Net { get; set; }
    }
}
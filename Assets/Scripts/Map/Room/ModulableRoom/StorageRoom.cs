﻿namespace Scripts.Map.Room.ModulableRoom
{
    public class StorageRoom : AModulableRoom
    {
        public override string GetName()
            => "Factory";

        public override string GetDescription()
            => "All your materials are stored here, without it you won't be able to mine or build anything";
    }
}

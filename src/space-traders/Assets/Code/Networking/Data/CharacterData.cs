using System;


namespace Assets.Code.Networking.Data
{
    [Serializable]
    public struct CharacterData
    {
        public int Id;
        public string Name { get; set; }
    }
}

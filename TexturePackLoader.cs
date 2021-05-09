namespace FirstGame
{
    static class TexturePackLoader
    {
        static private string DEFAULT_TEXTUREPACK_PATH = "Content\\Pictures\\";
        static private string GALAXY_HIGHWAYS_PATH = "Content\\TexturePack - Galaxy Highways\\";

        static public string EnteringTexturePack(string texturePack)
        {
            switch (texturePack)
            {
                case "Galaxy Highways":
                    return GALAXY_HIGHWAYS_PATH;
                default:
                    return DEFAULT_TEXTUREPACK_PATH;
            }
        }
    }
}

using Godot;
using System;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class VoxelWorld : Spatial
    {
        private Array<string> textures = new Array<string>();
        private ImageTexture texture;
        private Dictionary<string, Vector2> textureCoords = new Dictionary<string, Vector2>();
        private Dictionary<string, Block> blocks = new Dictionary<string, Block>();

        public ImageTexture Texture { get => texture; }
        public Node chunkHolder;
        private PackedScene chunkScene;
        private const int VoxelUnitSize = 1;


        public override void _Ready()
        {
            chunkHolder = GetNode("Chunks");
            
        }

        private void BuildTileSet()
        {
            var texure = new ImageTexture();
            var img = new Image();
            var size = textures.Count * 64;
            var sizeX = 512;
            var sizeY = (size / 512 + 1) * 64;
            img.Create(sizeX, sizeY, false, Image.Format.Rgba8);

            for (int i = 0; i < textures.Count; i++)
            {
                var texture = textures[i];
                var resource = GD.Load<StreamTexture>(texture);
                var image = resource.GetData();
                image.Convert(Image.Format.Rgba8);
                var x = i == 0 ? 0 : i * image.GetWidth() % sizeX;
                var y = i == 0 ? 0 : i * image.GetWidth() / sizeX * 64;
                var coord = new Vector2(x, y);
                img.BlitRect(image, image.GetUsedRect(), coord);
                textureCoords[texture] = coord;
            }
            img.SavePng("res://textures/tileset.png");
            texure.CreateFromImage(img);
        }
        public void ModsInitialized(Array<Mod> mods)
        {
            chunkHolder = GetNode("Chunks");
            chunkScene = ResourceLoader.Load<PackedScene>("res://level/Voxel_Chunk.tscn");
            LoadMods(mods);
            BuildTileSet();

        }

        private void LoadMods(Array<Mod> mods)
        {
            foreach (var mod in mods)
            {
                LoadMod(mod);
            }
        }
        private void LoadMod(Mod mod)
        {
            var directory = new Directory();
            var error = directory.Open($"res://mods/{mod.Path}/blocks/");
            if (error != Error.Ok)
                return;
            directory.ListDirBegin(true);
            var fileName = directory.GetNext();
            while (!fileName.Empty())
            {
                if(!fileName.EndsWith("import")){
                    var path = $"res://mods/{mod.Path}/blocks/{fileName}";
                    var resource = GD.Load<CSharpScript>(path);
                    blocks[path] = (Block) resource.New();
                }
                fileName = directory.GetNext();
            }


            directory = new Directory();
            error = directory.Open($"res://mods/{mod.Path}/textures/");
            if (error != Error.Ok)
                return;
            directory.ListDirBegin(true);
            fileName = directory.GetNext();
            while (!fileName.Empty())
            {
                if (!fileName.EndsWith(".import"))
                {
                    var resource = GD.Load<StreamTexture>($"res://mods/{mod.Path}/textures/{fileName}");
                    if (resource != null)
                        textures.Add(resource.ResourcePath);
                }
                fileName = directory.GetNext();
            }
        }

        public void CreateWorld(Vector3 worldSize, Vector3 chunkSize){
            foreach(Node child in chunkHolder.GetChildren())
                child.QueueFree();
            for (int x = 0; x < worldSize.x; x++)
            {
                for (int y = 0; y < worldSize.y; y++)
                {
                    for (int z = 0; z < worldSize.z; z++)
                    {
                        var newChunk = chunkScene.Instance() as VoxelChunk;
                        chunkHolder.AddChild(newChunk);
                        newChunk.GlobalTransform = new Transform(newChunk.GlobalTransform.basis, new Vector3(x * chunkSize.x * VoxelUnitSize, y * chunkSize.y * VoxelUnitSize, z * chunkSize.z * VoxelUnitSize));
                        newChunk.Setup(this, chunkSize, VoxelUnitSize);
                    }
                }
            }
            GD.Print("Successfully created the world!");
        }

        public bool SetWorldVoxel(Vector3 position, string voxel)
        {
            var result = false;
            foreach (VoxelChunk chunk in chunkHolder.GetChildren())
            {
                result = chunk.SetVoxel(position, voxel);
                if(result)
                    break;
            }
            return result;
        }

        public Vector2 GetTextureCoord(string path)
        {
            return textureCoords[path];
        }

        public Block GetBlock(string path){
            return blocks[path];
        }
        public string Export(){
            Array<string> data = new Array<string>();
            foreach (VoxelChunk chunk in chunkHolder.GetChildren())
            {
                data.Add(chunk.Export());
            }
            return JSON.Print(data);
        }
        public void Load(string json){
            var result = JSON.Parse(json);
            if(result.Error != Error.Ok)
                return;
            Array<string> chunks = result.Result as Array<string>;
            foreach (var chunk in chunks)
            {
                
            }
        }
    }
}
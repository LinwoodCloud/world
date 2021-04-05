using Godot;
using System;
using Godot.Collections;

namespace LinwoodWorld.System
{
    public class VoxelWorld : Spatial
    {
        [Export]
        private NodePath modLoader;
        private Array<ModResources> modResources;
        private Array<string> textures = new Array<string>();
        private ImageTexture texture;
        private Dictionary<string, Vector2> textureCoords = new Dictionary<string, Vector2>();
        private Dictionary<string, Block> blocks = new Dictionary<string, Block>();

        public ImageTexture Texture { get => texture; }
        public Node chunkHolder;
        private PackedScene chunkScene;
        private const int VoxelUnitSize = 1;

        private void BuildTileSet()
        {
            var texure = new ImageTexture();
            var img = new Image();
            var size = textures.Count * 64;
            var sizeX = 512;
            var sizeY = (size / 512 + 1) * 64;
            GD.Print(sizeX, ", ", sizeY);
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
                GD.Print(coord, ", ", image.GetUsedRect());
                img.BlitRect(image, image.GetUsedRect(), coord);
                textureCoords[texture] = coord;
            }
            img.SavePng("res://textures/tileset.png");
            texure.CreateFromImage(img);
        }
        public override void _Ready()
        {
            chunkHolder = GetNode("Chunks");
            chunkScene = ResourceLoader.Load<PackedScene>("res://level/Voxel_Chunk.tscn");
            GD.Print(textureCoords);
            LoadMods();
            BuildTileSet();

        }

        private void LoadMods()
        {
            var modLoaderNode = GetNode<ModLoader>(modLoader);
            modLoaderNode.Setup();
            var mods = modLoaderNode.Mods;
            modResources = new Array<ModResources>();
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
                GD.Print(fileName);
                if (!fileName.EndsWith(".import"))
                {
                    var resource = GD.Load<StreamTexture>($"res://mods/{mod.Path}/textures/{fileName}");
                    if (resource != null)
                        textures.Add(resource.ResourcePath);
                }
                fileName = directory.GetNext();
            }
            GD.Print(textures);
            modResources.Add(new ModResources(mod, blocks));
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
        }

        internal void SetWorldVoxel(object v)
        {
            throw new NotImplementedException();
        }

        public Vector2 GetTextureCoord(String path)
        {
            return textureCoords[path];
        }
    }
}
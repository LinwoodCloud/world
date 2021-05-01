using Godot;
using LinwoodWorld.Level;
using System.Collections.Generic;
using System.Threading;

namespace LinwoodWorld.WorldSystem
{
	public class VoxelWorld : Spatial
	{
		private List<string> textures = new List<string>();
		private ImageTexture texture;
		private Dictionary<string, Vector2> textureCoords = new Dictionary<string, Vector2>();
		private Dictionary<string, Block> blocks = new Dictionary<string, Block>();

		public ImageTexture Texture { get => texture; }
		public Node chunkHolder;
		private PackedScene chunkScene;
		public int voxelUnitSize = 1;
		public Vector3 chunkSize = new Vector3(16, 16, 16);
		public Vector3 worldSize;
		[Signal]
		public delegate void WorldCreated();


		public override void _Ready()
		{
			chunkHolder = GetNode("Chunks");

		}

		private void BuildTileSet()
		{
			texture = new ImageTexture();
			var img = new Image();
			var size = textures.Count * 64;
			var sizeX = 256;
			var sizeY = (size / 256 + 1) * 64;
			img.Create(sizeX, sizeY, false, Image.Format.Rgba8);

			for (int i = 0; i < textures.Count; i++)
			{
				var texture = textures[i];
				var resource = GD.Load<StreamTexture>(texture);
				var image = resource.GetData();
				image.Decompress();
				image.Convert(Image.Format.Rgba8);
				var x = i == 0 ? 0 : i * image.GetWidth() % sizeX;
				var y = i == 0 ? 0 : i * image.GetWidth() / sizeX * 64;
				var coord = new Vector2(x, y);
				img.BlitRect(image, image.GetUsedRect(), coord);
				textureCoords[texture] = coord / 64f;
			}
			img.ClearMipmaps();
			img.SavePng("res://textures/tileset.png");
			texture.CreateFromImage(img);
			var flags = (Godot.Texture.FlagsEnum)texture.Flags;
			if (flags.HasFlag(Godot.Texture.FlagsEnum.Filter))
			{
				flags -= Godot.Texture.FlagsEnum.Filter;
			}
			texture.Flags = (uint)flags;
		}
		public void ModsInitialized(List<Mod> mods)
		{
			chunkHolder = GetNode("Chunks");
			chunkScene = ResourceLoader.Load<PackedScene>("res://level/Voxel_Chunk.tscn");
			LoadMods(mods);
			BuildTileSet();
			CreateWorld(new Vector3(4, 6, 4));
		}

		private void LoadMods(List<Mod> mods)
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
				if (!fileName.EndsWith("import"))
				{
					var path = $"res://mods/{mod.Path}/blocks/{fileName}";
					var resource = GD.Load<CSharpScript>(path);
					blocks[path] = (Block)resource.New();
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

		private List<System.Threading.Thread> threads = new List<System.Threading.Thread>();

		public void CreateWorld(Vector3 worldSize)
		{
			this.worldSize = worldSize;
			foreach (Node child in chunkHolder.GetChildren())
				child.QueueFree();
			for (int x = 0; x < worldSize.x; x++)
			{
				for (int y = 0; y < worldSize.y; y++)
				{
					for (int z = 0; z < worldSize.z; z++)
					{
						var position = new Vector3(x, y, z);
						var newChunk = chunkScene.Instance() as VoxelChunk;
						chunkHolder.AddChild(newChunk);
						newChunk.GlobalTransform = new Transform(newChunk.GlobalTransform.basis, position * chunkSize);
						newChunk.Setup(this, chunkSize, voxelUnitSize);
						var thread = new System.Threading.Thread(new ThreadStart( () => CreateChunk(newChunk)));
						thread.Start();
						threads.Add(thread);
					}
				}
			}
			GD.Print("Starting threading...");
			foreach (var thread in threads)
			{
				thread.Join();
			}
			GD.Print("Successfully created the world!");
			SpawnPlayer();
			EmitSignal(nameof(WorldCreated));
		}
		private void SpawnPlayer()
		{
			var playerScene = ResourceLoader.Load<PackedScene>("res://level/Player.tscn");
			var player = playerScene.Instance() as Player;
			player.Setup(this);
			CallDeferred("add_child", player);
		}
		private void CreateChunk(VoxelChunk chunk)
		{
			chunk.MakeStarterTerrain();
		}

		public bool SetWorldVoxel(Vector3 position, string voxel)
		{
			var result = false;
			foreach (VoxelChunk chunk in chunkHolder.GetChildren())
			{
				result = chunk.SetVoxel(chunk.GlobalTransform.XformInv(position), voxel);
				if (result)
					break;
			}
			return result;
		}
		public VoxelChunk GetChunk(Vector3 position){
			foreach (VoxelChunk chunk in chunkHolder.GetChildren())
			{
				if(chunk.VoxelInBounds(chunk.GlobalTransform.XformInv(position)))
					return chunk;
			}
			return null;
		}

		public Vector2 GetTexturePosition(string path)
		{
			return textureCoords[path];
		}

		public Block GetBlock(string path)
		{
			if (path == null || !blocks.ContainsKey(path))
				return null;
			return blocks[path];
		}
		public string Export()
		{
			List<string> data = new List<string>();
			foreach (VoxelChunk chunk in chunkHolder.GetChildren())
			{
				data.Add(chunk.Export());
			}
			return JSON.Print(data);
		}
		public void Load(string json)
		{
			var result = JSON.Parse(json);
			if (result.Error != Error.Ok)
				return;
			List<string> chunks = result.Result as List<string>;
			foreach (var chunk in chunks)
			{

			}
		}
		public List<string> GetBlockPaths() {
			return new List<string>(blocks.Keys);
		}
		public List<Block> GetBlocks(){
			return new List<Block>(blocks.Values);
		}
	}
}

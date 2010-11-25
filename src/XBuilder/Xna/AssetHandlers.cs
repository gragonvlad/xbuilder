using Microsoft.Xna.Framework.Content;
using XBuilder.ContentPreview.Rendering;
using XBuilder.Vsx;

namespace XBuilder.Xna
{
	public class AssetHandlers
	{
		private XBuilderPackage _package;
		private readonly ModelHandler _modelHandler;
		private readonly TextureHandler _textureHandler;

		public AssetHandlers(ContentManager contentManager, GraphicsDeviceControl graphicsDeviceControl)
		{
			_modelHandler = new ModelHandler(contentManager, graphicsDeviceControl);
			_textureHandler = new TextureHandler(contentManager, graphicsDeviceControl);
		}

		public void Initialize(XBuilderPackage package)
		{
			_package = package;
		}

		public AssetHandler GetAssetHandler(string fileName)
		{
			AssetType assetType = FileExtensionUtility.GetAssetType(_package, fileName);
			switch (assetType)
			{
				case AssetType.Effect :
					throw new System.NotImplementedException();
				case AssetType.Model :
					return _modelHandler;
				case AssetType.Texture :
					return _textureHandler;
				default :
					throw new System.NotSupportedException();
			}
		}
	}
}
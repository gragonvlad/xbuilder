using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XBuilder.ContentPreview.Rendering
{
	public class TextureRenderer : AssetRenderer
	{
		private SpriteBatch _spriteBatch;
		private Texture2D _texture;
		private Control _parentControl;
        private Rectangle _textureRectangle;
        private Rectangle _displayRectangle;

        private bool _fitToWindow;

		/// <summary>
		/// Gets or sets the current texture.
		/// </summary>
		public Texture2D Texture
		{
			get { return _texture; }

			set
			{
				_texture = value;

                ToggleTextureSize(_fitToWindow);
			}
		}

		public TextureRenderer(Control parentControl, GraphicsDevice graphicsDevice)
		{
			_parentControl = parentControl;
			_spriteBatch = new SpriteBatch(graphicsDevice);
            _fitToWindow = true;
            Resize();
            
            parentControl.Resize += (sender, e) =>
            {
                Resize();
                _parentControl.Invalidate();
            };
		}

        private void Resize()
        {
            _displayRectangle = new Rectangle(_parentControl.DisplayRectangle.X,
                                             _parentControl.DisplayRectangle.Y,
                                             _parentControl.DisplayRectangle.Width,
                                             _parentControl.DisplayRectangle.Height);
            
            ToggleTextureSize(_fitToWindow);
        }

		public override void ToggleTextureSize(bool show)
		{
			_fitToWindow = show;

			if (_texture == null)
				return;

			if (_fitToWindow)
			{
				_textureRectangle = ScaleToFit(_displayRectangle, _texture.Bounds);
			}
			else
			{
				// Centre texture within bounds.
				_textureRectangle = new Rectangle((_displayRectangle.Width - _texture.Bounds.Width) / 2,
					(_displayRectangle.Height - _texture.Bounds.Height) / 2,
					_texture.Bounds.Width,
					_texture.Bounds.Height);
			}
			_parentControl.Invalidate();
		}

		private Rectangle ScaleToFit(Rectangle destinationRectangle, Rectangle rectangleToScale)
        {
            Rectangle rect = destinationRectangle;
            rect.Inflate(-1, -1);

            double aspectRatioScale = rectangleToScale.Width / rectangleToScale.Height;
            double aspectRatioDest = destinationRectangle.Width / destinationRectangle.Height;

            if (aspectRatioScale > aspectRatioDest)
                rect.Height = (int)(rect.Width / aspectRatioScale);
            else
                rect.Width = (int)(rect.Height * aspectRatioScale);

            rect.X = destinationRectangle.X + (destinationRectangle.Width - rect.Width) / 2;
            rect.Y = destinationRectangle.Y + (destinationRectangle.Height - rect.Height) / 2;
            return rect;
        }

		public override void Draw(GraphicsDevice graphicsDevice)
		{
			_spriteBatch.Begin();
            _spriteBatch.Draw(_texture, _textureRectangle, Color.White);
			_spriteBatch.End();
		}
	}
}
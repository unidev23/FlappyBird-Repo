using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Bird
	{
		//Private variables.
		private static SpriteUV 	sprite;
		private static TextureInfo	textureInfo;
		private static int			pushAmount = 100;
		private static float		yPositionBeforePush;
		private static bool			rise;
		private static float		angle;
		private static bool			alive;
		private static float 		spriteWidth;
		private static float		spriteHeight;
	
		
		public float PositionX{ get{return sprite.Position.X;}}
		public float PositionY{ get{return sprite.Position.Y; }}
		public Vector2 Position{set {sprite.Position = value;}}
		public float SpriteWidth{ get{return spriteWidth;}}
		public float SpriteHeight{ get{return spriteHeight;}}
		
		
		
		public bool Alive { get{return alive;} set{alive = value;} }
		
		//Accessors.
		public SpriteUV Sprite { get{return sprite;} }
		
		//Public functions.
		public Bird (Scene scene)
		{
			textureInfo  = new TextureInfo("/Application/textures/space_ship.gif");
			
			sprite	 		= new SpriteUV();
			sprite 			= new SpriteUV(textureInfo);
			sprite.Quad.S 	= textureInfo.TextureSizef;
			sprite.Position = new Vector2(50.0f,Director.Instance.GL.Context.GetViewport().Height*0.5f);
			//sprite.Pivot 	= new Vector2(0.5f,0.5f);
			angle = 0.0f;
			rise  = false;
			alive = true;
			spriteWidth = 144.0f;
			spriteHeight = 80.0f;
			
			//Add to the current scene.
			scene.AddChild(sprite);
		}
		
		public void Dispose()
		{
			textureInfo.Dispose();
		}
		
		public void Update(float deltaTime)
		{			

			
			//adjust the push
			if(rise)
			{
				// Float variable for the position of the top of the ship
				float top = (sprite.Position.Y + SpriteHeight);
				
				
				if(top < Director.Instance.GL.Context.GetViewport().Height)
				{
					//sprite.Rotate(0.008f);
					if( (sprite.Position.Y-yPositionBeforePush) < pushAmount && top < Director.Instance.GL.Context.GetViewport().Height - 4)
						sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + 3f);
					else
						rise = false;
				}
				else
				{
					//Console.WriteLine ("In top Else!");
					sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - 3);
				}
			}
			else
			{
				float bottom = (sprite.Position.Y);
				//sprite.Rotate(-0.005f);
				if(bottom >= 0)
				{
					Console.WriteLine ("sprite.Position.Y = " + sprite.Position.Y);
					Console.WriteLine ("SpriteHeight = " + SpriteHeight);
					Console.WriteLine("Bottom = " + bottom);
					Console.WriteLine(Director.Instance.GL.Context.GetViewport().Height);
					
					sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - 3);
				}
				else
				{
					//Console.WriteLine ("In bottom Else!");
				}
			}
		}	
		
		public void Tapped()
		{
			if(!rise)
			{
				rise = true;
				yPositionBeforePush = sprite.Position.Y;
			}
		}
	}
}


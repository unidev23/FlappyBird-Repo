using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Obstacle
	{
		const float kGap = 200.0f;
		const int kNumOfPipes = 2;
		
		//Private variables.
		private SpriteUV[] 	sprites;
		private TextureInfo	textureInfoTop;
		private TextureInfo	textureInfoBottom;
		private float		width;
		private float		height;
		//Bounds2 topBounds;
		//Bounds2 bottomBounds;
		
		public float Width{ get{ return width; } }
		public float Height{ get{ return height; } }
		public float PositionX{ get{ return sprites[0].Position.X; } }
		public float PositionY{ get{ return sprites[0].Position.Y; } }
		
		//Accessors.
		//public SpriteUV SpriteTop 	 { get{return sprites[0];} }
		//public SpriteUV SpriteBottom { get{return sprites[1];} }
		
		//Public functions.
		public Obstacle (float startX, Scene scene)
		{
			textureInfoTop     = new TextureInfo("/Application/textures/toppipe.png");
			textureInfoBottom  = new TextureInfo("/Application/textures/bottompipe.png");
			
			sprites	= new SpriteUV[2];
			
			//Top
			sprites[0]			= new SpriteUV(textureInfoTop);
			sprites[0].Quad.S 	= textureInfoTop.TextureSizef;
			//Add to the current scene.
			scene.AddChild(sprites[0]);
			
			//Bottom
			sprites[1]			= new SpriteUV(textureInfoBottom);	
			sprites[1].Quad.S 	= textureInfoBottom.TextureSizef;		
			//Add to the current scene.
			scene.AddChild(sprites[1]);
			
			//Get sprite bounds.
			Bounds2 b = sprites[0].Quad.Bounds2();
			width  = b.Point10.X;
			height = b.Point01.Y;
			
			//Position pipes.
			sprites[0].Position = new Vector2(startX,
			                              Director.Instance.GL.Context.GetViewport().Height*RandomPosition());
			
			sprites[1].Position = new Vector2(startX, sprites[0].Position.Y-height-kGap);
			
		}
		
		public void Dispose()
		{
			// Destroys the allocated memory of the texture
			textureInfoTop.Dispose();
			textureInfoBottom.Dispose();
		}
		
		public void Update(float deltaTime)
		{			
			//Console.WriteLine(sprites[0].Position.Y);
			
			// Per frame, each tube will move left 3 pixels
			sprites[0].Position = new Vector2(sprites[0].Position.X - 3, sprites[0].Position.Y);
			sprites[1].Position = new Vector2(sprites[1].Position.X - 3, sprites[1].Position.Y);
			
			//If off the left of the viewport, loop them around.
			if(sprites[0].Position.X < -width)
			{
				// Creates the first tube to appear on the screen
				sprites[0].Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width,
			                              Director.Instance.GL.Context.GetViewport().Height*RandomPosition());
			
				// Creates the second tube to appear on the screen
				sprites[1].Position = new Vector2(Director.Instance.GL.Context.GetViewport().Width,
			                              sprites[0].Position.Y-height-kGap);
			}		
		}
		
		private float RandomPosition()
		{
			// Places the tubes into random positions
			Random rand = new Random();
			float randomPosition = (float)rand.NextDouble();
			randomPosition += 0.45f;
			
			if(randomPosition > 1.0f)
				randomPosition = 0.9f;
		
			return randomPosition;
		}
		
		public bool HasCollidedWith(SpriteUV bird)
		{
			Bounds2 b = bird.Quad.Bounds2();
			float birdWidth  = b.Point10.X;
			float birdHeight = b.Point01.Y;
			
			float birdLeft = bird.Position.X + (birdWidth*0.1f);
			float birdRight = bird.Position.X + (birdWidth*0.9f);
			float birdBottom = bird.Position.Y;
			float birdTop = birdBottom + birdHeight;
			
			for(int i = 0; i < kNumOfPipes; i++)
			{
				float pipeLeft = sprites[i].Position.X;
				float pipeRight = pipeLeft + width;
				float pipeBottom = sprites[i].Position.Y;
				float pipeTop = pipeBottom + height;
				
				if ( (birdBottom < pipeTop) && (birdTop > pipeBottom) &&
				     (birdRight > pipeLeft) && (birdLeft < pipeRight) )
					return true;
			}
			return false;
			
		}
	}
}


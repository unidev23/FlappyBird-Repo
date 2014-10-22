using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace FlappyBird
{
	public class Coin
	{
		const int kNumOfCoins = 2;
		
		// Private variables
		private static SpriteUV 	coin;
		private float		width;
		private float		height;
		private static TextureInfo	textureInfo;
		//private static TextureInfo	textureInfo;
		public float PositionX{ get{ return coin.Position.X; } }
		public float PositionY{ get{ return coin.Position.Y; } }
		
		public Coin (Scene scene, ref Obstacle obstacle)
		{
			// Load the texture info
			textureInfo  = new TextureInfo("/Application/textures/coin.png");
			
			// Creates a coin
			coin = new SpriteUV();
			coin = new SpriteUV(textureInfo);
			coin.Quad.S 	= textureInfo.TextureSizef;
			// Add coin to the scene
			scene.AddChild(coin);
			
			// Gives us the width and the height of the coin
			Bounds2 b = coin.Quad.Bounds2();
			width  = b.Point10.X;
			height = b.Point01.Y;
			
			// Position coin
			coin.Position = new Vector2((obstacle.PositionX + 40.0f), (obstacle.PositionY + -125.0f));
			
			
		}
		
		public bool HasCollidedWith(SpriteUV bird)
		{
			// Create bounds for the bird
			Bounds2 b = bird.Quad.Bounds2();
			float birdWidth  = b.Point10.X;
			float birdHeight = b.Point01.Y;
			
			float birdLeft = bird.Position.X + (birdWidth*0.1f);
			float birdRight = bird.Position.X + (birdWidth*0.9f);
			float birdBottom = bird.Position.Y;
			float birdTop = birdBottom + birdHeight;
			
			for(int i = 0; i < kNumOfCoins; i++)
			{
				float coinLeft = coin.Position.X;
				float coinRight = coinLeft + width;
				float coinBottom = coin.Position.Y;
				float coinTop = coinBottom + height;
				
				if ( (birdBottom < coinTop) && (birdTop > coinBottom) &&
				     (birdRight > coinLeft) && (birdLeft < coinRight) )
					return true; // Yes we have collided
			}
			return false; // No we haven't collided
		}
		
		public void Update(float deltaTime, ref Obstacle obstacle)
		{
			// Per frame, each coin will move left 3 pixels
			//coin.Position = new Vector2(coin.Position.X - 3, coin.Position.Y);
			coin.Position = new Vector2((obstacle.PositionX + 40.0f), (obstacle.PositionY + -125.0f));
			
			if(coin.Position.X < -width)
			{
				// If the coin is off the screen, reset it
				coin.Position = new Vector2((obstacle.PositionX + 40.0f), (obstacle.PositionY + -125.0f));
			}
		}
	}
}


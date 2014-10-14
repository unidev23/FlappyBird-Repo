using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
	
namespace FlappyBird
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		private static Sce.PlayStation.HighLevel.UI.Label				scoreLabel;
		
		private static Obstacle[]	obstacles;
		private static Bird			bird;
		private static Background	background;
				
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{
				Update ();
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			//Clean up after ourselves.
			bird.Dispose();
			foreach(Obstacle obstacle in obstacles)
				obstacle.Dispose();
			background.Dispose();
			
			Director.Terminate ();
		}

		public static void Initialize ()
		{
			//Set up director and UISystem.
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			scoreLabel = new Sce.PlayStation.HighLevel.UI.Label();
			scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
			scoreLabel.VerticalAlignment = VerticalAlignment.Top;
			scoreLabel.SetPosition(
				Director.Instance.GL.Context.GetViewport().Width/2 - scoreLabel.Width/2,
				Director.Instance.GL.Context.GetViewport().Height*0.1f - scoreLabel.Height/2);
			scoreLabel.Text = "0";
			panel.AddChildLast(scoreLabel);
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			//Create the background.
			background = new Background(gameScene);
			
			//Create the flappy douche
			bird = new Bird(gameScene);
			
			//Create some obstacles.
			obstacles = new Obstacle[2];
			obstacles[0] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width*0.5f, gameScene);	
			obstacles[1] = new Obstacle(Director.Instance.GL.Context.GetViewport().Width, gameScene);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{
			//Determine whether the player tapped the screen
			var touches = Touch.GetData(0);
			//If tapped, inform the bird.
			if(touches.Count > 0)
				bird.Tapped();
			
			//Update the bird.
			bird.Update(0.0f);
			
			// Check to see if the bird has collided with a tube
			for(int i = 0; i < 2; i++)
			{
				if(CollisionCheck(bird.PositionX, bird.PositionY, bird.SpriteWidth, bird.SpriteHeight, 
			               	   obstacles[i].PositionX, obstacles[i].PositionY, obstacles[i].Width, obstacles[i].Height))
				{
					//bird.Alive = false;
					//bird.PositionX = -100;
					//bird.PositionY = -100;
					bird.Position = new Vector2(-100, -100);
				}
			}
			
//			if(CollisionCheck(bird, obstacles))
//			{
//				Console.WriteLine ("Collision!!!");
//			}
			

			
			
			if(bird.Alive)
			{
				//Move the background.
				background.Update(0.0f);
							
				//Update the obstacles.
				foreach(Obstacle obstacle in obstacles)
					obstacle.Update(0.0f);
			}
		}
		
		public static bool CollisionCheck(float x1, float y1, float width1, float height1, float x2, float y2, float width2, float height2)
		{
			
			float left1 = x1;
			float left2 = x2;
			float right1 = x1 + width1;
			float right2 = x2 + width2;
			float top1 = y1;
			float top2 = y2;
			float bottom1 = y1 + height1;
			float bottom2 = y2 + height2;
			
			if (bottom1 < top2) 
				return false;
			if (top1 > bottom2)
				return false;
			if (right1 < left2) 
				return false;
			if (left1 > right2) 
				return false;

			return true;
		}

		
	}
}

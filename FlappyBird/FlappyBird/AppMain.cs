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
		
		// Create our Rectangle array of coins
		private static Coin[] 	coins;
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
			
			// Create some coins.
			coins = new Coin[2];
			coins[0] = new Coin(gameScene, ref obstacles[0]);
			coins[1] = new Coin(gameScene, ref obstacles[1]);
			
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
				if(obstacles[i].HasCollidedWith(bird.Sprite))
				{
					//bird.Alive = false;
					//Console.WriteLine ("Collided with Obstacle!");
				}
			}
			
			// Check to see if the bird has collided with a coin
			for(int i = 0; i < 2; i++)
			{
				if(coins[i].HasCollidedWith(bird.Sprite))
				{
					//bird.Alive = false;
					Console.WriteLine ("Collided with Coin!");
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
				
				coins[0].Update (0.0f, ref obstacles[0]);
				coins[1].Update (0.0f, ref obstacles[1]);
				
				//Update the coins
//				for(int i = 0; i < 2; i++)
//				{
//					coins[i].Update (0.0f, ref obstacles[i]);
//				}
			}
		}

		
	}
}

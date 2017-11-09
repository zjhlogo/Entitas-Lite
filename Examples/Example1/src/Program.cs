﻿using Entitas;
using System;
using System.Threading;


public class PositionComponent : IComponent
{
	public int x;
	public int y;
}

public class VelocityComponent : IComponent
{
	public int x;
	public int y;

	public void SetValue(int nx, int ny)
	{
		x = nx;
		y = ny;
	}
}


// Move each Entity's Position with Velocity
public class MovementSystem : IExecuteSystem
{
	public void Execute()
	{
		var context = Contexts.sharedInstance.defaultContext;

		var entities = context.GetEntities(Matcher.AllOf<PositionComponent, VelocityComponent>());
		foreach (var e in entities)
		{
			var pos = e.GetComponent<PositionComponent>();
			var vel = e.GetComponent<VelocityComponent>();

			pos.x += vel.x;
			pos.y += vel.y;
		}
	}
}

// Sample view just display Entity's Position
public class ViewSystem : IExecuteSystem
{
	public void Execute()
	{
		var context = Contexts.sharedInstance.defaultContext;

		var entities = context.GetEntities(Matcher.AllOf<PositionComponent>());
		foreach (var e in entities)
		{
			var pos = e.GetComponent<PositionComponent>();
			Console.WriteLine("Entity" + e.creationIndex + ": x="+pos.x+" y="+pos.y);
		}
	}
}


public class GameController
{
	private Systems _feature;

	public void Start()
	{
		var contexts = Contexts.sharedInstance;

		// create random entity
		var rand = new Random();
		var context = Contexts.sharedInstance.defaultContext;
		var e = context.CreateEntity();
			e.AddComponent<PositionComponent>();
			e.AddComponent<VelocityComponent>().SetValue(rand.Next()%10, rand.Next()%10);

		// init systems
		_feature = new Feature();
		_feature.Initialize();
	}

	public void Update()
	{
		_feature.Execute();
		_feature.Cleanup();
	}
}


namespace Example1
{
	class Program
	{
		static void Main(string[] args)
		{
			var game = new GameController();

			game.Start();

			Console.WriteLine("Press ESC to exit ...");
			Console.WriteLine();

			// main lopp for game
			while (true)
			{
				game.Update();

				Thread.Sleep(1000);

				if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
					break;
			}
		}
	}
}

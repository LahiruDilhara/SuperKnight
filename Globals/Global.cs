using Godot;
using System;
using System.ComponentModel.Design.Serialization;

public partial class Global : Node
{
	private static int Score = 0;

	private static int Hitpoints = 5;

	public static int getScore(){
		return Score;
	}

	public static void increaseScore(){
		Score++;
		
	}

	public static void resetScore(){
		Score = 0;
	}

	public static int getHitpoints(){
		return Hitpoints;
	}

	public static void reduceHitpoint(){
		--Hitpoints;
	}

	public static void increaseHitpoint(){
		++Hitpoints;
	}
}

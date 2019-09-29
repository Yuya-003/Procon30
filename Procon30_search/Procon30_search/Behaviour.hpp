#pragma once

class Behaviour {
public:
	enum class Action {
		move, remove, stay
	};

	enum class Dir {
		upLeft = 1, up, upRight, left, none, right, downLeft, down, downRight
	};

	Action action;
	Dir dir;

	Behaviour(Action a = Action::stay, Dir d = Dir::none) :action(a), dir(d) {}
};
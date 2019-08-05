#pragma once

class Behaviour {
public:
	enum Action {
		move, remove, stay
	};

	enum Dir {
		upLeft = 1, up, upRight, left, none, right, downLeft, down, downRight
	};

	Action action;
	Dir dir;
};
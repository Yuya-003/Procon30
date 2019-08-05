#pragma once

class Cell {
public:
	enum Status {
		none, ally, enemy
	};

	int point;
	Status status;
};
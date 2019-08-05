#pragma once

#include "Cell.hpp"
#include "Player.hpp"
#include "Position.hpp"

#include <cmath>
#include <vector>
#include <queue>
#include <algorithm>

class FieldInfo {
public:
	std::vector<std::vector<Cell>> field; //マス目上の配列
	std::vector<Player> allies; //味方エージェントの居場所・ID
	std::vector<Player> enemies; //相手エージェントの居場所・ID

	FieldInfo(size_t h = 0, size_t w = 0);
	size_t Width(); //横の幅
	size_t Height(); //縦の幅
};
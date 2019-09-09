#pragma once

#include "FieldInfo.hpp"
#include "Position.hpp"
#include "Behaviour.hpp"

#include <vector>
#include <array>
#include <cmath>
#include <fstream>
#include <sstream>
#include <iostream>

class BasicSearch {
public:

	class Node {
	public:
		enum Status {
			none, open, closed
		};

		Status status;
		Position pos;
		Cell cell;
		int cost; //実コスト = 今までかかったコスト
		int hCost; //ヒューリスティクスコスト = 目的地までの距離
		int score; //スコア = h_cost + cost - Cellのpoint
		Node* parent; //親ノード

		//ヒューリスティクスコストを計算
		void CalculateH(Position goal) {
			hCost = sqrt(pow(goal.x - pos.x, 2) + pow(goal.y - pos.y, 2));
		}
		//スコアを計算
		void CalculateScore() {
			score = hCost + cost - cell.point;
		}
	};

	const int searchLimit = 3;
	const int borderScore = 10;
	const int dir = 8;

	BasicSearch();

	virtual std::vector<std::vector<Behaviour>> Search(FieldInfo field);
};
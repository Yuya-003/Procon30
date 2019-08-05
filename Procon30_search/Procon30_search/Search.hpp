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
	};

	int searchLimit = 3;
	int borderScore = 10;

	BasicSearch();

	virtual std::vector<Behaviour> Search(FieldInfo field);

private:
	int CalculateH(Position goal, Position player);	//ヒューリスティクスコストを計算
	int CalculateScore(Node node);
};
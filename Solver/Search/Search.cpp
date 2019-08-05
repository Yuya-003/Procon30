#include "Search.hpp"

BasicSearch::BasicSearch() {
}

std::vector<Behaviour> BasicSearch::Search(FieldInfo field) {
	std::vector<std::vector<Node>> map(field.Height(), std::vector<Node>(field.Width(), Node()));

	std::vector<Node> nodes;
	for (int k = 0; k <= field.allies.size(); k++) {
		//目的地の設定
		Node goal;
		goal.cell.point = 1e-6;
		for (int i = 0; i < field.Height(); i++) {
			for (int j = 0; j < field.Width(); j++) {
				//ノードの初期化
				map[i][j].cell = field.field[i][j];
				map[i][j].status = Node::Status::none;
				map[i][j].pos = Position(j, i);
				//どちらのチームのものでもなく、10点以上のタイルで、最も左下側のものを目的地とする
				if (map[i][j].cell.status == Cell::none && map[i][j].cell.point >= borderScore) {
					goal = map[i][j];
				}
			}
		}

		Position startPos = Position(field.allies[k].x, field.allies[k].y);	//スタート地点を保存
		Position playerPos = startPos;	//現在位置を保存

		//スタート地点をopenに
		map[startPos.y][startPos.x].parent = nullptr;
		map[startPos.y][startPos.x].status = Node::Status::open;
		map[startPos.y][startPos.x].cost = 0;	//実コストを0に
		map[startPos.y][startPos.x].hCost = CalculateH(goal.pos, startPos);	//ヒューリスティクスコストを計算
		map[startPos.y][startPos.x].score = CalculateScore(map[startPos.y][startPos.x]);	//スコアを計算

		for (int dx = -1; dx <= 1; dx++) {
			for (int dy = -1; dy <= 1; dy++) {
				Position n = Position(playerPos.x + dx, playerPos.y + dy);
				if (map[n.y][n.x].status == Node::Status::none && map[n.y][n.x].cell.point > 1e-6) {
					map[n.y][n.x].parent = &map[playerPos.y][playerPos.x];
					map[n.y][n.x].status = Node::Status::open;
					map[n.y][n.x].cost = map[playerPos.y][playerPos.x].cost + 1;
					map[n.y][n.x].hCost = CalculateH(goal.pos, n);
					map[n.y][n.x].score = CalculateScore(map[n.y][n.x]);
				}
			}
		}

		/*
		nodes.push_back(map[playerPos.y][playerPos.x]);	//ノードの末尾に追加
		while (!nodes.empty()) {
			Node node = nodes[0];
			for (int i = 1; i < nodes.size(); i++) {
				if (node.score < nodes[i].score) {	//ノードの先頭のスコアと以降のスコアを比較→最大値を求める
					node = nodes[i];
				}
			}

			//以下にいろいろ追加

		}
		*/
	}

	//探索結果をまとめてリターン
	std::vector<Behaviour> behaviour;
	behaviour[0].action = Behaviour::Action::stay;
	behaviour[0].dir = Behaviour::Dir::none;
	behaviour[1].action = Behaviour::Action::stay;
	behaviour[1].dir = Behaviour::Dir::none;
	return behaviour;
}

//ヒューリスティクスコストを計算
int BasicSearch::CalculateH(Position goal, Position player) {
	int dx = goal.x - player.x;
	int dy = goal.y - player.y;

	if (dx >= dy) { return dx; }
	else { return dy; }
}

//スコアを計算
int BasicSearch::CalculateScore(Node node) {
	return node.hCost + node.cost - node.cell.point;
}

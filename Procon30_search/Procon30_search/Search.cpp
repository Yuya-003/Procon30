#include "Search.hpp"

BasicSearch::BasicSearch() {
}

//<演算子のオーバーロード(priority_queueで使用)
inline bool operator<(const BasicSearch::Node& a, const BasicSearch::Node& b) {
	return a.score > b.score;
}

std::vector<Behaviour> BasicSearch::Search(FieldInfo field, Player player) {
	int dx[8] = { 1,1,0,-1,-1,-1,0,1 };
	int dy[8] = { 0,1,1,1,0,-1,-1,-1 };
	std::vector<std::vector<Node>> map(field.Height(), std::vector<Node>(field.Width(), Node()));
	std::vector<std::vector<int>> dirMap(field.Height(), std::vector<int>(field.Width(), 0));
	std::vector<std::vector<int>> openList(field.Height(), std::vector<int>(field.Width(), 0));
	std::vector<std::vector<int>> closedList(field.Height(), std::vector<int>(field.Width(), 0));
	std::vector<Behaviour> behaviour;
	std::priority_queue<Node> nodes[2];
	Node node;
	int index = 0;
	int moveCount = 0;

	//目的地を計算
	Node goal;
	goal.cell.point = (int)1e-6;
	for (std::size_t i = 0; i < field.Height(); i++) {
		for (std::size_t j = 0; j < field.Width(); j++) {
			//マップを初期化
			map[i][j].cell = field.field[i][j];
			map[i][j].status = Node::Status::none;
			map[i][j].pos = Position(j, i);
			map[i][j].cost = 0;
			//占領されていない、最も点数の高いタイルを目的地とする
			if (map[i][j].cell.status == Cell::none && map[i][j].cell.point >= borderScore) {
				goal = map[i][j];
			}
		}
	}

	Position startPos = Position(player.x, player.y);	//初期位置を保存

	//行動の初期化
	for (size_t i = 0; i < 3; i++) {
		behaviour.push_back(Behaviour(Behaviour::Action::stay, Behaviour::Dir::none));
	}

	/*
	//初期位置の情報を修正
	map[startPos.y][startPos.x].status = Node::Status::open;
	map[startPos.y][startPos.x].cost = 0;
	map[startPos.y][startPos.x].CalculateH(goal.pos);
	map[startPos.y][startPos.x].CalculateScore();

	for (int dx = -1; dx <= 1; dx++) {
		for (int dy = -1; dy <= 1; dy++) {
			Position n = Position(startPos.x + dx, startPos.y + dy);
			if (n.x >= 0 && n.x < field.Width() && n.y >= 0 && n.y < field.Height()
				&& map[n.y][n.x].status == Node::Status::none && map[n.y][n.x].cell.point > 1e-6) {
				map[n.y][n.x].status = Node::Status::open;
				map[n.y][n.x].cost = map[startPos.y][startPos.x].cost + 1;
				map[n.y][n.x].CalculateH(goal.pos);
				map[n.y][n.x].CalculateScore();
			}
		}
	}*/

	node = map[startPos.y][startPos.x];
	node.CalculateH(goal.pos);
	node.CalculateScore();
	nodes[index].push(node);
	openList[field.Height() - 1][field.Width() - 1] = node.score;

	while (!nodes[index].empty()) {
		node = nodes[index].top();
		int x = node.pos.x;
		int y = node.pos.y;

		nodes[index].pop();

		openList[x][y] = 0;
		closedList[x][y] = 1;

		if ((x == goal.pos.x || fabs(x - goal.pos.x) == 1) && (y == goal.pos.y || fabs(y - goal.pos.y) == 1)) {
			std::vector<Behaviour> behavList;
			size_t i = 0;

			while (!(x == startPos.x && y == startPos.y)) {
				int j = dirMap[x][y];
				int route = route = Conversion((j + dir / 2) % 2);

				behavList.push_back(Behaviour(Behaviour::Action::move, static_cast<Behaviour::Dir>(route)));
				if (field.field[x][y].status == Cell::Status::enemy) { behavList[i].action = Behaviour::Action::remove; }

				x += dx[j];
				y += dy[j];

				if (i == dirMap.size()) { break; }
			}

			for (i = 0; i < 3; i++) {
				behaviour[i] = behavList[behavList.size() - (i + 1)];
			}

			while (!nodes[index].empty()) nodes[index].pop();
			moveCount = 0;
			break;
		}

		for (int i = 0; i < dir; i++) {
			std::size_t xdx = x + dx[i];
			std::size_t ydy = y + dy[i];

			if (!(xdx < 0 || xdx > field.Height() - 1 || ydy < 0 || ydy > field.Width() - 1
				|| map[xdx][ydy].cell.status == Cell::Status::enemy || closedList[xdx][ydy] == 1)) {
				Node m0;
				m0.pos = Position(xdx, ydy);
				m0.cost = node.cost;
				m0.score = node.score;
				m0.CalcureteCost(i);
				m0.CalculateH(goal.pos);
				m0.CalculateScore();

				if (openList[xdx][ydy] == 0) {
					openList[xdx][ydy] = m0.score;
					nodes[index].push(m0);
					dirMap[xdx][ydy] = (i + dir / 2) % dir;
					moveCount++;
				}
				else if (openList[xdx][ydy] > m0.score) {
					openList[xdx][ydy] = m0.score;
					dirMap[xdx][ydy] = (i + dir / 2) % dir;

					while (!(nodes[index].top().pos.x == xdx && nodes[index].top().pos.y == ydy)) {
						nodes[1 - index].push(nodes[index].top());
						nodes[index].pop();
					}
					nodes[index].pop();

					if (nodes[index].size() > nodes[1 - index].size()) { index = 1 - index; }
					while (!nodes[index].empty()) {
						nodes[1 - index].push(nodes[index].top());
						nodes[index].pop();
					}
					index = 1 - index;
					nodes[index].push(m0);
					moveCount++;
				}

				if (moveCount == 3) { break; }
			}
		}
	}

	//結果をリターン
	return behaviour;
}

inline int BasicSearch::Conversion(int num)
{
	int result = 5;
	if (num == 0) { result = 6; }
	else if (num == 1) { result = 9; }
	else if (num == 2) { result = 8; }
	else if (num == 3) { result = 7; }
	else if (num == 4) { result = 4; }
	else if (num == 5) { result = 1; }
	else if (num == 6) { result = 2; }
	else if (num == 7) { result = 3; }
	return result;
}

/*
std::vector<std::vector<Behaviour>> BasicSearch::Search(FieldInfo field) {
	int dx[8] = { 1,1,0,-1,-1,-1,0,1 };
	int dy[8] = { 0,1,1,1,0,-1,-1,-1 };
	std::vector<std::vector<Node>> map(field.Height(), std::vector<Node>(field.Width(), Node()));
	std::vector<std::vector<int>> dirMap(field.Height(), std::vector<int>(field.Width(), 0));
	std::vector<std::vector<int>> openList(field.Height(), std::vector<int>(field.Width(), 0));
	std::vector<std::vector<int>> closedList(field.Height(), std::vector<int>(field.Width(), 0));
	std::vector<std::vector<Behaviour>> behaviour(field.allies.size(), std::vector<Behaviour>(3, Behaviour()));
	std::priority_queue<Node> nodes[2];
	Node node;
	int index = 0;
	int moveCount = 0;

	for (std::size_t k = 0; k < field.allies.size(); k++) {
		//目的地を計算
		Node goal;
		goal.cell.point = (int)1e-6;
		for (std::size_t i = 0; i < field.Height(); i++) {
			for (std::size_t j = 0; j < field.Width(); j++) {
				//マップを初期化
				map[i][j].cell = field.field[i][j];
				map[i][j].status = Node::Status::none;
				map[i][j].pos = Position(j, i);
				map[i][j].cost = 0;
				//占領されていない、最も点数の高いタイルを目的地とする
				if (map[i][j].cell.status == Cell::none && map[i][j].cell.point >= borderScore) {
					goal = map[i][j];
				}
			}
		}

		Position startPos = Position(field.allies[k].x, field.allies[k].y);	//初期位置を保存
		/*
		//初期位置の情報を修正
		map[startPos.y][startPos.x].status = Node::Status::open;
		map[startPos.y][startPos.x].cost = 0;
		map[startPos.y][startPos.x].CalculateH(goal.pos);
		map[startPos.y][startPos.x].CalculateScore();
		
		for (int dx = -1; dx <= 1; dx++) {
			for (int dy = -1; dy <= 1; dy++) {
				Position n = Position(startPos.x + dx, startPos.y + dy);
				if (n.x >= 0 && n.x < field.Width() && n.y >= 0 && n.y < field.Height()
					&& map[n.y][n.x].status == Node::Status::none && map[n.y][n.x].cell.point > 1e-6) {
					map[n.y][n.x].status = Node::Status::open;
					map[n.y][n.x].cost = map[startPos.y][startPos.x].cost + 1;
					map[n.y][n.x].CalculateH(goal.pos);
					map[n.y][n.x].CalculateScore();
				}
			}
		}
		
		node = map[startPos.y][startPos.x];
		node.CalculateH(goal.pos);
		node.CalculateScore();
		nodes[index].push(node);
		openList[field.Height() - 1][field.Width() - 1] = node.score;

		while (!nodes[index].empty()) {
			node = nodes[index].top();
			int x = node.pos.x;
			int y = node.pos.y;

			nodes[index].pop();

			openList[x][y] = 0;
			closedList[x][y] = 1;

			/*
			for (size_t i = 1; i < nodes[index].size(); i++) {
				if (!nodes[i].empty()) {
					for (size_t j = 0; j < nodes[i].size(); i++) {
						std::cout << nodes[i].top().pos.x << "," << nodes[i].top().pos.y << " " << std::endl;
					}
					std::cout << std::endl;
					int topScore = nodes[i].top().score;
					if (node.score < topScore) {
						node = nodes[i].top();
					}
				}
			}

			if ((x == goal.pos.x || fabs(x - goal.pos.x) == 1) && (y == goal.pos.y || fabs(y - goal.pos.y) == 1)) {
				int i = 0;
				for (size_t i = 0; i < 3; i++) {
					int j = dirMap[x][y];
					char c = '0' + (10 - (j + dir / 2) % 2);

					behaviour[k][i].action = Behaviour::Action::move;
					behaviour[k][i].dir = static_cast<Behaviour::Dir>(10 - (dirMap[x][y] + dir / 2) % 2);

					x += dx[j];
					y += dy[j];

					if (i == dirMap.size()) { break; }
				}

				while (!nodes[index].empty()) nodes[index].pop();
				moveCount = 0;
				break;
			}

			for (int i = 0; i < dir; i++) {
				std::size_t xdx = x + dx[i];
				std::size_t ydy = y + dy[i];

				if (!(xdx < 0 || xdx > field.Height() - 1 || ydy < 0 || ydy > field.Width() - 1 
					|| map[xdx][ydy].cell.status == Cell::Status::enemy || closedList[xdx][ydy] == 1)) {
					Node m0;
					m0.pos = Position(xdx, ydy);
					m0.cost = node.cost;
					m0.score = node.score;
					m0.CalcureteCost(i);
					m0.CalculateH(goal.pos);
					m0.CalculateScore();

					if (openList[xdx][ydy] == 0) {
						openList[xdx][ydy] = m0.score;
						nodes[index].push(m0);
						dirMap[xdx][ydy] = (i + dir / 2) % dir;
						moveCount++;
					}
					else if (openList[xdx][ydy] > m0.score) {
						openList[xdx][ydy] = m0.score;
						dirMap[xdx][ydy] = (i + dir / 2) % dir;

						while (!(nodes[index].top().pos.x == xdx && nodes[index].top().pos.y == ydy)) {
							nodes[1 - index].push(nodes[index].top());
							nodes[index].pop();
						}
						nodes[index].pop();

						if (nodes[index].size() > nodes[1 - index].size()) { index = 1 - index; }
						while (!nodes[index].empty()) {
							nodes[1 - index].push(nodes[index].top());
							nodes[index].pop();
						}
						index = 1 - index;
						nodes[index].push(m0);
						moveCount++;
					}

					if (moveCount == 3) { break; }
				}
			}
		}
	}

	//結果をリターン
	return behaviour;
}
*/
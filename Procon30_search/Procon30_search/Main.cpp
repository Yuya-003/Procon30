#include "Search.hpp"
#include "Main.hpp"
#include <fstream>
#include <sstream>

int main() {
	std::string str;
	size_t i, j;

	//探索結果があるか確認
	std::ifstream resultIfs("./result.txt");
	resultIfs >> str;

	if (str == "none") {
		BasicSearch bs;
		std::string text;
		std::vector<std::vector<std::string>> splited;
		std::vector<std::vector<int>> field;

		//盤面データ受け取り
		std::ifstream fieldIfs("./field.txt");

		//文字を分割
		while (fieldIfs >> text) { splited.push_back(SplitLine(text, ',')); }

		//文字を数値に変換
		for (i = 0; i < splited.size(); i++) {
			std::vector<int> line;
			for (j = 0; j < splited[i].size(); j++) {
				line.push_back(std::stoi(splited[i][j]));
			}
			field.push_back(line);
		}

		//盤面のサイズを初期化
		size_t x = field[0][0], y = field[0][1];
		FieldInfo fi(x, y);

		//ポイントと状態を初期化
		for (i = 0; i < y; i++) {
			for (j = 0; j < x; j++) {
				fi.field[i][j].point = field[i + 1][j];
				fi.field[i][j].status = Cell::none;
			}
		}

		//エージェントの位置とIDを初期化
		while (++i < field.size()) {
			Player pl;
			pl.id = field[i][0];
			pl.x = field[i][1] - 1;
			pl.y = field[i][2] - 1;
			fi.allies.push_back(pl);
		}

		//探索
		std::vector<std::vector<Behaviour>> result = bs.Search(fi);

		std::ofstream fieldOfs("./field.txt");
		std::ofstream resultOfs("./result.txt");

		//探索結果を出力 → ID:行動:方向
		for (i = 0; i < fi.allies.size(); i++) {
			int temp = static_cast<int>(result[i][0].action);
			fieldOfs << fi.allies[i].id << " : " << temp;
			if (result[i][0].action != Behaviour::Action::stay) {
				temp = static_cast<int>(result[i][0].dir);
				resultOfs << " : " << temp;
			}
			resultOfs << std::endl;
		}

		//2ターン先の行動をターン毎に保存
		for (i = 1; i < result[i].size(); i++) {
			resultOfs << "[" << i << "]" << std::endl;
			for (j = 0; i < fi.allies.size(); j++) {
				std::cout << static_cast<int>(result[j][i].action) << std::endl;
				int temp = static_cast<int>(result[j][i].action);	//error:out of range
				resultOfs << fi.allies[j].id << ":" << temp << ":";
				if (result[j][i].action != Behaviour::Action::stay) {
					temp = static_cast<int>(result[j][i].dir);
					resultOfs << temp;
				}
				resultOfs << std::endl;
			}
		}
	}
	else {
		//保存された探索結果を分離
		std::string field;
		size_t i;
		std::vector<std::vector<std::string>> result;
		std::ofstream resultOfs("./result.txt");
		std::ofstream fieldOfs("./field.txt");

		result.push_back(SplitLine(str, ':'));
		while (resultIfs >> field) { result.push_back(SplitLine(field, ':')); }

		//結果出力
		for (i = 1; i < result.size(); i++) {
			if (result[i][0] == "[2]") { break; }

			field = result[i][0] + ":" + result[i][1];
			if (result[i][1] != "stay") {
				field += (" : " + result[i][j]);
			}
			field += "\n";
			fieldOfs << field;
		}

		if (result[0][0] == "[2]") {
			resultOfs << "none";
		}
		else {
			resultOfs << "[2]";
			while (++i && i < result.size()) {
				field = result[i][0] + ":" + result[i][1];
				if (result[i][1] != "stay") {
					field += (" : " + result[i][j]);
				}
				field += "\n";
				resultOfs << field;
			}
		}
	}
}
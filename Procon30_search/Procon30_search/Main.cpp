#include "Search.hpp"
#include <fstream>
#include <sstream>

std::vector<std::string> SplitLine(const std::string&, char);

int main() {
	std::string str;

	//探索結果があるか確認
	std::fstream resultData("./result.txt", std::ios::in | std::ios::out);
	resultData >> str;

	if (str == "none") {
		BasicSearch bs;
		std::string text;
		std::vector<std::vector<std::string>> splited;
		std::vector<std::vector<int>> field;

		//盤面データ受け取り
		std::fstream fieldData("./field.txt", std::ios::in | std::ios::out);
		fieldData >> text;

		//FieldInfoに変換
		//文字を分割
		std::vector<std::string> temp = SplitLine(text, '\n');
		for (size_t i = 0; i < temp.size(); i++) {
			splited.push_back(SplitLine(temp[i], ','));

		}

		//文字を数値に変換
		for (size_t i = 0; i < splited.size(); i++) {
			std::vector<int> line;
			for (size_t j = 0; j < splited[i].size(); j++) {
				line.push_back(std::stoi(splited[i][j]));
			}
			field.push_back(line);
		}

		//盤面のサイズを初期化
		size_t x = field[0][0], y = field[0][1];
		FieldInfo fi(x, y);

		//ポイントと状態を初期化
		size_t i, j;
		for (i = 0; i < y; i++) {
			for (j = 0; j < x; j++) {
				fi.field[i][j].point = field[i + 1][j];
				fi.field[i][j].status = Cell::none;
			}
		}

		//エージェントの位置とIDを初期化
		while (i < field.size()) {
			Player pl;
			pl.id = field[i][0];
			pl.x = field[i][1] - 1;
			pl.y = field[i][2] - 1;
			fi.allies.push_back(pl);

			i++;
		}

		//探索
		std::vector<std::vector<Behaviour>> result = bs.Search(fi);

		//探索結果を出力 → ID:行動:方向
		for (int i = 0; i < fi.allies.size(); i++) {
			int temp = static_cast<int>(result[i][0].action);
			fieldData << fi.allies[i].id << " : " << temp;
			if (result[i][0].action != Behaviour::Action::stay) {
				temp = static_cast<int>(result[i][0].dir);
				resultData << " : " << temp;
			}
			resultData << std::endl;
		}

		//2ターン先の行動をターン毎に保存
		for (int i = 1; i < result[i].size(); i++) {
			resultData << "[" << i << "]" << std::endl;
			for (int j = 0; i < fi.allies.size(); j++) {
				int temp = static_cast<int>(result[j][i].action);
				resultData << fi.allies[j].id << ":" << temp << ":";
				if (result[j][i].action != Behaviour::Action::stay) {
					temp = static_cast<int>(result[j][i].dir);
					resultData << temp;
				}
				resultData << std::endl;
			}
		}
	}
	else {
		//保存された探索結果を分離
		std::string field;
		std::vector<std::string> result;
		while (std::getline(std::istringstream(str), field, '\n')) {
			result.push_back(field);
		}

		//結果出力
		std::fstream fieldData("./field.txt", std::ios::out);
		for (int i = 1; i < result.size(); i++) {
			if (result[i] == "[2]") { 
				for (int j = i; j < result.size(); j++) {
					resultData << result[j] << std::endl;
				}
				break; 
			}

			fieldData << result[i] << std::endl;

			if (i == result.size() - 1) { resultData << "none"; }
		}
	}
}

std::vector<std::string> SplitLine(const std::string& str, char delim)
{
	std::vector<std::string> splited;
	std::stringstream ss(str);
	std::string buf;

	while (std::getline(ss, buf, delim)) {
		splited.push_back(buf);
	}

	return splited;
}

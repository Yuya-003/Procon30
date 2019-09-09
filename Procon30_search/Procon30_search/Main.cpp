#include "Search.hpp"
#include <fstream>
#include <sstream>>

void main() {
	std::string str;

	//探索結果があるか確認
	std::fstream resultData("./result.txt", std::ios::in | std::ios::out);
	resultData >> str;

	if (str == "none") {
		BasicSearch bs;
		FieldInfo fi;

		//盤面データ受け取り
		std::fstream fieldData("./field.txt", std::ios::in | std::ios::out);
		fieldData >> str;

		//FieldInfoに変換

		//探索
		std::vector<std::vector<Behaviour>> result = bs.Search(fi);

		//探索結果を出力 → ID:行動:方向
		for (int i = 0; i < fi.allies.size(); i++) {
			fieldData << fi.allies[i].id << " : " << result[i][0].action;
			if (result[i][0].action != Behaviour::Action::stay) {
				resultData << " : " << result[i][0].dir;
			}
			resultData << std::endl;
		}

		//2ターン先の行動をターン毎に保存
		for (int i = 1; i < result[i].size(); i++) {
			resultData << "[" << i << "]" << std::endl;
			for (int j = 0; i < fi.allies.size(); j++) {
				resultData << fi.allies[j].id << ":" << result[j][i].action << ":";
				if (result[j][i].action != Behaviour::Action::stay) {
					resultData << result[j][i].dir;
				}
				resultData << std::endl;
			}
		}
	}
	else {
		//保存された探索結果を分離
		std::istringstream stream(str);
		std::string field;
		std::vector<std::string> result;
		while (std::getline(stream, field, '\n')) {
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
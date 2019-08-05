#include "Search.hpp"
#include <fstream>
#include <sstream>>

void main() {
	std::string str;

	//探索結果があるか確認
	std::fstream resultData("./result.txt", std::ios::in | std::ios::out);
	resultData >> str;

	if (str == "none") {
		//探索した結果を分離
		std::istringstream stream(str);
		std::string field;
		std::vector<std::string> result;
		while (std::getline(stream, field, '\n')) {
			result.push_back(field);
		}

		//結果出力
		resultData << result[0];

		resultData.clear();
		if (result.size() >= 2) {
			for (int i = 1; i < result.size(); i++) {
				resultData << result[i];
			}
		}
		else {
			resultData << "none";
		}
	}
	else {
		BasicSearch bs;
		FieldInfo fi;

		//盤面データ受け取り
		std::fstream fieldData("./field.txt", std::ios::in | std::ios::out);
		fieldData >> str;

		//FieldInfoに変換

		//探索
		std::vector<Behaviour> result = bs.Search(fi);

		//探索結果を出力 → ID:行動:方向
		fieldData.clear();
		for (int i = 0; i < fi.allies.size(); i++) {
			fieldData << fi.allies[i].id << ":" << result[i].action << ":";
			if (result[i].action != Behaviour::Action::stay) {
				fieldData << result[i].dir;
			}
			fieldData << std::endl;
		}
	}
}
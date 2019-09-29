#include "Search.hpp"
#include <fstream>
#include <sstream>

int main() {
	std::string str;

	//探索結果があるか確認
	std::fstream resultData("./result.txt", std::ios::in | std::ios::out);
	resultData >> str;

	if (str == "none") {
		BasicSearch bs;
		FieldInfo fi;
		std::string text, field;

		//盤面データ受け取り
		std::fstream fieldData("./field.txt", std::ios::in | std::ios::out);
		fieldData >> text;

		//FieldInfoに変換
		while (std::getline(std::istringstream(text),field, '\n')) {
			while (std::getline(std::istringstream(field), text, ' ')) {

			}
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
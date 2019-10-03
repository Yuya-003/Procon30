#pragma once
//•¶Žš—ñ‚ð•ªŠ„‚·‚é
inline std::vector<std::string> SplitLine(const std::string& str, char delim)
{
	std::vector<std::string> splited;
	std::stringstream ss(str);
	std::string buf;

	while (std::getline(ss, buf, delim)) {
		splited.push_back(buf);
	}

	return splited;
}

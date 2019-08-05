#include "FieldInfo.hpp"

FieldInfo::FieldInfo(size_t h, size_t w)
{
	for (int i = 0; i < h; i++) {
		this->field.push_back(std::vector<Cell>(w));
	}
}

size_t FieldInfo::Width()
{
	return this->field[0].size();
}

size_t FieldInfo::Height()
{
	return this->field.size();
}